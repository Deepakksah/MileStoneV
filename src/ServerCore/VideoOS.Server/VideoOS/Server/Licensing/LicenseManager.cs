using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using Newtonsoft.Json;
using VideoOS.Common.Crypto;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Configuration;
using VideoOS.Common.Integration.LicenseInfo;
using VideoOS.Common.Integration.Licensing;
using VideoOS.Common.Interfaces.Licensing;
using VideoOS.Licensing;
using VideoOS.Server.DataComponents;
using VideoOS.Utilities.Timers;

namespace VideoOS.Server.Licensing;

public sealed class LicenseManager : ILicenseManager, ILicenseFeatureProvider, ILicenseRequestFactory, IDisposable
{
	private readonly IDataComponentFactory _dataComponentFactory;

	private readonly ILicenseDataSetComponent _licenseDataSetComponent;

	private readonly ITemporaryLicenseStorage _temporaryLicenseStorage;

	private readonly ILicenseManagerCache _licenseManagerCache;

	private readonly ILicenseManagerComponentsProvider _componentsProvider;

	private readonly IServerEnvironment _serverEnvironment;

	private ILicenseDocument _licenseDocument;

	private IActivationManager _activationManager;

	private readonly IOemResources _oemResources;

	private ITimer _licenseTimer;

	private readonly ICachedLicenseInfo _cachedLicenseInfo;

	private string _licenseOwnerXml;

	private string _sitesInformationXml;

	private ILicenseAllocator _licenseAllocator;

	private IHardwareLicenseCountEvaluator _hardwareLicenseCountEvaluator;

	private static object _retrieveLicenseAllocatorLock = new object();

	private readonly object _obtainLicenseLock = new object();

	private DateTime _cachedVerifyTime = DateTime.MinValue;

	public int TotalAllowedChangesWithoutActivation { get; private set; }

	public IRecordersRequiringLicenseUpdateDictionary RecordersRequiringUpdate { get; private set; }

	public ILicenseCountManager CountManager { get; private set; }

	public string LicenseOwnerXml
	{
		get
		{
			return _licenseOwnerXml;
		}
		set
		{
			if (_cachedLicenseInfo.HasCachedLicenseInfo)
			{
				_cachedLicenseInfo.CachedLicenseInfo.Owner = LicenseOwnerInformation.FromXml(value);
			}
			_licenseOwnerXml = value;
		}
	}

	public string SitesInformationXml
	{
		get
		{
			return _sitesInformationXml;
		}
		set
		{
			if (_cachedLicenseInfo.HasCachedLicenseInfo)
			{
				_cachedLicenseInfo.CachedLicenseInfo.SitesLicenseXml = SitesLicenseXml.FromXml(value);
			}
			_sitesInformationXml = value;
		}
	}

	public string EncryptedSlc => _licenseDocument.EncryptedSlc;

	public string ProductName => _licenseDocument.ProductName;

	public string CustomerName => _licenseDocument.CustomerName;

	public string SoftwareUpgradePlanExpireDate => _licenseDocument.SoftwareUpgradePlanExpireDate;

	public bool IsLicenseExpired => _licenseDocument.IsLicenseExpired;

	public DateTime LicenseActivatedDate => _licenseDocument.ActivatedDate;

	public bool LicenseExpires => _licenseDocument.ExpiryDate != DateTime.MinValue;

	public DateTime LicenseExpiryDate => _licenseDocument.ExpiryDate;

	public int TotalReplacementLimit
	{
		get
		{
			if (IsFeatureLicensed("HardwareLicensing"))
			{
				return _licenseDocument.ReplacementLimit.CalculateTotalReplacementLimit(CountManager.GetHardwareUsedOnSite(icp: false));
			}
			return _licenseDocument.ReplacementLimit.CalculateTotalReplacementLimit(CountManager.CountOfLicensesUsed(isInterconnected: false));
		}
	}

	public IActivationManager ActivationManager
	{
		get
		{
			return _activationManager;
		}
		set
		{
			_activationManager = value;
		}
	}

