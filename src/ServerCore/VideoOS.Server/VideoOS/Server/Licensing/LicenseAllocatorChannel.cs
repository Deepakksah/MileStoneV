using System;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Configuration;
using VideoOS.Common.Integration.LicenseInfo;
using VideoOS.Common.Interfaces.Licensing;
using VideoOS.Utilities.Telemetry;

namespace VideoOS.Server.Licensing;

public sealed class LicenseAllocatorChannel : ILicenseAllocator
{
	private enum ChangeWithoutActivationType
	{
		NotSet,
		CurrentYear,
		PreviousYear
	}

	private const int CameraFeedLicenseCount = 1;

	private const int AutoRenewalPeriod = 100;

	private readonly ILicenseCountManager _licenseCountManager;

	private readonly ILicenseDocument _licenseDocument;

	private readonly ITemporaryLicenseStorage _temporaryLicenseStorage;

	private readonly IHardwareLicenseCountEvaluator _hardwareLicenseCountEvaluator;

	private readonly int _cameraIcpFeeds;

	private readonly int _channelLicenseCount;

	private int _totalAllowedChangesWithoutActivation;

	private readonly ICustomLicenseType _customLicenseType;

	private ILogger _licenseLogger;

	public LicenseAllocatorChannel(ILicenseCountManager licenseCountManager, ILicenseDocument licenseDocument, ITemporaryLicenseStorage temporaryLicenseStorage, IHardwareLicenseCountEvaluator hardwareLicenseCountEvaluator, int allowedChangesWithoutActivation)
	{
		if (allowedChangesWithoutActivation < 0)
		{
			throw new ArgumentOutOfRangeException("allowedChangesWithoutActivation");
		}
		_licenseCountManager = licenseCountManager ?? throw new ArgumentNullException("licenseCountManager");
		_licenseDocument = licenseDocument ?? throw new ArgumentNullException("licenseDocument");
		_temporaryLicenseStorage = temporaryLicenseStorage ?? throw new ArgumentNullException("temporaryLicenseStorage");
		_hardwareLicenseCountEvaluator = hardwareLicenseCountEvaluator ?? throw new ArgumentNullException("hardwareLicenseCountEvaluator");
		_totalAllowedChangesWithoutActivation = allowedChangesWithoutActivation;
		_customLicenseType = new CustomLicenseType(licenseDocument);
		_cameraIcpFeeds = _licenseDocument.GetICPFeedLicenses("Camera");
		_channelLicenseCount = _licenseDocument.ChannelLicenseCount;
		_licenseLogger = Telemetry.GetILogger("Licensing");
		_licenseLogger.LogInformation("License Allocation Created: AllowedChangesWithoutActivation: {AllowedChangesWithoutActivation}", _totalAllowedChangesWithoutActivation);
	}

	public void ResetChangesWithoutActivation(int changesWithoutActivation)
	{
		_totalAllowedChangesWithoutActivation = changesWithoutActivation;
	}

	public void AllocateLicenses(IHardwareInfo hardwareInfo, RecorderLicense recorderLicense, ILicenseSnapshot licenseSnapshot, bool acquireNewLicenses, bool ignoreAlreadyLicensed)
	{
		if (_customLicenseType.CustomLicenseIsUsed(hardwareInfo) != CustomLicensingType.Unknown)
		{
			hardwareInfo.LicenseState = HardwareLicenseState.Free;
			if (!DriverNumbering.IsArculesMipDriver(hardwareInfo.DriverNumber) || !_licenseDocument.TryGetFeatureParameterValue("CustomLicenseType", hardwareInfo.DriverName, out var parameterValue) || (CustomLicensingType)Enum.Parse(typeof(CustomLicensingType), parameterValue) != CustomLicensingType.ZeroLicenseType)
			{
				if (hardwareInfo.Enabled)
				{
					TemporaryLicense license = new TemporaryLicense(hardwareInfo.MacAddress, DateTime.UtcNow.AddDays(_licenseDocument.HardwareGracePeriod), string.Empty);
					recorderLicense.AddHardwareLicense(hardwareInfo.Id, license);
				}
				AcquireZeroLicenseForHardware(hardwareInfo, recorderLicense, licenseSnapshot);
				return;
			}
			if (!acquireNewLicenses)
			{
				AcquireLicensesForAlreadyLicensedArculesDevices(hardwareInfo, recorderLicense, licenseSnapshot);
			}
			if (acquireNewLicenses)
			{
				AcquireLicensesForArculesDevicesWithoutLicense(hardwareInfo, recorderLicense, licenseSnapshot);
			}
			AcquireLicensesForHardwareAndNonCameraArculesDevices(hardwareInfo, recorderLicense, licenseSnapshot, acquireNewLicenses);
			_licenseLogger.LogDebug("AllocateLicenses : {HardwareName} : {LicenseState}", hardwareInfo.Name, hardwareInfo.LicenseState);
			return;
		}
		if (_hardwareLicenseCountEvaluator.UpdateLicenseCount(hardwareInfo))
		{
			AcquireLicenseForHardwareOnWhitelist(hardwareInfo, recorderLicense, licenseSnapshot, acquireNewLicenses);
		}
		else
		{
			if (!acquireNewLicenses)
			{
				AcquireLicensesForAlreadyLicensedDevices(hardwareInfo, recorderLicense, licenseSnapshot);
			}
			if (acquireNewLicenses)
			{
				AcquireLicensesForDevicesWithoutLicense(hardwareInfo, recorderLicense, licenseSnapshot);
			}
			AcquireLicensesForHardwareAndNonCameraDevices(hardwareInfo, recorderLicense, licenseSnapshot, acquireNewLicenses);
		}
		_licenseLogger.LogDebug("AllocateLicenses : {HardwareName} : {LicenseState}", hardwareInfo.Name, hardwareInfo.LicenseState);
	}

	private void AcquireLicensesForHardwareAndNonCameraArculesDevices(IHardwareInfo hardwareInfo, RecorderLicense recorderLicense, ILicenseSnapshot licenseSnapshot, bool acquireNewLicenses)
	{
		if (hardwareInfo.HasCameraDevices)
		{
			UpdateHardwareLicenseTypeToValidOrFree(hardwareInfo, recorderLicense);
			if (acquireNewLicenses)
			{
				SetNonCameraLicenseStatesForHardwareWithEnabledCameras(hardwareInfo, recorderLicense, licenseSnapshot);
			}
		}
		else if (hardwareInfo.Enabled)
		{
			bool flag = ProcessEnabledNonWhitelistedArculesHardware(hardwareInfo, recorderLicense, licenseSnapshot, acquireNewLicenses);
			if (acquireNewLicenses && flag)
			{
				SetAllDevicesNoLicenseRequiredForHardwareWithoutEnabledCameras(hardwareInfo, recorderLicense, licenseSnapshot);
			}
		}
	}

