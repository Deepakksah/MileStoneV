using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration.Configuration;
using VideoOS.Common.Integration.LicenseInfo;
using VideoOS.Common.Interfaces.Licensing;
using VideoOS.Licensing;
using VideoOS.Server.Properties;
using VideoOS.Utilities.Telemetry;

namespace VideoOS.Server.Licensing;

public sealed class LicenseInfoXmlWriter
{
	private ILicenseManager _licenseManager;

	private LicenseInformation _licenseInfo;

	private readonly ILogger _licenseLogger;

	public LicenseInfoXmlWriter(ILicenseManager licenseManager)
	{
		_licenseManager = licenseManager ?? throw new ArgumentNullException("licenseManager");
		_licenseInfo = new LicenseInformation();
		_licenseLogger = Telemetry.GetILogger("Licensing");
	}

	public LicenseInformation CreateLicenseInformationCache(ILicenseDataSetComponent data)
	{
		ILicenseSnapshot licenseSnapshot = _licenseManager.LoadLicenseSnapshot();
		if (licenseSnapshot == null && data != null)
		{
			licenseSnapshot = data.LoadLicenseDataSet(out var isObtainedFromCache);
			if (!isObtainedFromCache)
			{
				_licenseManager.UpdateLicenseSnapshot(licenseSnapshot);
			}
		}
		UpdateLicenseInformation(null, licenseSnapshot);
		return _licenseInfo;
	}

	public string CreateLicenseInformationXml(LicenseInformation cachedLicenseInfo, ILicenseDataSetComponent data)
	{
		_licenseLogger.LogDebug("Start CreateLicenseInformationXml");
		ILicenseSnapshot licenseSnapshot = _licenseManager.LoadLicenseSnapshot();
		if (licenseSnapshot == null)
		{
			bool isObtainedFromCache = false;
			licenseSnapshot = data?.LoadLicenseDataSet(out isObtainedFromCache);
			if (cachedLicenseInfo == null && !isObtainedFromCache)
			{
				_licenseManager.UpdateLicenseSnapshot(licenseSnapshot);
			}
		}
		UpdateLicenseInformation(cachedLicenseInfo, licenseSnapshot);
		_licenseLogger.LogDebug("End CreateLicenseInformationXml");
		return _licenseInfo.ToXml();
	}