	public Exception LastAutomaticActivationError => _activationManager.LastAutomaticActivationError as LicenseException;

	public IHardwareLicenseCountEvaluator HardwareLicenseCountEvaluator
	{
		get
		{
			if (_hardwareLicenseCountEvaluator == null)
			{
				_hardwareLicenseCountEvaluator = _componentsProvider.CreateHardwareLicenseCountEvaluator(_licenseDocument);
			}
			return _hardwareLicenseCountEvaluator;
		}
	}

	public ILicenseCalculator LicenseCalculator => _componentsProvider.CreateLicenseCalculator(_licenseDocument);

	public string SystemSLC
	{
		get
		{
			if (!string.IsNullOrEmpty(_licenseDocument.SoftwareLicenseCode))
			{
				return _licenseDocument.SoftwareLicenseCode;
			}
			_serverEnvironment.Server.Logger.LogWarning("Failed to look up system SLC. SLC was null or empty");
			return string.Empty;
		}
	}

	public ITotalLicense TotalActivatedLicenses => LicenseFactory.CreateNewTotalLicense(_licenseDocument.TotalActivatedHardwareLicenses, _licenseDocument.TotalActivatedCameraLicenses, _licenseDocument.TotalActivatedICPCameraLicenses, _licenseDocument.TotalActivatedArculesCameraLicenses);

	public string SystemInformationXml
	{
		get
		{
			string systemSLC;
			string encryptedSlc;
			lock (this)
			{
				if (!HasLicense())
				{
					throw new LicenseException("Missing License", LicenseErrorNumber.Missing);
				}
				systemSLC = SystemSLC;
				if (string.IsNullOrEmpty(systemSLC))
				{
					throw new LicenseException("SLC is invalid", LicenseErrorNumber.InvalidSLC);
				}
				encryptedSlc = EncryptedSlc;
			}
			return VideoOS.Server.Licensing.ActivationManager.FormatSystemInformationXml(systemSLC, encryptedSlc);
		}
	}

	public int RecorderLicenseCount => _licenseDocument.RecorderLicenseCount;

	public int HardwareLicenseCount => _licenseDocument.HardwareLicenseCount;

	public int ChannelLicenseCount => _licenseDocument.ChannelLicenseCount;

	public bool IsLicenseCacheRebuilding => _licenseManagerCache.IsRunning;

	public event EventHandler AutomaticActivationFailed;

	public event EventHandler RecordersNeedLicenseUpdate;

	public LicenseManager(ILicenseDataSetComponent licenseDataSetComponent, ILicenseManagerComponentsProvider componentProvider, IServerEnvironment serverEnvironment, ICachedLicenseInfo cachedLicenseInfo, ILicenseManagerCache licenseManagerCache)
		: this(licenseDataSetComponent, new DataComponentFactory(), new TemporaryLicenseStorage(), componentProvider, OemResources.Instance, ServerConfig.Instance, null, new TimerWrapper(GetLicenseRegenerationPeriod()), serverEnvironment, cachedLicenseInfo, licenseManagerCache)
	{
	}