	private bool ProcessEnabledNonWhitelistedArculesHardware(IHardwareInfo hardwareInfo, RecorderLicense recorderLicense, ILicenseSnapshot licenseSnapshot, bool acquireNewLicenses)
	{
		if (CheckAndUpdateNonWhitelistedHardwareAlreadyLicensedOnOtherRecordingServer(recorderLicense, hardwareInfo))
		{
			return true;
		}
		TemporaryLicense temporaryLicense = _temporaryLicenseStorage.FindLicense(hardwareInfo.MacAddress);
		if (_licenseDocument.IsMacAddressLicensedHash(hardwareInfo.MacAddressHashed))
		{
			_temporaryLicenseStorage.RemoveHardwareLicense(hardwareInfo.MacAddress);
			EnsureHardwareIsLicensedOnSite(hardwareInfo, recorderLicense, isFreeLicense: false);
			recorderLicense.AddHardwareLicense(hardwareInfo.Id, new PermanentLicense(hardwareInfo.MacAddress, _licenseDocument.GetDeviceLicenseKey(hardwareInfo.MacAddress)));
			hardwareInfo.LicenseState = HardwareLicenseState.Valid;
			licenseSnapshot.SetHardwareLicenseExpire(hardwareInfo, new DateTime(DateTime.MaxValue.Ticks, DateTimeKind.Utc));
		}
		else
		{
			if (!acquireNewLicenses)
			{
				return false;
			}
			temporaryLicense = EnsureTemporaryHardwareLicenseExists(hardwareInfo, temporaryLicense, licenseSnapshot);
			if (temporaryLicense.IsExpired())
			{
				hardwareInfo.LicenseState = HardwareLicenseState.Expired;
				hardwareInfo.Devices.ForEach(delegate(IDeviceInfo d)
				{
					SetDeviceStatus(d, DeviceInfoLicenseState.Expired);
				});
				licenseSnapshot.SetHardwareLicenseExpire(hardwareInfo, temporaryLicense.ExpiryDateUtc);
				return false;
			}
			temporaryLicense.ExpiryDateUtc = EnsureDateDoesNotExceedGracePeriod(temporaryLicense.ExpiryDateUtc);
			EnsureHardwareIsLicensedOnSite(hardwareInfo, recorderLicense, isFreeLicense: false);
			recorderLicense.AddHardwareLicense(hardwareInfo.Id, temporaryLicense);
			licenseSnapshot.SetHardwareLicenseExpire(hardwareInfo, temporaryLicense.ExpiryDateUtc);
			hardwareInfo.LicenseState = HardwareLicenseState.Temporary;
		}
		return true;
	}

	private void AcquireLicensesForArculesDevicesWithoutLicense(IHardwareInfo hardwareInfo, RecorderLicense recorderLicense, ILicenseSnapshot licenseSnapshot)
	{
		if (!hardwareInfo.Enabled)
		{
			return;
		}
		foreach (IDeviceInfo device in hardwareInfo.Devices)
		{
			if (device.Enabled && !IsDeviceAlreadyLicensedOnOtherRecordingServer(recorderLicense, hardwareInfo, device) && RegisteredDeviceType.IsCamera(device.DeviceType))
			{
				ProcessEnabledArculesCameraDeviceWithoutLicense(hardwareInfo, device, recorderLicense, licenseSnapshot);
			}
		}
	}

	private void ProcessEnabledArculesCameraDeviceWithoutLicense(IHardwareInfo hardwareInfo, IDeviceInfo deviceInfo, RecorderLicense recorderLicense, ILicenseSnapshot licenseSnapshot)
	{
		if (!HasGracePeriodExpired(hardwareInfo, deviceInfo, recorderLicense, licenseSnapshot))
		{
			AddLicenseForGraceCameraDevice(hardwareInfo, deviceInfo, licenseSnapshot, recorderLicense);
		}
	}

	private void AcquireLicensesForAlreadyLicensedArculesDevices(IHardwareInfo hardwareInfo, RecorderLicense recorderLicense, ILicenseSnapshot licenseSnapshot)
	{
		foreach (IDeviceInfo cameraDevice in hardwareInfo.CameraDevices)
		{
			if (!IsDeviceAlreadyLicensedOnOtherRecordingServer(recorderLicense, hardwareInfo, cameraDevice) && cameraDevice.Enabled && hardwareInfo.Enabled)
			{
				if (_licenseDocument.IsDeviceLicensed(cameraDevice))
				{
					AddLicenseForPermanentArculesCameraDevice(hardwareInfo, cameraDevice, licenseSnapshot, recorderLicense);
				}
				else
				{
					ProcessLicenseForPreviouslyLicensedArculesDevice(hardwareInfo, cameraDevice, recorderLicense, licenseSnapshot);
				}
			}
		}
	}

	private void ProcessLicenseForPreviouslyLicensedArculesDevice(IHardwareInfo hardwareInfo, IDeviceInfo deviceInfo, RecorderLicense recorderLicense, ILicenseSnapshot licenseSnapshot)
	{
		if (licenseSnapshot.GetDeviceLicenseExpireDate(deviceInfo).Date >= DateTime.UtcNow.Date)
		{
			AddLicenseForGraceCameraDevice(hardwareInfo, deviceInfo, licenseSnapshot, recorderLicense);
		}
	}

	private void AddLicenseForPermanentArculesCameraDevice(IHardwareInfo hardwareInfo, IDeviceInfo deviceInfo, ILicenseSnapshot licenseSnapshot, RecorderLicense recorderLicense)
	{
		if (RegisteredDeviceType.IsCamera(deviceInfo.DeviceType))
		{
			_temporaryLicenseStorage.RemoveDeviceLicense(hardwareInfo.MacAddress, deviceInfo.UniqueIdentifier);
			recorderLicense.AddDeviceLicense(hardwareInfo.Id, deviceInfo.Id, "ffac8a74-85c4-4ce9-8f99-8ce7a790387d", DateTime.MaxValue.Ticks);
			licenseSnapshot.SetDeviceLicenseExpire(deviceInfo, new DateTime(DateTime.MaxValue.Ticks, DateTimeKind.Utc));
			SetDeviceStatus(deviceInfo, DeviceInfoLicenseState.Permanent);
			if (!_licenseCountManager.IsCameraFeedLicensedOnRecorder(hardwareInfo, deviceInfo, recorderLicense.RecorderId))
			{
				IRecorderInfo recorder = licenseSnapshot.GetRecorder(recorderLicense.RecorderId);
				_licenseCountManager.AddLicensedCameraFeed(recorder, hardwareInfo, deviceInfo, DateTime.MaxValue.Ticks, 1);
			}
			_licenseCountManager.UpdateCameraFeedLicensedValue(hardwareInfo, deviceInfo, DateTime.MaxValue.Ticks);
		}
	}

	private bool CheckAndUpdateWhitelistedHardwareAlreadyLicensedOnOtherRecordingServer(RecorderLicense recorderLicense, IHardwareInfo hardwareInfo)
	{
		bool flag = _licenseCountManager.IsHardwareLicensedOnSite(hardwareInfo.MacAddress);
		if (flag && hardwareInfo.LicenseState == HardwareLicenseState.Unknown)
		{
			hardwareInfo.LicenseState = HardwareLicenseState.Free;
			recorderLicense.AddHardwareLicense(hardwareInfo.Id, new TemporaryLicense(hardwareInfo.MacAddress, DateTime.UtcNow.AddDays(_licenseDocument.HardwareGracePeriod), string.Empty));
			foreach (IDeviceInfo enabledDevice in hardwareInfo.EnabledDevices)
			{
				enabledDevice.LicenseState = DeviceInfoLicenseState.NoLicenseRequired;
				recorderLicense.AddDeviceLicense(hardwareInfo.Id, enabledDevice.Id, "ffac8a74-85c4-4ce9-8099-8ce7a790387d", DateTime.UtcNow.AddDays(_licenseDocument.HardwareGracePeriod).Ticks);
			}
		}
		return flag;
	}

	private bool CheckAndUpdateNonWhitelistedHardwareAlreadyLicensedOnOtherRecordingServer(RecorderLicense recorderLicense, IHardwareInfo hardwareInfo)
	{
		bool flag = false;
		flag = ((hardwareInfo.EnabledCameras.Count <= 0) ? _licenseCountManager.IsHardwareWithoutEnabledCamerasLicensed(hardwareInfo) : _licenseCountManager.IsHardwareLicensedOnSite(hardwareInfo.MacAddress));
		if (flag && hardwareInfo.LicenseState == HardwareLicenseState.Unknown)
		{
			hardwareInfo.LicenseState = HardwareLicenseState.Free;
			recorderLicense.AddHardwareLicense(hardwareInfo.Id, new TemporaryLicense(hardwareInfo.MacAddress, DateTime.UtcNow.AddDays(_licenseDocument.HardwareGracePeriod), string.Empty));
		}
		return flag;
	}