	private void UpdateLicenseInformation(LicenseInformation cachedLicenseInfo, ILicenseSnapshot licenseSnapshot)
	{
		lock (_licenseManager)
		{
			if (cachedLicenseInfo != null)
			{
				_licenseInfo = cachedLicenseInfo;
				return;
			}
			if (!_licenseManager.HasLicense())
			{
				_licenseInfo.LicenseState = LicenseState.Missing;
				return;
			}
			_licenseInfo.ProductName = _licenseManager.ProductName;
			if (!UpdateSystemSLC())
			{
				return;
			}
			_licenseInfo.EncryptedSlc = _licenseManager.EncryptedSlc;
			if (_licenseManager.LicenseExpires)
			{
				_licenseInfo.LicenseExpirationDate = _licenseManager.LicenseExpiryDate;
			}
			else
			{
				_licenseInfo.LicenseExpirationDate = DateTime.MaxValue;
			}
			_licenseInfo.LicenseActivationDate = _licenseManager.LicenseActivatedDate;
			_licenseInfo.Owner = LicenseOwnerInformation.FromXml(_licenseManager.LicenseOwnerXml);
			_licenseInfo.SitesLicenseXml = SitesLicenseXml.FromXml(_licenseManager.SitesInformationXml);
			UpdateFeatureInformation();
			if (!UpdateLicenseState())
			{
				return;
			}
			_licenseInfo.LicenseState = LicenseState.Valid;
			ITotalLicense totalActivatedLicenses = _licenseManager.TotalActivatedLicenses;
			_licenseInfo.TotalActivated.HardwareCount = totalActivatedLicenses.HardwareCount;
			_licenseInfo.TotalActivated.CameraCount = totalActivatedLicenses.CameraCount;
			_licenseInfo.TotalActivated.ICPCameraCount = totalActivatedLicenses.ICPCameraCount;
			_licenseInfo.TotalActivated.ArculesCameraCount = totalActivatedLicenses.ArculesCameraCount;
			_licenseInfo.HardwareLicenses.TotalReplacementLimit = _licenseManager.TotalReplacementLimit;
			UpdateAvailableLicenseInformation();
			UpdateUsedLicensesInformation(licenseSnapshot);
			if (!_licenseLogger.IsEnabled(LogLevel.Debug))
			{
				return;
			}
			_licenseLogger.LogDebug("License Information");
			_licenseLogger.LogDebug("Available: {Available}", _licenseInfo.HardwareLicenses.Available);
			_licenseLogger.LogDebug("Used: {Used}", _licenseInfo.HardwareLicenses.Used);
			_licenseLogger.LogDebug("Temporary: {Temporary}", _licenseInfo.HardwareLicenses.Temporary);
			_licenseLogger.LogDebug("Expired: {Expired}", _licenseInfo.HardwareLicenses.Expired);
			_licenseLogger.LogDebug("Replaced: {Replaced}", _licenseInfo.HardwareLicenses.Replaced);
			_licenseLogger.LogDebug("Missing: {Missing}", _licenseInfo.HardwareLicenses.Missing);
			_licenseLogger.LogDebug("Total Replacement Limit: {TotalReplacementLimit}", _licenseInfo.HardwareLicenses.TotalReplacementLimit);
			if (licenseSnapshot == null)
			{
				return;
			}
			foreach (IRecorderInfo recorder in licenseSnapshot.Recorders)
			{
				foreach (IHardwareInfo item in recorder.Hardware)
				{
					_licenseLogger.LogDebug("Hardware: {HardwareName} - {MacAddress}", item.Name, item.MacAddress);
					_licenseLogger.LogDebug("Hardware Id: {HardwareId}", item.Id);
					_licenseLogger.LogDebug("License State: {LicenseState}", item.LicenseState);
					_licenseLogger.LogDebug("ICP: {IsInterconnectedProduct}", item.IsInterconnectedProduct);
					_licenseLogger.LogDebug("Has non camera devices enabled: {EnabledNonCameraDevices}", item.EnabledNonCameraDevices.Count > 0);
					foreach (IDeviceInfo item2 in item.CameraDevices.OrderBy((IDeviceInfo d) => d.DeviceType))
					{
						_licenseLogger.LogDebug("Device DeviceType: {DeviceType}", item2.DeviceType);
						_licenseLogger.LogDebug("Device DeviceIndex: {DeviceIndex}", item2.DeviceIndex);
						_licenseLogger.LogDebug("Device Enabled: {DeviceEnabled}", item2.Enabled);
						_licenseLogger.LogDebug("Device LicenseState: {LicenseState}", item2.LicenseState);
						_licenseLogger.LogDebug("Device DeviceId: {DeviceId}", item2.Id);
						_licenseLogger.LogDebug("Device OriginalDeviceId: {OriginalDeviceId}", item2.OriginalDeviceId);
					}
				}
			}
		}
	}

	private bool UpdateLicenseState()
	{
		try
		{
			if (_licenseManager.IsLicenseExpired)
			{
				_licenseInfo.LicenseState = LicenseState.Expired;
				return false;
			}
			_licenseManager.VerifyLicense();
		}
		catch (LicenseException ex)
		{
			_licenseInfo.LicenseState = LicenseState.Invalid;
			_licenseInfo.LicenseErrorMessage = ex.ErrorNumber.ToString(CultureInfo.InvariantCulture);
			return false;
		}
		return true;
	}

	private bool UpdateSystemSLC()
	{
		string systemSLC = _licenseManager.SystemSLC;
		if (!string.IsNullOrEmpty(systemSLC))
		{
			_licenseInfo.Slc = systemSLC;
			return true;
		}
		_licenseInfo.LicenseState = LicenseState.Invalid;
		_licenseInfo.LicenseErrorMessage = Resources.SLCInvalidInLicense;
		return false;
	}