	public LicenseManager(ILicenseDataSetComponent licenseDataSetComponent, IDataComponentFactory dataComponentFactory, ITemporaryLicenseStorage temporaryLicenseStorage, ILicenseManagerComponentsProvider componentsProvider, IOemResources oemResources, IServerConfig serverConfig, ILicenseDocument licenseDocument, ITimer licenseTimer, IServerEnvironment serverEnvironment, ICachedLicenseInfo cachedLicenseInfo, ILicenseManagerCache licenseManagerCache)
	{
		_licenseDocument = licenseDocument;
		if (licenseDocument != null)
		{
			LicenseFactory.IsChannelBasedLicensing = _licenseDocument.IsFeatureLicensed("ChannelLicensing");
		}
		if (licenseDataSetComponent == null)
		{
			throw new ArgumentNullException("licenseDataSetComponent");
		}
		if (dataComponentFactory == null)
		{
			throw new ArgumentNullException("dataComponentFactory");
		}
		if (temporaryLicenseStorage == null)
		{
			throw new ArgumentNullException("temporaryLicenseStorage");
		}
		if (componentsProvider == null)
		{
			throw new ArgumentNullException("componentsProvider");
		}
		if (oemResources == null)
		{
			throw new ArgumentNullException("oemResources");
		}
		if (licenseTimer == null)
		{
			throw new ArgumentNullException("licenseTimer");
		}
		if (serverEnvironment == null)
		{
			throw new ArgumentNullException("serverEnvironment");
		}
		_cachedLicenseInfo = cachedLicenseInfo ?? throw new ArgumentNullException("cachedLicenseInfo");
		_licenseManagerCache = licenseManagerCache ?? throw new ArgumentNullException("licenseManagerCache");
		_licenseDataSetComponent = licenseDataSetComponent;
		_dataComponentFactory = dataComponentFactory;
		_temporaryLicenseStorage = temporaryLicenseStorage;
		_componentsProvider = componentsProvider;
		_serverEnvironment = serverEnvironment;
		_activationManager = new ActivationManager(this, serverConfig, serverEnvironment);
		_oemResources = oemResources;
		_licenseTimer = licenseTimer;
		_licenseTimer.Elapsed += LicenseTimerOnElapsed;
	}

	public ILicenseRequest CreateLicenseRequest(bool includeHardwareIds)
	{
		if (_licenseDocument == null)
		{
			throw new NotSupportedException();
		}
		if (_licenseDocument.FeatureExists("HardwareLicensing"))
		{
			return new LicenseRequestHardware(_componentsProvider, this, _licenseDocument, _dataComponentFactory, _serverEnvironment.ServerLogManager, _licenseDataSetComponent, includeHardwareIds, _oemResources);
		}
		if (_licenseDocument.FeatureExists("ChannelLicensing"))
		{
			return new LicenseRequestChannel(_componentsProvider, this, _licenseDocument, _dataComponentFactory, _serverEnvironment.ServerLogManager, _licenseDataSetComponent, includeHardwareIds, _oemResources);
		}
		throw new LicenseException("Unsupported licensing mode.", LicenseErrorNumber.UnsupportedLicensingMode);
	}

	public bool IsFeatureLicensed(string featureName)
	{
		if (_licenseDocument != null)
		{
			return _licenseDocument.IsFeatureLicensed(featureName);
		}
		return false;
	}

	public IEnumerable<IFeature> QueryVisibleFeatures()
	{
		return FeatureInformation.QueryAllLicensedFeatures(_licenseDocument);
	}

	public bool TryGetFeatureExpiryDate(string featureName, out DateTime expiryDate)
	{
		expiryDate = DateTime.MaxValue;
		if (_licenseDocument == null)
		{
			return false;
		}
		return _licenseDocument.TryGetFeatureExpiryDate(featureName, out expiryDate);
	}

	public void Initialize()
	{
		_temporaryLicenseStorage.LoadData(_dataComponentFactory);
		_activationManager.AutomaticActivationFailedEventHandler += RaiseOnAutomaticActivationFailed;
		_licenseTimer.StartTimer();
	}

	public bool IsHardwareInCurrentYearChangesWithoutActivation(string mac)
	{
		return _temporaryLicenseStorage.FindLicense(mac)?.HasCurrentYearToken(_licenseDocument.ActivatedDate) ?? false;
	}

	public bool IsHardwareInChangesWithoutActivation(string mac)
	{
		return _temporaryLicenseStorage.FindLicense(mac)?.IsChangedWithoutActivation() ?? false;
	}

	public bool IsHardwareActivated(IHardwareInfo hardwareInfo)
	{
		if (hardwareInfo == null)
		{
			return false;
		}
		return _licenseDocument.IsMacAddressLicensedHash(hardwareInfo.MacAddressHashed);
	}

	public bool HasLicense()
	{
		return _licenseDocument != null;
	}