	private bool IsDeviceAlreadyLicensedOnOtherRecordingServer(RecorderLicense recorderLicense, IHardwareInfo hardwareInfo, IDeviceInfo deviceInfo)
	{
		bool flag = _licenseCountManager.IsCameraFeedLicensedOnSite(hardwareInfo, deviceInfo);
		if (flag && deviceInfo.LicenseState == DeviceInfoLicenseState.Unknown)
		{
			deviceInfo.LicenseState = DeviceInfoLicenseState.LicensedOnOtherRecordingServer;
			if (deviceInfo.Enabled)
			{
				recorderLicense.AddDeviceLicense(hardwareInfo.Id, deviceInfo.Id, "ffac8a74-85c4-4ce9-8099-8ce7a790387d", DateTime.UtcNow.AddDays(_licenseDocument.HardwareGracePeriod).Ticks);
			}
		}
		return flag;
	}

	private void AcquireZeroLicenseForHardware(IHardwareInfo hardwareInfo, RecorderLicense recorderLicense, ILicenseSnapshot licenseSnapshot)
	{
		if (hardwareInfo == null || recorderLicense == null || licenseSnapshot == null)
		{
			return;
		}
		foreach (IDeviceInfo device in hardwareInfo.Devices)
		{
			SetDeviceNoLicenseRequired(hardwareInfo, device, licenseSnapshot, recorderLicense);
		}
		hardwareInfo.LicenseState = HardwareLicenseState.NoLicenseRequired;
	}

	private void AcquireLicenseForHardwareOnWhitelist(IHardwareInfo hardwareInfo, RecorderLicense recorderLicense, ILicenseSnapshot licenseSnapshot, bool acquireNewLicenses)
	{
		if (CheckAndUpdateWhitelistedHardwareAlreadyLicensedOnOtherRecordingServer(recorderLicense, hardwareInfo))
		{
			return;
		}
		if (hardwareInfo.Enabled)
		{
			ProcessEnabledWhiteListedHardware(hardwareInfo, recorderLicense, licenseSnapshot, acquireNewLicenses);
			if (!acquireNewLicenses)
			{
				if (hardwareInfo.LicenseState != HardwareLicenseState.Valid)
				{
					ProcessLicenseForPreviouslyLicensedHardwareOnWhitelist(hardwareInfo, recorderLicense, licenseSnapshot);
				}
				UpdateDeviceLicenseStateForWhiteListedHardware(hardwareInfo, recorderLicense, licenseSnapshot);
			}
			else
			{
				if (hardwareInfo.LicenseState == HardwareLicenseState.Temporary || hardwareInfo.LicenseState == HardwareLicenseState.ChangesWithoutActivation)
				{
					CreateRecordingLicenseForWhitelistedHardware(hardwareInfo, recorderLicense);
				}
				UpdateDeviceLicenseStateForWhiteListedHardware(hardwareInfo, recorderLicense, licenseSnapshot);
			}
		}
		else
		{
			ProcessDisabledHardware(hardwareInfo);
			UpdateDeviceLicenseStateForWhiteListedHardware(hardwareInfo, recorderLicense, licenseSnapshot);
		}
	}

	private void UpdateDeviceLicenseStateForWhiteListedHardware(IHardwareInfo hardwareInfo, RecorderLicense recorderLicense, ILicenseSnapshot licenseSnapshot)
	{
		foreach (IDeviceInfo device in hardwareInfo.Devices)
		{
			switch (hardwareInfo.LicenseState)
			{
			case HardwareLicenseState.Valid:
			case HardwareLicenseState.Temporary:
			case HardwareLicenseState.ChangesWithoutActivation:
			case HardwareLicenseState.NoLicenseRequired:
				SetDeviceNoLicenseRequired(hardwareInfo, device, licenseSnapshot, recorderLicense);
				break;
			case HardwareLicenseState.Expired:
			case HardwareLicenseState.Missing:
				SetDeviceNotLicensed(device, licenseSnapshot);
				break;
			}
		}
	}

	private void AcquireLicensesForAlreadyLicensedDevices(IHardwareInfo hardwareInfo, RecorderLicense recorderLicense, ILicenseSnapshot licenseSnapshot)
	{
		int num = 0;
		foreach (IDeviceInfo cameraDevice in hardwareInfo.CameraDevices)
		{
			if (IsDeviceAlreadyLicensedOnOtherRecordingServer(recorderLicense, hardwareInfo, cameraDevice))
			{
				continue;
			}
			if (cameraDevice.Enabled && hardwareInfo.Enabled)
			{
				if (_licenseCountManager.IsHardwareInChangesWithoutActivation(hardwareInfo.MacAddress))
				{
					_licenseCountManager.RemoveHardwareFromChangesWithoutActivation(hardwareInfo.MacAddress);
				}
				ChangeWithoutActivationType changeWithoutActivationType = IsDeviceInChangesWithoutActivation(hardwareInfo, cameraDevice);
				bool flag = IsLicenseAvailable(hardwareInfo.IsInterconnectedProduct, 1);
				bool flag2 = (hardwareInfo.IsInterconnectedProduct && _licenseDocument.IsIcpDeviceLicensed(cameraDevice, num)) || (!hardwareInfo.IsInterconnectedProduct && _licenseDocument.IsDeviceLicensed(cameraDevice));
				if (flag && flag2)
				{
					AddLicenseForPermanentCameraDevice(hardwareInfo, cameraDevice, licenseSnapshot, recorderLicense);
					if (hardwareInfo.IsInterconnectedProduct)
					{
						num++;
					}
				}
				else if (changeWithoutActivationType != ChangeWithoutActivationType.NotSet && flag && _licenseCountManager.IsDeviceInChangesWithoutActivation(hardwareInfo, cameraDevice))
				{
					AddLicenseForChangeWithoutActivationCameraDevice(hardwareInfo, cameraDevice, licenseSnapshot, recorderLicense, ref changeWithoutActivationType);
				}
				else
				{
					ProcessLicenseForPreviouslyLicensedDevice(hardwareInfo, cameraDevice, recorderLicense, licenseSnapshot, flag, changeWithoutActivationType);
				}
				continue;
			}
			ChangeWithoutActivationType changeWithoutActivationType2 = IsDeviceInChangesWithoutActivation(hardwareInfo, cameraDevice);
			if (changeWithoutActivationType2 != ChangeWithoutActivationType.NotSet)
			{
				if (changeWithoutActivationType2 == ChangeWithoutActivationType.CurrentYear)
				{
					_licenseCountManager.AddDeviceToChangesWithoutActivation(hardwareInfo, cameraDevice);
					SetDeviceStatus(cameraDevice, DeviceInfoLicenseState.ChangeWithoutActivation);
				}
				recorderLicense.AddDeviceLicense(hardwareInfo.Id, cameraDevice.Id, "ffac8a74-85c4-4ce9-8099-8ce7a790387d", DateTime.UtcNow.AddDays(_licenseDocument.HardwareGracePeriod).Ticks);
			}
		}
	}