	private void UpdateFeatureInformation()
	{
		IEnumerable<IFeature> enumerable = _licenseManager.QueryVisibleFeatures();
		foreach (IFeature item2 in enumerable)
		{
			bool licensed = _licenseManager.IsFeatureLicensed(item2.Name);
			if (!_licenseManager.TryGetFeatureExpiryDate(item2.Name, out var expiryDate))
			{
				expiryDate = DateTime.MaxValue;
			}
			FeatureInformation featureInformation = new FeatureInformation(item2.Name, item2.Name, licensed, expiryDate, item2.Visible);
			if (item2.Parameters.Count > 0)
			{
				foreach (KeyValuePair<string, string> parameter in item2.Parameters)
				{
					if (!string.Equals(parameter.Key, "Expires", StringComparison.Ordinal))
					{
						FeatureParameterInformation item = new FeatureParameterInformation(parameter.Key, parameter.Key, parameter.Value);
						featureInformation.Parameters.Add(item);
					}
				}
			}
			_licenseInfo.Features.Add(featureInformation);
		}
	}

	private void UpdateAvailableLicenseInformation()
	{
		_licenseInfo.RecorderLicenses.Available = _licenseManager.RecorderLicenseCount;
		if (_licenseManager.IsFeatureLicensed("ChannelLicensing"))
		{
			_licenseInfo.HardwareLicenses.Available = _licenseManager.ChannelLicenseCount;
		}
		else
		{
			_licenseInfo.HardwareLicenses.Available = _licenseManager.HardwareLicenseCount;
		}
		string[] allKnownTypes = RegisteredDeviceType.BuiltInTypes.GetAllKnownTypes();
		string[] array = allKnownTypes;
		foreach (string text in array)
		{
			FeedLicenseInformation feedLicenseInformation = new FeedLicenseInformation(text);
			feedLicenseInformation.Available = _licenseManager.GetFeedLicenses(text);
			_licenseInfo.FeedLicenses.Add(feedLicenseInformation);
			FeedLicenseInformation feedLicenseInformation2 = new FeedLicenseInformation(text);
			feedLicenseInformation2.Available = _licenseManager.GetICPFeedLicenses(text);
			_licenseInfo.ICPFeedLicenses.Add(feedLicenseInformation2);
		}
	}

	private void UpdateUsedLicensesInformation(ILicenseSnapshot licenseSnapshot)
	{
		int num = 0;
		ILicenseCalculator licenseCalculator = _licenseManager.LicenseCalculator;
		List<IHardwareInfo> list = new List<IHardwareInfo>();
		foreach (IRecorderInfo recorder in licenseSnapshot.Recorders)
		{
			UpdateRecorderLicenseInformation(recorder, licenseSnapshot, list);
		}
		if (licenseCalculator != null && _licenseInfo.LicenseType == LicenseType.Channel)
		{
			_licenseInfo.LicenseReport.MissingLicenses = licenseCalculator.CalculateLicenseRequirements(list, out var licenseCalculations);
			_licenseInfo.LicenseReport.LicenseCalculationResults.AddRange(licenseCalculations);
			num = licenseCalculations.Sum((ILicenseCalculationResult h) => h.ChannelLicenses);
			_licenseInfo.LicenseReport.MultiLicenseHardware = licenseCalculations.Where((ILicenseCalculationResult h) => h.ChannelLicenses > 1).Any();
		}
		_licenseInfo.LicenseReport.MissingLicenses = num > _licenseInfo.HardwareLicenses.Available;
		_licenseInfo.HardwareLicenses.Replaced = _licenseManager.CountManager.UsedChangesWithoutActivation;
		_licenseLogger.LogInformation("UsedChangesWithoutActivation : {UsedChangesWithoutActivation}", _licenseManager.CountManager.UsedChangesWithoutActivation);
		_licenseLogger.LogInformation("TotalReplacementLimit        : {TotalReplacementLimit}", _licenseInfo.HardwareLicenses.TotalReplacementLimit);
		if (_licenseInfo.HardwareLicenses.Replaced > _licenseInfo.HardwareLicenses.TotalReplacementLimit)
		{
			_licenseInfo.HardwareLicenses.Replaced = _licenseInfo.HardwareLicenses.TotalReplacementLimit;
		}
	}