	public void UpdateLicense(ILicenseDocument newLicense, LicenseUpdateOption updateOption)
	{
		LicensingEventTracer.Logger.TraceMethodStart("UpdateLicense");
		if (newLicense == null)
		{
			throw new ArgumentNullException("newLicense");
		}
		_licenseAllocator = null;
		if (newLicense.IsFeatureLicensed("HardwareLicensing"))
		{
			CountManager = new LicenseCountManagerHardware();
			LicenseFactory.IsChannelBasedLicensing = false;
		}
		else
		{
			if (!newLicense.IsFeatureLicensed("ChannelLicensing"))
			{
				throw new LicenseException("Unsupported licensing mode.", LicenseErrorNumber.UnsupportedLicensingMode);
			}
			CountManager = new LicenseCountManagerChannel();
			LicenseFactory.IsChannelBasedLicensing = true;
		}
		lock (_obtainLicenseLock)
		{
			if (updateOption == LicenseUpdateOption.Update)
			{
				newLicense.VerifyLicense();
				_temporaryLicenseStorage.ClearChangesWithoutActivationTokens();
			}
			_cachedLicenseInfo.ClearCachedLicenseInfo();
			_licenseDocument = newLicense;
			LoadLicenseSnapshot();
		}
		if (updateOption == LicenseUpdateOption.Initialize)
		{
			newLicense.VerifyLicense();
		}
		_activationManager.SystemActivation = newLicense.IsFeatureLicensed("SystemActivated");
		if (_activationManager.SystemActivation)
		{
			_activationManager.QueueAutomaticActivation();
		}
		LicensingEventTracer.Logger.TraceMethodStop("UpdateLicense");
	}

	public void UpdateLicenseSnapshot(ILicenseSnapshot licenseSnapshot)
	{
		if (licenseSnapshot == null)
		{
			throw new ArgumentNullException("licenseSnapshot");
		}
		lock (_obtainLicenseLock)
		{
			LicensingEventTracer.Logger.TraceMethodStart("UpdateLicenseSnapshot");
			try
			{
				CountManager.ClearChangesWithoutActivation(_temporaryLicenseStorage, LicenseActivatedDate);
				_cachedLicenseInfo.ClearCachedLicenseInfo();
				foreach (IRecorderInfo recorder in licenseSnapshot.Recorders)
				{
					GetRecorderLicense(recorder.Id, licenseSnapshot, acquireLicenseForCurrentlyUnlicensedItems: false);
				}
				foreach (IRecorderInfo recorder2 in licenseSnapshot.Recorders)
				{
					byte[] recorderLicense = GetRecorderLicense(recorder2.Id, licenseSnapshot, acquireLicenseForCurrentlyUnlicensedItems: true);
					_licenseManagerCache.AddRecordingServerLicense(recorder2.Id, recorderLicense);
				}
			}
			finally
			{
				UpdateLicenseData(licenseSnapshot);
				LicensingEventTracer.Logger.TraceMethodStop("UpdateLicenseSnapshot");
			}
		}
	}

	public ILicenseSnapshot LoadLicenseSnapshot()
	{
		bool isObtainedFromCache;
		ILicenseSnapshot licenseSnapshot = _licenseDataSetComponent.LoadLicenseDataSet(out isObtainedFromCache);
		if (!isObtainedFromCache)
		{
			UpdateLicenseSnapshot(licenseSnapshot);
		}
		return licenseSnapshot;
	}

	public void VerifyLicense()
	{
		if (_licenseDocument == null)
		{
			throw new LicenseException("License is missing.", LicenseErrorNumber.Missing);
		}
		_licenseDocument.VerifyLicense();
		_temporaryLicenseStorage.Verify();
	}

	public int GetICPFeedLicenses(string feedType)
	{
		return _licenseDocument.GetICPFeedLicenses(feedType);
	}

	public int GetFeedLicenses(string feedType)
	{
		return _licenseDocument.GetFeedLicenses(feedType);
	}

	public void ActivateLicense(string username, string password)
	{
		_activationManager.ActivateLicense(username, password);
	}