	private void AddLicenseForChangeWithoutActivationHardware(IHardwareInfo hardwareInfo, RecorderLicense recorderLicense, ILicenseSnapshot licenseSnapshot, TemporaryLicense tempLicense, ChangeWithoutActivationType changeWithoutActivationType)
	{
		tempLicense = EnsureTemporaryHardwareLicenseExists(hardwareInfo, tempLicense, licenseSnapshot);
		if (changeWithoutActivationType == ChangeWithoutActivationType.CurrentYear)
		{
			_licenseCountManager.AddHardwareToChangesWithoutActivation(hardwareInfo.MacAddress);
			tempLicense.SetChangesWithoutActivationToken(_licenseDocument.ActivatedDate);
		}
		EnsureHardwareIsLicensedOnSite(hardwareInfo, recorderLicense, isFreeLicense: false);
		recorderLicense.AddHardwareLicense(hardwareInfo.Id, tempLicense);
		licenseSnapshot.SetHardwareLicenseExpire(hardwareInfo, DateTime.UtcNow.AddDays(_licenseDocument.HardwareGracePeriod));
		hardwareInfo.LicenseState = HardwareLicenseState.ChangesWithoutActivation;
	}

	private void ProcessLicenseForPreviouslyLicensedHardwareOnWhitelist(IHardwareInfo hardwareInfo, RecorderLicense recorderLicense, ILicenseSnapshot licenseSnapshot)
	{
		DateTime hardwareLicenseExpireDate = licenseSnapshot.GetHardwareLicenseExpireDate(hardwareInfo);
		if (hardwareLicenseExpireDate.Ticks != 0L)
		{
			TemporaryLicense temporaryLicense = _temporaryLicenseStorage.FindLicense(hardwareInfo.MacAddress);
			ChangeWithoutActivationType changeWithoutActivationType = IsHardwareInChangesWithoutActivation(hardwareInfo);
			bool flag = IsLicenseAvailable(hardwareInfo.IsInterconnectedProduct, 1);
			if (!hardwareInfo.Enabled)
			{
				hardwareInfo.LicenseState = HardwareLicenseState.NoLicenseRequired;
			}
			else if (flag && temporaryLicense != null && changeWithoutActivationType != ChangeWithoutActivationType.NotSet && AllowChangesWithoutActivation(hardwareInfo))
			{
				AddLicenseForChangeWithoutActivationHardware(hardwareInfo, recorderLicense, licenseSnapshot, temporaryLicense, changeWithoutActivationType);
			}
			else if (flag)
			{
				AddLicenseForHardwareOnWhitelist(hardwareInfo, recorderLicense, licenseSnapshot, hardwareLicenseExpireDate, temporaryLicense);
			}
		}
	}

	private void ProcessLicenseForPreviouslyLicensedDevice(IHardwareInfo hardwareInfo, IDeviceInfo deviceInfo, RecorderLicense recorderLicense, ILicenseSnapshot licenseSnapshot, bool isLicenseAvailable, ChangeWithoutActivationType changeWithoutActivationType)
	{
		DateTime deviceLicenseExpireDate = licenseSnapshot.GetDeviceLicenseExpireDate(deviceInfo);
		if (isLicenseAvailable && deviceLicenseExpireDate.Date >= DateTime.UtcNow.Date)
		{
			if (AllowChangesWithoutActivation(hardwareInfo))
			{
				AddLicenseForChangeWithoutActivationCameraDevice(hardwareInfo, deviceInfo, licenseSnapshot, recorderLicense, ref changeWithoutActivationType);
			}
			else
			{
				AddLicenseForGraceCameraDevice(hardwareInfo, deviceInfo, licenseSnapshot, recorderLicense);
			}
		}
	}

	private void AddLicenseForHardwareOnWhitelist(IHardwareInfo hardwareInfo, RecorderLicense recorderLicense, ILicenseSnapshot licenseSnapshot, DateTime expireDate, TemporaryLicense temporaryLicense)
	{
		if (temporaryLicense == null)
		{
			temporaryLicense = new TemporaryLicense(hardwareInfo.MacAddress, expireDate, string.Empty);
			_temporaryLicenseStorage.AddLicense(temporaryLicense);
		}
		temporaryLicense.ExpiryDateUtc = EnsureDateDoesNotExceedGracePeriod(temporaryLicense.ExpiryDateUtc);
		if (temporaryLicense.IsExpired())
		{
			hardwareInfo.LicenseState = HardwareLicenseState.Expired;
			return;
		}
		hardwareInfo.LicenseState = HardwareLicenseState.Temporary;
		EnsureHardwareIsLicensedOnSite(hardwareInfo, recorderLicense, isFreeLicense: false);
		recorderLicense.AddHardwareLicense(hardwareInfo.Id, temporaryLicense);
		licenseSnapshot.SetHardwareLicenseExpire(hardwareInfo, temporaryLicense.ExpiryDateUtc);
	}

	private void AcquireLicensesForDevicesWithoutLicense(IHardwareInfo hardwareInfo, RecorderLicense recorderLicense, ILicenseSnapshot licenseSnapshot)
	{
		if (!hardwareInfo.Enabled)
		{
			return;
		}
		foreach (IDeviceInfo device in hardwareInfo.Devices)
		{
			if (device.LicenseState == DeviceInfoLicenseState.Unknown && device.Enabled && !IsDeviceAlreadyLicensedOnOtherRecordingServer(recorderLicense, hardwareInfo, device))
			{
				if (_licenseCountManager.IsHardwareInChangesWithoutActivation(hardwareInfo.MacAddress))
				{
					_licenseCountManager.RemoveHardwareFromChangesWithoutActivation(hardwareInfo.MacAddress);
				}
				bool flag = RegisteredDeviceType.IsCamera(device.DeviceType);
				bool isLicenseAvailable = IsLicenseAvailable(hardwareInfo.IsInterconnectedProduct, 1);
				if (flag)
				{
					ProcessEnabledCameraDeviceWithoutLicense(hardwareInfo, device, recorderLicense, licenseSnapshot, isLicenseAvailable);
				}
			}
		}
	}

	private void ProcessEnabledCameraDeviceWithoutLicense(IHardwareInfo hardwareInfo, IDeviceInfo deviceInfo, RecorderLicense recorderLicense, ILicenseSnapshot licenseSnapshot, bool isLicenseAvailable)
	{
		if (isLicenseAvailable)
		{
			if (AllowChangesWithoutActivation(hardwareInfo) || !HasGracePeriodExpired(hardwareInfo, deviceInfo, recorderLicense, licenseSnapshot))
			{
				if (AllowChangesWithoutActivation(hardwareInfo))
				{
					ChangeWithoutActivationType changeWithoutActivationType = IsDeviceInChangesWithoutActivation(hardwareInfo, deviceInfo);
					AddLicenseForChangeWithoutActivationCameraDevice(hardwareInfo, deviceInfo, licenseSnapshot, recorderLicense, ref changeWithoutActivationType);
				}
				else
				{
					_licenseCountManager.RemoveDeviceFromChangesWithoutActivation(LicenseFactory.CreateHardwareDeviceItem(hardwareInfo, deviceInfo));
					AddLicenseForGraceCameraDevice(hardwareInfo, deviceInfo, licenseSnapshot, recorderLicense);
				}
			}
		}
		else
		{
			IDeviceLicenseInfo deviceLicense = licenseSnapshot.GetDeviceLicense(deviceInfo);
			if (deviceLicense != null && deviceLicense.Expires < DateTime.UtcNow)
			{
				SetDeviceStatus(deviceInfo, DeviceInfoLicenseState.Expired);
			}
			else
			{
				SetDeviceNotLicensed(deviceInfo, licenseSnapshot);
			}
		}
	}