	private void UpdateRecorderLicenseInformation(IRecorderInfo recorderInfo, ILicenseSnapshot licenseSnapshot, List<IHardwareInfo> activeHardware)
	{
		if (recorderInfo != null && recorderInfo.Enabled)
		{
			IHardwareLicenseCountEvaluator hardwareLicenseCountEvaluator = _licenseManager.HardwareLicenseCountEvaluator;
			IRecorderLicenseInfo recorderLicense = licenseSnapshot.GetRecorderLicense(recorderInfo.Id);
			if (recorderLicense != null)
			{
				_licenseInfo.RecorderLicenses.Used++;
			}
			else
			{
				_licenseInfo.RecorderLicenses.Missing++;
			}
			UpdateRecorderLicenseInformation(recorderInfo, hardwareLicenseCountEvaluator, licenseSnapshot, activeHardware);
		}
	}

	private void UpdateRecorderLicenseInformation(IRecorderInfo recorderInfo, IHardwareLicenseCountEvaluator hardwareLicenseCountEvaluator, ILicenseSnapshot licenseSnapshot, List<IHardwareInfo> activeHardware)
	{
		if (recorderInfo == null)
		{
			return;
		}
		foreach (IHardwareInfo item in recorderInfo.Hardware)
		{
			if (!item.Enabled)
			{
				continue;
			}
			activeHardware.Add(item);
			ILicenseInfoUpdater licenseInfoUpdater = null;
			if (_licenseInfo.LicenseType == LicenseType.Hardware)
			{
				licenseInfoUpdater = new HardwareLicenseInfoUpdater(_licenseManager, _licenseInfo);
			}
			else
			{
				if (_licenseInfo.LicenseType != LicenseType.Channel)
				{
					throw new LicenseException("Unsupported licensing mode.", LicenseErrorNumber.UnsupportedLicensingMode);
				}
				foreach (IDeviceInfo enabledDevice in item.EnabledDevices)
				{
					UpdateDeviceInfoLicenseState(licenseSnapshot, item, enabledDevice);
				}
				licenseInfoUpdater = new ChannelLicenseInfoUpdater(_licenseManager, _licenseInfo);
			}
			licenseInfoUpdater?.UpdateLicenseInformation(recorderInfo.Id, hardwareLicenseCountEvaluator, licenseSnapshot, item);
		}
	}

	private void UpdateDeviceInfoLicenseState(ILicenseSnapshot licenseSnapshot, IHardwareInfo hardwareInfo, IDeviceInfo deviceInfo)
	{
		if (deviceInfo.LicenseState != DeviceInfoLicenseState.Unknown)
		{
			return;
		}
		DateTime deviceLicenseExpireDate = licenseSnapshot.GetDeviceLicenseExpireDate(deviceInfo);
		if (_licenseManager.CountManager.DeviceStatus.ContainsKey(deviceInfo.Id))
		{
			deviceInfo.LicenseState = _licenseManager.CountManager.DeviceStatus[deviceInfo.Id];
			if (deviceInfo.LicenseState == DeviceInfoLicenseState.Grace && deviceLicenseExpireDate.Date < DateTime.UtcNow.Date)
			{
				deviceInfo.LicenseState = DeviceInfoLicenseState.Expired;
			}
		}
		else if (_licenseManager.CountManager.IsDeviceInChangesWithoutActivation(hardwareInfo, deviceInfo))
		{
			deviceInfo.LicenseState = DeviceInfoLicenseState.ChangeWithoutActivation;
		}
		else if (deviceLicenseExpireDate.Ticks == 0L)
		{
			deviceInfo.LicenseState = DeviceInfoLicenseState.Unlicensed;
		}
		else if (deviceLicenseExpireDate.Ticks == DateTime.MaxValue.Ticks)
		{
			deviceInfo.LicenseState = DeviceInfoLicenseState.Permanent;
		}
		else if (deviceLicenseExpireDate.Date < DateTime.UtcNow.Date)
		{
			deviceInfo.LicenseState = DeviceInfoLicenseState.Expired;
		}
		else if (deviceLicenseExpireDate.Date >= DateTime.UtcNow.Date)
		{
			deviceInfo.LicenseState = DeviceInfoLicenseState.Grace;
		}
	}
}