	public bool IsActivationRequired()
	{
		bool includeHardwareIds = true;
		ILicenseRequest licenseRequest = CreateLicenseRequest(includeHardwareIds);
		return licenseRequest.IsActivationRequired();
	}

	public void QueueAutomaticActivation()
	{
		_activationManager.QueueAutomaticActivation();
	}

	public bool QueryAutomaticLicenseActivationEnabled()
	{
		return _activationManager.QueryAutomaticLicenseActivationEnabled();
	}

	public bool NearExpiryAutomaticActivationNeeded(DateTime utcNow, DateTime expiryDate)
	{
		if ((expiryDate - utcNow).Days < 14)
		{
			return QueryAutomaticLicenseActivationEnabled();
		}
		return false;
	}

	public byte[] CreateServerLicenseRequest(bool includeHardwareIds)
	{
		ILicenseRequest licenseRequest = CreateLicenseRequest(includeHardwareIds);
		string lrqPublicKey = "<RSAKeyValue><Modulus>zmwboAV9rGYf4pa2NW/Tfydh62vDm2YQcekVHPfph8n/Range44GtzFOr7gsSiSJ65ro1IwPJLisroXcF/Eg9RRxsPmh8v9gn6+OqEvDhITp07shmv5N2B1Do921ZnknjDBjxo7Fnki+upIJhScsbNyylF0kUkDMljXOIjis3A8=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
		CngKey cngKey = CngKey.Import(Convert.FromBase64String(Keys.SiteSpecificPrivatePublicKey), CngKeyBlobFormat.GenericPrivateBlob);
		string sitePublicKey = Convert.ToBase64String(cngKey.Export(CngKeyBlobFormat.GenericPublicBlob));
		LicenseFileHelper licenseFileHelper = new LicenseFileHelper();
		LicenseRequestContainer licenseRequestContainer = new LicenseRequestContainer(licenseRequest.GenerateLicenseRequestAsXml());
		licenseRequestContainer.IntegrationData = Convert.ToBase64String(Encoding.UTF8.GetBytes(licenseRequest.GenerateSerializedIntegrationRegistrations()));
		return licenseFileHelper.CreateLicenseRequestFile(JsonConvert.SerializeObject(licenseRequestContainer), lrqPublicKey, sitePublicKey);
	}

	public byte[] GetRecorderLicense(Guid recorderId)
	{
		if (_licenseManagerCache.RetrieveRecordingServerLicense(recorderId, out var license))
		{
			return license;
		}
		RecordersRequiringUpdate = LicenseFactory.CreateRecordersRequiringLicensingDictionary();
		lock (_obtainLicenseLock)
		{
			ILicenseSnapshot licenseSnapshot = LoadLicenseSnapshot();
			if (_licenseManagerCache.RetrieveRecordingServerLicense(recorderId, out license))
			{
				return license;
			}
			try
			{
				license = GetRecorderLicense(recorderId, licenseSnapshot, acquireLicenseForCurrentlyUnlicensedItems: true);
				CountManager.ValidateRelatedRecordingServerLicenses(this, licenseSnapshot, recorderId);
			}
			finally
			{
				_cachedLicenseInfo.ClearCachedLicenseInfo();
				UpdateLicenseData(licenseSnapshot);
			}
		}
		if (RecordersRequiringUpdate.Count != 0)
		{
			RaiseOnRecordersNeedLicenseUpdate();
		}
		RecordersRequiringUpdate = null;
		return license;
	}