	private bool HasGracePeriodExpired(IHardwareInfo hardwareInfo, IDeviceInfo deviceInfo, RecorderLicense recorderLicense, ILicenseSnapshot licenseSnapshot)
	{
		long ticks = licenseSnapshot.GetDeviceLicenseExpireDate(deviceInfo).Ticks;
		if (ticks > 0 && ticks < DateTime.UtcNow.Date.Ticks)
		{
			AddLicenseForGraceCameraDevice(hardwareInfo, deviceInfo, licenseSnapshot, recorderLicense);
			return true;
		}
		return false;
	}

	private void AcquireLicensesForHardwareAndNonCameraDevices(IHardwareInfo hardwareInfo, RecorderLicense recorderLicense, ILicenseSnapshot licenseSnapshot, bool acquireNewLicenses)
	{
		if (hardwareInfo.HasCameraDevices)
		{
			UpdateHardwareLicenseTypeToValidOrFree(hardwareInfo, recorderLicense);
			if (acquireNewLicenses)
			{
				SetNonCameraLicenseStatesForHardwareWithEnabledCameras(hardwareInfo, recorderLicense, licenseSnapshot);
			}
		}
		else if (hardwareInfo.Enabled)
		{
			bool flag = ProcessEnabledNonWhitelistedHardware(hardwareInfo, recorderLicense, licenseSnapshot, acquireNewLicenses);
			if (acquireNewLicenses && flag)
			{
				SetAllDevicesNoLicenseRequiredForHardwareWithoutEnabledCameras(hardwareInfo, recorderLicense, licenseSnapshot);
			}
		}
		else
		{
			ProcessDisabledHardware(hardwareInfo);
		}
	}

	private void SetNonCameraLicenseStatesForHardwareWithEnabledCameras(IHardwareInfo hardwareInfo, RecorderLicense recorderLicense, ILicenseSnapshot licenseSnapshot)
	{
		bool hasLicensedCameras = hardwareInfo.HasLicensedCameras;
		foreach (IDeviceInfo enabledNonCameraDevice in hardwareInfo.EnabledNonCameraDevices)
		{
			if (hasLicensedCameras)
			{
				SetDeviceNoLicenseRequired(hardwareInfo, enabledNonCameraDevice, licenseSnapshot, recorderLicense);
			}
			else
			{
				SetDeviceNotLicensed(enabledNonCameraDevice, licenseSnapshot);
			}
		}
	}

	private void SetAllDevicesNoLicenseRequiredForHardwareWithoutEnabledCameras(IHardwareInfo hardwareInfo, RecorderLicense recorderLicense, ILicenseSnapshot licenseSnapshot)
	{
		foreach (IDeviceInfo enabledDevice in hardwareInfo.EnabledDevices)
		{
			SetDeviceNoLicenseRequired(hardwareInfo, enabledDevice, licenseSnapshot, recorderLicense);
		}
	}

	private void CreateRecordingLicenseForWhitelistedHardware(IHardwareInfo hardwareInfo, RecorderLicense recorderLicense)
	{
		TemporaryLicense temporaryLicense = _temporaryLicenseStorage.FindLicense(hardwareInfo.MacAddress);
		if (temporaryLicense == null)
		{
			DateTime expiryDateUtc = DateTime.UtcNow.AddDays(_licenseDocument.HardwareGracePeriod);
			temporaryLicense = new TemporaryLicense(hardwareInfo.MacAddress, expiryDateUtc, string.Empty);
			_temporaryLicenseStorage.AddLicense(temporaryLicense);
		}
		EnsureHardwareIsLicensedOnSite(hardwareInfo, recorderLicense, isFreeLicense: false);
		recorderLicense.AddHardwareLicense(hardwareInfo.Id, temporaryLicense);
	}

	private void UpdateHardwareLicenseTypeToValidOrFree(IHardwareInfo hardwareInfo, RecorderLicense recorderLicense)
	{
		LicenseBase licenseBase;
		if (_licenseDocument.IsMacAddressLicensedHash(hardwareInfo.MacAddressHashed))
		{
			hardwareInfo.LicenseState = HardwareLicenseState.Valid;
			licenseBase = new PermanentLicense(hardwareInfo.MacAddress, _licenseDocument.GetDeviceLicenseKey(hardwareInfo.MacAddress));
			_temporaryLicenseStorage.RemoveHardwareLicense(hardwareInfo.MacAddress);
		}
		else
		{
			hardwareInfo.LicenseState = HardwareLicenseState.Free;
			DateTime expiryDateUtc = DateTime.UtcNow.AddDays(_licenseDocument.HardwareGracePeriod);
			licenseBase = new TemporaryLicense(hardwareInfo.MacAddress, expiryDateUtc, string.Empty);
			_temporaryLicenseStorage.AddLicense(licenseBase as TemporaryLicense);
		}
		EnsureHardwareIsLicensedOnSite(hardwareInfo, recorderLicense, isFreeLicense: true);
		recorderLicense.AddHardwareLicense(hardwareInfo.Id, licenseBase);
	}

	private void ProcessEnabledWhiteListedHardware(IHardwareInfo hardwareInfo, RecorderLicense recorderLicense, ILicenseSnapshot licenseSnapshot, bool acquireNewLicenses)
	{
		TemporaryLicense temporaryLicense = _temporaryLicenseStorage.FindLicense(hardwareInfo.MacAddress);
		if (IsLicenseAvailable(hardwareInfo.IsInterconnectedProduct, 1))
		{
			ChangeWithoutActivationType changeWithoutActivationType = IsHardwareInChangesWithoutActivation(hardwareInfo);
			_licenseLogger.LogDebug("ProcessEnabledWhiteListedHardware : {HardwareName} : {ChangeWithoutActivationType}", hardwareInfo.Name, changeWithoutActivationType);
			if (_licenseDocument.IsMacAddressLicensedHash(hardwareInfo.MacAddressHashed))
			{
				_temporaryLicenseStorage.RemoveHardwareLicense(hardwareInfo.MacAddress);
				if (_licenseCountManager.IsHardwareInChangesWithoutActivation(hardwareInfo.MacAddress))
				{
					_licenseCountManager.RemoveHardwareFromChangesWithoutActivation(hardwareInfo.MacAddress);
				}
				EnsureHardwareIsLicensedOnSite(hardwareInfo, recorderLicense, isFreeLicense: false);
				recorderLicense.AddHardwareLicense(hardwareInfo.Id, new PermanentLicense(hardwareInfo.MacAddress, _licenseDocument.GetDeviceLicenseKey(hardwareInfo.MacAddress)));
				hardwareInfo.LicenseState = HardwareLicenseState.Valid;
				licenseSnapshot.SetHardwareLicenseExpire(hardwareInfo, new DateTime(DateTime.MaxValue.Ticks, DateTimeKind.Utc));
			}
			else if (acquireNewLicenses && changeWithoutActivationType != ChangeWithoutActivationType.NotSet)
			{
				AddLicenseForChangeWithoutActivationHardware(hardwareInfo, recorderLicense, licenseSnapshot, temporaryLicense, changeWithoutActivationType);
			}
			else if (acquireNewLicenses && AllowChangesWithoutActivation(hardwareInfo))
			{
				temporaryLicense = EnsureTemporaryHardwareLicenseExists(hardwareInfo, temporaryLicense, licenseSnapshot);
				_licenseCountManager.AddHardwareToChangesWithoutActivation(hardwareInfo.MacAddress);
				recorderLicense.AddHardwareLicense(hardwareInfo.Id, temporaryLicense);
				licenseSnapshot.SetHardwareLicenseExpire(hardwareInfo, DateTime.UtcNow.AddDays(_licenseDocument.HardwareGracePeriod));
				temporaryLicense.SetChangesWithoutActivationToken(_licenseDocument.ActivatedDate);
				hardwareInfo.LicenseState = HardwareLicenseState.ChangesWithoutActivation;
			}
			else
			{
				if (!acquireNewLicenses)
				{
					return;
				}
				temporaryLicense = EnsureTemporaryHardwareLicenseExists(hardwareInfo, temporaryLicense, licenseSnapshot);
				if (temporaryLicense.IsExpired())
				{
					hardwareInfo.LicenseState = HardwareLicenseState.Expired;
					hardwareInfo.Devices.ForEach(delegate(IDeviceInfo d)
					{
						SetDeviceStatus(d, DeviceInfoLicenseState.Expired);
					});
					licenseSnapshot.SetHardwareLicenseExpire(hardwareInfo, temporaryLicense.ExpiryDateUtc);
				}
				else
				{
					temporaryLicense.ExpiryDateUtc = EnsureDateDoesNotExceedGracePeriod(temporaryLicense.ExpiryDateUtc);
					EnsureHardwareIsLicensedOnSite(hardwareInfo, recorderLicense, isFreeLicense: false);
					recorderLicense.AddHardwareLicense(hardwareInfo.Id, temporaryLicense);
					licenseSnapshot.SetHardwareLicenseExpire(hardwareInfo, temporaryLicense.ExpiryDateUtc);
					hardwareInfo.LicenseState = HardwareLicenseState.Temporary;
				}
			}
		}
		else
		{
			if (_licenseCountManager.IsHardwareLicensedOnSite(hardwareInfo.MacAddress))
			{
				_licenseCountManager.UpdateHardwareLicenseValue(hardwareInfo.MacAddress, 0L);
			}
			if (licenseSnapshot.AddUnlicensedHardware(hardwareInfo))
			{
				hardwareInfo.LicenseState = HardwareLicenseState.Missing;
			}
			else
			{
				hardwareInfo.LicenseState = HardwareLicenseState.MissingDuplicate;
			}
			licenseSnapshot.RemoveHardwareLicense(hardwareInfo);
			if (temporaryLicense != null)
			{
				temporaryLicense.ExpiryDateUtc = new DateTime(0L, DateTimeKind.Utc);
				_temporaryLicenseStorage.RemoveHardwareLicense(hardwareInfo.MacAddress);
			}
		}
	}