	public byte[] GetRecorderLicense(Guid recorderId, ILicenseSnapshot licenseSnapshot, bool acquireLicenseForCurrentlyUnlicensedItems)
	{
		if (licenseSnapshot == null)
		{
			throw new ArgumentNullException("licenseSnapshot");
		}
		lock (_obtainLicenseLock)
		{
			try
			{
				RecorderRequestInfo recorderRequestInfo = null;
				IRecorderInfo recorder = licenseSnapshot.GetRecorder(recorderId);
				if (recorder == null)
				{
					recorderRequestInfo = new RecorderRequestInfo(recorderId);
				}
				else
				{
					recorder.ClearExistingLicenseInformation();
					recorderRequestInfo = CreateRecorderRequestInfo(recorder);
				}
				RecorderLicense recorderLicense_ProcessRequest = GetRecorderLicense_ProcessRequest(recorderRequestInfo, acquireLicenseForCurrentlyUnlicensedItems, licenseSnapshot);
				return recorderLicense_ProcessRequest.GetResponse();
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				_serverEnvironment.Server.Logger.LogError("Error getting recorder license. {Exception}", ex2);
				throw new LicenseException("Error getting recorder license", LicenseErrorNumber.ErrorGettingRecorderLicense, ex2);
			}
		}
	}

	private RecorderLicense GetRecorderLicense_ProcessRequest(RecorderRequestInfo recorderRequest, bool acquireLicenseForCurrentlyUnlicensedItems, ILicenseSnapshot licenseSnapshot)
	{
		RecorderLicense recorderLicense = new RecorderLicense(recorderRequest.Id);
		foreach (string allFeatureName in _licenseDocument.AllFeatureNames)
		{
			recorderLicense.FeaturesSupported.Add(allFeatureName);
		}
		if (LicenseExpires)
		{
			recorderLicense.ExpiryDate = LicenseExpiryDate;
		}
		if (!GetRecorderLicense_HandleZeroCountLicenseRequest(recorderRequest.Id, licenseSnapshot))
		{
			licenseSnapshot.RemoveRecorderLicense(recorderRequest.Id);
			return recorderLicense;
		}
		try
		{
			CachedVerifyLicense();
		}
		catch (LicenseException)
		{
			return recorderLicense;
		}
		bool flag = licenseSnapshot.GetRecorderLicense(recorderRequest.Id) == null;
		CountManager.ClearByRecorder(recorderRequest.Id);
		if (licenseSnapshot.RecorderLicenseCount() >= _licenseDocument.RecorderLicenseCount || (flag && !acquireLicenseForCurrentlyUnlicensedItems))
		{
			licenseSnapshot.RemoveRecorderLicense(recorderRequest.Id);
			if (licenseSnapshot.RecorderLicenseCount() >= _licenseDocument.RecorderLicenseCount)
			{
				return recorderLicense;
			}
		}
		return ProcessLicenses(licenseSnapshot, recorderRequest, recorderLicense, acquireLicenseForCurrentlyUnlicensedItems);
	}

	private RecorderLicense ProcessLicenses(ILicenseSnapshot licenseSnapshot, RecorderRequestInfo recorderRequestInfo, RecorderLicense recorderLicense, bool acquireLicenseForCurrentlyUnlicensedItems)
	{
		if (licenseSnapshot.GetRecorderLicense(recorderRequestInfo.Id) == null)
		{
			licenseSnapshot.AddRecorderLicense(LicenseFactory.CreateRecorderLicenseInfo(recorderRequestInfo.Id));
		}
		recorderLicense.Status = RecorderLicense.StatusConstants.Licensed;
		int num = 0;
		if (IsFeatureLicensed("HardwareLicensing"))
		{
			int num2 = recorderRequestInfo.Hardware.Count((IHardwareInfo h) => !h.IsInterconnectedProduct && h.Enabled);
			num = num2 + CountManager.GetHardwareUsedOnSite(icp: false);
		}
		else
		{
			int num3 = (from h in recorderRequestInfo.Hardware
				from d in h.Devices
				where h.Enabled
				where !h.IsInterconnectedProduct
				where d.Enabled && RegisteredDeviceType.IsCamera(d.DeviceType)
				select d).Count();
			num = num3 + CountManager.GetCameraFeedsUsedOnSite(icp: false);
		}
		TotalAllowedChangesWithoutActivation = _licenseDocument.ReplacementLimit.CalculateTotalReplacementLimit(num);
		GetRecorderLicense_ProcessHardwareList(recorderRequestInfo, recorderLicense, licenseSnapshot, acquireNewLicenses: false, ignoreAlreadyLicensed: false);
		if (acquireLicenseForCurrentlyUnlicensedItems)
		{
			GetRecorderLicense_ProcessHardwareList(recorderRequestInfo, recorderLicense, licenseSnapshot, acquireNewLicenses: true, ignoreAlreadyLicensed: true);
		}
		return recorderLicense;
	}

	private bool GetRecorderLicense_HandleZeroCountLicenseRequest(Guid recorderId, ILicenseSnapshot licenseSnapshot)
	{
		IRecorderInfo recorder = licenseSnapshot.GetRecorder(recorderId);
		if (recorder == null || recorder.Hardware.Count <= 0)
		{
			CountManager.ClearByRecorder(recorderId);
		}
		if (recorder == null || !recorder.Enabled)
		{
			return false;
		}
		return true;
	}

	private void GetRecorderLicense_ProcessHardwareList(RecorderRequestInfo recorderRequestInfo, RecorderLicense recorderLicense, ILicenseSnapshot licenseSnapshot, bool acquireNewLicenses, bool ignoreAlreadyLicensed)
	{
		ILicenseAllocator licenseAllocator = GetLicenseAllocator();
		foreach (IHardwareInfo item in recorderRequestInfo.Hardware)
		{
			CountManager.RegisterHardwareRecorderRelation(recorderLicense.RecorderId, item.MacAddress);
			licenseAllocator.AllocateLicenses(item, recorderLicense, licenseSnapshot, acquireNewLicenses, ignoreAlreadyLicensed);
		}
	}

	private ILicenseAllocator GetLicenseAllocator()
	{
		lock (_retrieveLicenseAllocatorLock)
		{
			if (_licenseAllocator == null)
			{
				_hardwareLicenseCountEvaluator = null;
				ILicenseAllocator licenseAllocator = null;
				if (IsFeatureLicensed("HardwareLicensing"))
				{
					licenseAllocator = new LicenseAllocatorHardware(CountManager, _licenseDocument, _temporaryLicenseStorage, _serverEnvironment, TotalAllowedChangesWithoutActivation);
				}
				else
				{
					if (!IsFeatureLicensed("ChannelLicensing"))
					{
						throw new LicenseException("Unsupported licensing mode.", LicenseErrorNumber.UnsupportedLicensingMode);
					}
					licenseAllocator = new LicenseAllocatorChannel(CountManager, _licenseDocument, _temporaryLicenseStorage, HardwareLicenseCountEvaluator, TotalAllowedChangesWithoutActivation);
				}
				_licenseAllocator = licenseAllocator;
			}
			else
			{
				_licenseAllocator.ResetChangesWithoutActivation(TotalAllowedChangesWithoutActivation);
			}
		}
		return _licenseAllocator;
	}

	private void CachedVerifyLicense()
	{
		if (DateTime.UtcNow >= _cachedVerifyTime.AddMinutes(1.0))
		{
			VerifyLicense();
			_cachedVerifyTime = DateTime.UtcNow;
		}
	}

	private RecorderRequestInfo CreateRecorderRequestInfo(IRecorderInfo recorderInfo)
	{
		RecorderRequestInfo recorderRequestInfo = new RecorderRequestInfo(recorderInfo.Id);
		recorderRequestInfo.Enabled = recorderInfo?.Enabled ?? false;
		if (!recorderRequestInfo.Enabled)
		{
			return recorderRequestInfo;
		}
		bool flag = IsFeatureLicensed("InterconnectedProducts");
		foreach (IHardwareInfo item in recorderInfo.Hardware)
		{
			if (!item.IsInterconnectedProduct || flag)
			{
				recorderRequestInfo.Hardware.Add(item);
				_hardwareLicenseCountEvaluator?.UpdateLicenseCount(item);
				recorderRequestInfo.Devices.AddRange(item.Devices);
			}
		}
		return recorderRequestInfo;
	}

	public string GetFeatureParameterValue(string featureName, string featureParameterName)
	{
		return _licenseDocument.GetFeatureParameterValue(featureName, featureParameterName);
	}