	private bool ProcessEnabledNonWhitelistedHardware(IHardwareInfo hardwareInfo, RecorderLicense recorderLicense, ILicenseSnapshot licenseSnapshot, bool acquireNewLicenses)
	{
		if (CheckAndUpdateNonWhitelistedHardwareAlreadyLicensedOnOtherRecordingServer(recorderLicense, hardwareInfo))
		{
			return true;
		}
		TemporaryLicense temporaryLicense = _temporaryLicenseStorage.FindLicense(hardwareInfo.MacAddress);
		if (IsLicenseAvailable(hardwareInfo.IsInterconnectedProduct, 1))
		{
			ChangeWithoutActivationType changeWithoutActivationType = IsHardwareInChangesWithoutActivation(hardwareInfo);
			if (_licenseDocument.IsMacAddressLicensedHash(hardwareInfo.MacAddressHashed))
			{
				_temporaryLicenseStorage.RemoveHardwareLicense(hardwareInfo.MacAddress);
				if (_licenseCountManager.IsHardwareInChangesWithoutActivation(hardwareInfo.MacAddress))
				{
					_licenseCountManager.RemoveHardwareFromChangesWithoutActivation(hardwareInfo.MacAddress);
				}
				_licenseCountManager.RemoveDevicesFromChangesWithoutActivation(hardwareInfo);
				hardwareInfo.CameraDevices.ForEach(delegate(IDeviceInfo d)
				{
					RemoveDeviceFromChangesWithoutActivation(d);
				});
				EnsureHardwareIsLicensedOnSite(hardwareInfo, recorderLicense, isFreeLicense: false);
				recorderLicense.AddHardwareLicense(hardwareInfo.Id, new PermanentLicense(hardwareInfo.MacAddress, _licenseDocument.GetDeviceLicenseKey(hardwareInfo.MacAddress)));
				hardwareInfo.LicenseState = HardwareLicenseState.Valid;
				licenseSnapshot.SetHardwareLicenseExpire(hardwareInfo, new DateTime(DateTime.MaxValue.Ticks, DateTimeKind.Utc));
			}
			else if (acquireNewLicenses && changeWithoutActivationType != ChangeWithoutActivationType.NotSet)
			{
				AddLicenseForChangeWithoutActivationHardware(hardwareInfo, recorderLicense, licenseSnapshot, temporaryLicense, changeWithoutActivationType);
			}
			else if (acquireNewLicenses && AllowChangesWithoutActivation(hardwareInfo))
			{
				temporaryLicense = EnsureTemporaryHardwareLicenseExists(hardwareInfo, temporaryLicense, licenseSnapshot);
				_licenseCountManager.AddHardwareToChangesWithoutActivation(hardwareInfo.MacAddress);
				recorderLicense.AddHardwareLicense(hardwareInfo.Id, temporaryLicense);
				licenseSnapshot.SetHardwareLicenseExpire(hardwareInfo, DateTime.UtcNow.AddDays(_licenseDocument.HardwareGracePeriod));
				temporaryLicense.SetChangesWithoutActivationToken(_licenseDocument.ActivatedDate);
				hardwareInfo.LicenseState = HardwareLicenseState.ChangesWithoutActivation;
			}
			else
			{
				if (!acquireNewLicenses)
				{
					return false;
				}
				temporaryLicense = EnsureTemporaryHardwareLicenseExists(hardwareInfo, temporaryLicense, licenseSnapshot);
				if (temporaryLicense.IsExpired())
				{
					hardwareInfo.LicenseState = HardwareLicenseState.Expired;
					hardwareInfo.Devices.ForEach(delegate(IDeviceInfo d)
					{
						SetDeviceStatus(d, DeviceInfoLicenseState.Expired);
					});
					licenseSnapshot.SetHardwareLicenseExpire(hardwareInfo, temporaryLicense.ExpiryDateUtc);
					return false;
				}
				temporaryLicense.ExpiryDateUtc = EnsureDateDoesNotExceedGracePeriod(temporaryLicense.ExpiryDateUtc);
				EnsureHardwareIsLicensedOnSite(hardwareInfo, recorderLicense, isFreeLicense: false);
				recorderLicense.AddHardwareLicense(hardwareInfo.Id, temporaryLicense);
				licenseSnapshot.SetHardwareLicenseExpire(hardwareInfo, temporaryLicense.ExpiryDateUtc);
				hardwareInfo.LicenseState = HardwareLicenseState.Temporary;
			}
			return true;
		}
		if (_licenseCountManager.IsHardwareLicensedOnSite(hardwareInfo.MacAddress))
		{
			_licenseCountManager.UpdateHardwareLicenseValue(hardwareInfo.MacAddress, 0L);
		}
		if (licenseSnapshot.AddUnlicensedHardware(hardwareInfo))
		{
			hardwareInfo.LicenseState = HardwareLicenseState.Missing;
		}
		else
		{
			hardwareInfo.LicenseState = HardwareLicenseState.MissingDuplicate;
		}
		licenseSnapshot.RemoveHardwareLicense(hardwareInfo);
		if (temporaryLicense != null)
		{
			temporaryLicense.ExpiryDateUtc = new DateTime(0L, DateTimeKind.Utc);
			_temporaryLicenseStorage.RemoveHardwareLicense(hardwareInfo.MacAddress);
		}
		return false;
	}

	private void ProcessDisabledHardware(IHardwareInfo hardwareInfo)
	{
		TemporaryLicense temporaryLicense = _temporaryLicenseStorage.FindLicense(hardwareInfo.MacAddress);
		hardwareInfo.LicenseState = HardwareLicenseState.NoLicenseRequired;
		if (temporaryLicense != null)
		{
			ChangeWithoutActivationType changeWithoutActivationType = IsHardwareInChangesWithoutActivation(hardwareInfo);
			if (changeWithoutActivationType != ChangeWithoutActivationType.NotSet && changeWithoutActivationType == ChangeWithoutActivationType.CurrentYear && AllowChangesWithoutActivation(hardwareInfo))
			{
				temporaryLicense.SetChangesWithoutActivationToken(_licenseDocument.ActivatedDate);
				_licenseCountManager.AddHardwareToChangesWithoutActivation(hardwareInfo.MacAddress);
			}
		}
	}

	private TemporaryLicense EnsureTemporaryHardwareLicenseExists(IHardwareInfo hardwareInfo, TemporaryLicense temporaryLicense, ILicenseSnapshot licenseSnapshot)
	{
		if (temporaryLicense == null)
		{
			DateTime expiryDateUtc = licenseSnapshot.GetHardwareLicenseExpireDate(hardwareInfo);
			if (expiryDateUtc.Ticks == 0L)
			{
				expiryDateUtc = DateTime.UtcNow.AddDays(_licenseDocument.HardwareGracePeriod);
			}
			temporaryLicense = new TemporaryLicense(hardwareInfo.MacAddress, expiryDateUtc, null);
			_temporaryLicenseStorage.AddLicense(temporaryLicense);
		}
		return temporaryLicense;
	}

	private TemporaryDeviceLicense EnsureTemporaryDeviceLicenseExists(IHardwareInfo hardwareInfo, IDeviceInfo deviceInfo, TemporaryDeviceLicense temporaryLicense, long expiryValue, bool setChangesWithoutActivation)
	{
		if (temporaryLicense == null)
		{
			temporaryLicense = _temporaryLicenseStorage.FindDeviceLicense(hardwareInfo.MacAddress, deviceInfo.UniqueIdentifier);
			if (temporaryLicense == null || temporaryLicense.ExpiryDateUtc.Ticks == DateTime.MaxValue.Ticks)
			{
				temporaryLicense = new TemporaryDeviceLicense(hardwareInfo.MacAddress, deviceInfo.UniqueIdentifier, new DateTime(expiryValue, DateTimeKind.Utc), null);
			}
			if (!temporaryLicense.IsChangedWithoutActivation() && setChangesWithoutActivation)
			{
				temporaryLicense = new TemporaryDeviceLicense(hardwareInfo.MacAddress, deviceInfo.UniqueIdentifier, DateTime.UtcNow.AddDays(_licenseDocument.HardwareGracePeriod), null);
				temporaryLicense.SetChangesWithoutActivationToken(_licenseDocument.ActivatedDate);
			}
			_temporaryLicenseStorage.RemoveDeviceLicense(temporaryLicense.MacAddress, temporaryLicense.UniqueIdentifier);
			_temporaryLicenseStorage.AddDeviceLicense(temporaryLicense);
		}
		return temporaryLicense;
	}

	private bool IsLicenseAvailable(bool isInterconnectedProduct, int countRequired)
	{
		int num = _licenseCountManager.CountOfLicensesUsed(isInterconnectedProduct);
		int num2 = (isInterconnectedProduct ? _cameraIcpFeeds : _channelLicenseCount);
		return num + countRequired <= num2;
	}

	private void SetDeviceNoLicenseRequired(IHardwareInfo hardwareInfo, IDeviceInfo deviceInfo, ILicenseSnapshot licenseSnapshot, RecorderLicense recorderLicense)
	{
		licenseSnapshot.SetDeviceLicenseExpire(deviceInfo, new DateTime(DateTime.MaxValue.Ticks, DateTimeKind.Utc));
		SetDeviceStatus(deviceInfo, DeviceInfoLicenseState.NoLicenseRequired);
		recorderLicense.AddDeviceLicense(hardwareInfo.Id, deviceInfo.Id, "ffac8a74-85c4-4ce9-8f99-8ce7a790387d", DateTime.MaxValue.Ticks);
	}

	private void SetDeviceNotLicensed(IDeviceInfo deviceInfo, ILicenseSnapshot licenseSnapshot)
	{
		if (licenseSnapshot.AddUnlicensedDevice(deviceInfo))
		{
			SetDeviceStatus(deviceInfo, DeviceInfoLicenseState.Unlicensed);
		}
		else
		{
			SetDeviceStatus(deviceInfo, DeviceInfoLicenseState.UnlicensedDuplicate);
		}
		licenseSnapshot.RemoveDeviceLicense(deviceInfo);
	}

	private void AddLicenseForGraceCameraDevice(IHardwareInfo hardwareInfo, IDeviceInfo deviceInfo, ILicenseSnapshot licenseSnapshot, RecorderLicense recorderLicense)
	{
		if (!RegisteredDeviceType.IsCamera(deviceInfo.DeviceType))
		{
			return;
		}
		DateTime deviceLicenseExpireDate = licenseSnapshot.GetDeviceLicenseExpireDate(deviceInfo);
		long ticks = EnsureDateDoesNotExceedGracePeriod(deviceLicenseExpireDate).Ticks;
		if (ticks == 0L || ticks == DateTime.MaxValue.Ticks)
		{
			ticks = DateTime.UtcNow.AddDays(_licenseDocument.HardwareGracePeriod).Ticks;
		}
		TemporaryDeviceLicense temporaryLicense = _temporaryLicenseStorage.FindDeviceLicense(hardwareInfo.MacAddress, deviceInfo.UniqueIdentifier);
		temporaryLicense = EnsureTemporaryDeviceLicenseExists(hardwareInfo, deviceInfo, temporaryLicense, ticks, setChangesWithoutActivation: false);
		if (temporaryLicense.IsExpired())
		{
			SetDeviceStatus(deviceInfo, DeviceInfoLicenseState.Expired);
			return;
		}
		SetDeviceStatus(deviceInfo, DeviceInfoLicenseState.Grace);
		recorderLicense.AddDeviceLicense(hardwareInfo.Id, deviceInfo.Id, "ffac8a74-85c4-4ce9-8099-8ce7a790387d", ticks);
		licenseSnapshot.SetDeviceLicenseExpire(deviceInfo, new DateTime(ticks, DateTimeKind.Utc));
		if (!_licenseCountManager.IsCameraFeedLicensedOnSite(hardwareInfo, deviceInfo))
		{
			IRecorderInfo recorder = licenseSnapshot.GetRecorder(recorderLicense.RecorderId);
			_licenseCountManager.AddLicensedCameraFeed(recorder, hardwareInfo, deviceInfo, temporaryLicense.ExpiryDateUtc.Ticks, 1);
		}
		_licenseCountManager.UpdateCameraFeedLicensedValue(hardwareInfo, deviceInfo, temporaryLicense.ExpiryDateUtc.Ticks);
	}