	private void LicenseTimerOnElapsed(object sender, EventArgs eventArgs)
	{
		RecordersRequiringUpdate = LicenseFactory.CreateRecordersRequiringLicensingDictionary();
		lock (_obtainLicenseLock)
		{
			_licenseDataSetComponent.LicenseCacheLock.EnterWriteLock();
			try
			{
				Collection<string> macAddresses = _temporaryLicenseStorage.FindHardwareWithPreviousYearTokens(_licenseDocument.ActivatedDate);
				CountManager.RemoveHardwareFromChangesWithoutActivation(macAddresses);
				if (IsFeatureLicensed("ChannelLicensing"))
				{
					Collection<IHardwareDeviceItem> hardwareDeviceItems = _temporaryLicenseStorage.FindHardwareDevicesWithPreviousYearTokens(_licenseDocument.ActivatedDate);
					CountManager.RemoveDevicesFromChangesWithoutActivation(hardwareDeviceItems);
				}
				_licenseManagerCache.ClearAllCaches();
				ILicenseSnapshot licenseSnapshot = LoadLicenseSnapshot();
				foreach (IRecorderInfo recorder in licenseSnapshot.Recorders)
				{
					byte[] recorderLicense = GetRecorderLicense(recorder.Id, licenseSnapshot, acquireLicenseForCurrentlyUnlicensedItems: true);
					RecordersRequiringUpdate.Add(recorder.Id, recorderLicense);
				}
				UpdateLicenseData(licenseSnapshot);
			}
			finally
			{
				_licenseDataSetComponent.LicenseCacheLock.ExitWriteLock();
			}
		}
		RaiseOnRecordersNeedLicenseUpdate();
		RecordersRequiringUpdate = null;
	}

	private void UpdateLicenseData(ILicenseSnapshot licenseSnapshot)
	{
		LicensingEventTracer.Logger.TraceMethodStart("UpdateLicenseData");
		if (licenseSnapshot.LicenseUpdated)
		{
			SaveLicenseChangesWithExceptionLogging(licenseSnapshot);
		}
		LicensingEventTracer.Logger.TraceMethodStop("UpdateLicenseData");
	}

	private void SaveLicenseChangesWithExceptionLogging(ILicenseSnapshot licenseSnapshot)
	{
		try
		{
			_temporaryLicenseStorage.PersistData(_dataComponentFactory);
			licenseSnapshot.SaveChanges();
		}
		catch (Exception ex)
		{
			_serverEnvironment.Server.Logger.LogError("{Exception}", ex);
		}
	}

	private static double GetLicenseRegenerationPeriod()
	{
		double result = TimeSpan.FromDays(1.0).TotalMilliseconds;
		string localMachineRegistryPath = OemResources.Instance.Server.LocalMachineRegistryPath;
		try
		{
			string text = (string)Registry.GetValue(localMachineRegistryPath, "RegenerationPeriodMs", string.Empty);
			if (!string.IsNullOrEmpty(text))
			{
				result = double.Parse(text, CultureInfo.InvariantCulture);
			}
		}
		catch (SecurityException)
		{
		}
		catch (IOException)
		{
		}
		catch (ArgumentException)
		{
		}
		return result;
	}

	private void RaiseOnAutomaticActivationFailed(object sender, EventArgs eventArgs)
	{
		this.AutomaticActivationFailed?.Invoke(sender, eventArgs);
	}

	private void RaiseOnRecordersNeedLicenseUpdate()
	{
		this.RecordersNeedLicenseUpdate?.Invoke(this, EventArgs.Empty);
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	private void Dispose(bool disposing)
	{
		if (disposing)
		{
			if (_activationManager != null)
			{
				_activationManager.AutomaticActivationFailedEventHandler -= RaiseOnAutomaticActivationFailed;
				_activationManager.Dispose();
				_activationManager = null;
			}
			if (_licenseTimer != null)
			{
				_licenseTimer.StopTimer();
				_licenseTimer.Dispose();
				_licenseTimer = null;
			}
		}
	}
}