	private void AddLicenseForChangeWithoutActivationCameraDevice(IHardwareInfo hardwareInfo, IDeviceInfo deviceInfo, ILicenseSnapshot licenseSnapshot, RecorderLicense recorderLicense, ref ChangeWithoutActivationType changeWithoutActivationType)
	{
		if (RegisteredDeviceType.IsCamera(deviceInfo.DeviceType) && hardwareInfo.Enabled)
		{
			long ticks = licenseSnapshot.GetDeviceLicenseExpireDate(deviceInfo).Ticks;
			if (ticks == 0L || ticks == DateTime.MaxValue.Ticks)
			{
				ticks = DateTime.UtcNow.AddDays(_licenseDocument.HardwareGracePeriod).Ticks;
			}
			TemporaryDeviceLicense temporaryDeviceLicense = EnsureTemporaryDeviceLicenseExists(hardwareInfo, deviceInfo, null, ticks, setChangesWithoutActivation: true);
			if (changeWithoutActivationType == ChangeWithoutActivationType.NotSet)
			{
				temporaryDeviceLicense.SetChangesWithoutActivationToken(_licenseDocument.ActivatedDate);
				changeWithoutActivationType = ChangeWithoutActivationType.CurrentYear;
			}
			if (changeWithoutActivationType != ChangeWithoutActivationType.PreviousYear && !_licenseCountManager.IsDeviceInChangesWithoutActivation(hardwareInfo, deviceInfo))
			{
				_licenseCountManager.AddDeviceToChangesWithoutActivation(hardwareInfo, deviceInfo);
			}
			temporaryDeviceLicense = new TemporaryDeviceLicense(hardwareInfo.MacAddress, deviceInfo.UniqueIdentifier, DateTime.UtcNow.AddDays(100.0), temporaryDeviceLicense.ChangesWithoutActivationToken);
			if (deviceInfo.Enabled)
			{
				recorderLicense.AddDeviceLicense(hardwareInfo.Id, deviceInfo.Id, "ffac8a74-85c4-4ce9-8099-8ce7a790387d", temporaryDeviceLicense.ExpiryDateUtc.Ticks);
				licenseSnapshot.SetDeviceLicenseExpire(deviceInfo, temporaryDeviceLicense.ExpiryDateUtc);
			}
			SetDeviceStatus(deviceInfo, DeviceInfoLicenseState.ChangeWithoutActivation);
			if (!_licenseCountManager.IsCameraFeedLicensedOnRecorder(hardwareInfo, deviceInfo, recorderLicense.RecorderId))
			{
				IRecorderInfo recorder = licenseSnapshot.GetRecorder(recorderLicense.RecorderId);
				_licenseCountManager.AddLicensedCameraFeed(recorder, hardwareInfo, deviceInfo, temporaryDeviceLicense.ExpiryDateUtc.Ticks, 1);
			}
			_licenseCountManager.UpdateCameraFeedLicensedValue(hardwareInfo, deviceInfo, temporaryDeviceLicense.ExpiryDateUtc.Ticks);
		}
	}

	private void AddLicenseForPermanentCameraDevice(IHardwareInfo hardwareInfo, IDeviceInfo deviceInfo, ILicenseSnapshot licenseSnapshot, RecorderLicense recorderLicense)
	{
		if (RegisteredDeviceType.IsCamera(deviceInfo.DeviceType))
		{
			_temporaryLicenseStorage.RemoveDeviceLicense(hardwareInfo.MacAddress, deviceInfo.UniqueIdentifier);
			recorderLicense.AddDeviceLicense(hardwareInfo.Id, deviceInfo.Id, "ffac8a74-85c4-4ce9-8f99-8ce7a790387d", DateTime.MaxValue.Ticks);
			licenseSnapshot.SetDeviceLicenseExpire(deviceInfo, new DateTime(DateTime.MaxValue.Ticks, DateTimeKind.Utc));
			SetDeviceStatus(deviceInfo, DeviceInfoLicenseState.Permanent);
			if (_licenseCountManager.IsDeviceInChangesWithoutActivation(hardwareInfo, deviceInfo))
			{
				_licenseCountManager.RemoveDeviceFromChangesWithoutActivation(LicenseFactory.CreateHardwareDeviceItem(hardwareInfo, deviceInfo));
			}
			if (!_licenseCountManager.IsCameraFeedLicensedOnRecorder(hardwareInfo, deviceInfo, recorderLicense.RecorderId))
			{
				IRecorderInfo recorder = licenseSnapshot.GetRecorder(recorderLicense.RecorderId);
				_licenseCountManager.AddLicensedCameraFeed(recorder, hardwareInfo, deviceInfo, DateTime.MaxValue.Ticks, 1);
			}
			_licenseCountManager.UpdateCameraFeedLicensedValue(hardwareInfo, deviceInfo, DateTime.MaxValue.Ticks);
		}
	}

	private void SetDeviceStatus(IDeviceInfo deviceInfo, DeviceInfoLicenseState licenseState)
	{
		deviceInfo.LicenseState = licenseState;
		_licenseCountManager.DeviceStatus[deviceInfo.Id] = licenseState;
	}

	private bool AllowChangesWithoutActivation(IHardwareInfo hardwareInfo)
	{
		int usedChangesWithoutActivation;
		return AllowChangesWithoutActivation(hardwareInfo, out usedChangesWithoutActivation);
	}

	private bool AllowChangesWithoutActivation(IHardwareInfo hardwareInfo, out int usedChangesWithoutActivation)
	{
		usedChangesWithoutActivation = _licenseCountManager.UsedChangesWithoutActivation;
		if (!hardwareInfo.IsInterconnectedProduct)
		{
			if (usedChangesWithoutActivation >= _totalAllowedChangesWithoutActivation)
			{
				return _licenseCountManager.IsHardwareInChangesWithoutActivation(hardwareInfo.MacAddress);
			}
			return true;
		}
		return false;
	}

	private ChangeWithoutActivationType IsHardwareInChangesWithoutActivation(IHardwareInfo hardwareInfo)
	{
		TemporaryLicense temporaryLicense = _temporaryLicenseStorage.FindLicense(hardwareInfo.MacAddress);
		if (temporaryLicense == null || !temporaryLicense.IsChangedWithoutActivation())
		{
			return ChangeWithoutActivationType.NotSet;
		}
		if (temporaryLicense.HasCurrentYearToken(_licenseDocument.ActivatedDate))
		{
			return ChangeWithoutActivationType.CurrentYear;
		}
		return ChangeWithoutActivationType.PreviousYear;
	}

	private ChangeWithoutActivationType IsDeviceInChangesWithoutActivation(IHardwareInfo hardwareInfo, IDeviceInfo deviceInfo)
	{
		TemporaryDeviceLicense temporaryDeviceLicense = _temporaryLicenseStorage.FindDeviceLicense(hardwareInfo.MacAddress, deviceInfo.UniqueIdentifier);
		if (temporaryDeviceLicense == null || !temporaryDeviceLicense.IsChangedWithoutActivation())
		{
			return ChangeWithoutActivationType.NotSet;
		}
		if (temporaryDeviceLicense.HasCurrentYearToken(_licenseDocument.ActivatedDate))
		{
			return ChangeWithoutActivationType.CurrentYear;
		}
		return ChangeWithoutActivationType.PreviousYear;
	}

	private DateTime EnsureDateDoesNotExceedGracePeriod(DateTime dateTime)
	{
		if (dateTime > DateTime.UtcNow.AddDays(_licenseDocument.HardwareGracePeriod + 1))
		{
			return DateTime.UtcNow.AddDays(_licenseDocument.HardwareGracePeriod);
		}
		return dateTime;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void EnsureHardwareIsLicensedOnSite(IHardwareInfo hardwareInfo, RecorderLicense recorderLicense, bool isFreeLicense)
	{
		if (!_licenseCountManager.IsHardwareLicensedOnSite(hardwareInfo.MacAddress))
		{
			_licenseCountManager.AddLicensedHardware(hardwareInfo, recorderLicense.RecorderId, isFreeLicense);
		}
	}

	private void RemoveDeviceFromChangesWithoutActivation(IDeviceInfo deviceInfo)
	{
		_temporaryLicenseStorage.RemoveDeviceLicense(deviceInfo.HardwareInfo.MacAddress, deviceInfo.UniqueIdentifier);
		deviceInfo.LicenseState = DeviceInfoLicenseState.Unknown;
	}
}
