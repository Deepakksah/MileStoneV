using System;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration.Configuration;
using VideoOS.Common.Interfaces.Licensing;
using VideoOS.Licensing;

namespace VideoOS.Server.Licensing;

public sealed class LicenseAllocatorHardware : ILicenseAllocator
{
	private readonly ILicenseCountManager _licenseCountManager;

	private readonly ILicenseDocument _licenseDocument;

	private readonly ITemporaryLicenseStorage _temporaryLicenseStorage;

	private readonly IServerEnvironment _serverEnvironment;

	private int _totalAllowedChangesWithoutActivation;

	public LicenseAllocatorHardware(ILicenseCountManager licenseCountManager, ILicenseDocument licenseDocument, ITemporaryLicenseStorage temporaryLicenseStorage, IServerEnvironment serverEnvironment, int allowedChangesWithoutActivation)
	{
		if (licenseCountManager == null)
		{
			throw new ArgumentNullException("licenseCountManager");
		}
		if (licenseDocument == null)
		{
			throw new ArgumentNullException("licenseDocument");
		}
		if (temporaryLicenseStorage == null)
		{
			throw new ArgumentNullException("temporaryLicenseStorage");
		}
		if (serverEnvironment == null)
		{
			throw new ArgumentNullException("serverEnvironment");
		}
		if (allowedChangesWithoutActivation < 0)
		{
			throw new ArgumentOutOfRangeException("allowedChangesWithoutActivation");
		}
		_licenseCountManager = licenseCountManager;
		_licenseDocument = licenseDocument;
		_temporaryLicenseStorage = temporaryLicenseStorage;
		_serverEnvironment = serverEnvironment;
		_totalAllowedChangesWithoutActivation = allowedChangesWithoutActivation;
	}

	public void ResetChangesWithoutActivation(int changesWithoutActivation)
	{
		_totalAllowedChangesWithoutActivation = changesWithoutActivation;
	}

	public void AllocateLicenses(IHardwareInfo hardwareInfo, RecorderLicense recorderLicense, ILicenseSnapshot licenseSnapshot, bool acquireNewLicenses, bool ignoreAlreadyLicensed)
	{
		AcquireHardwareLicense(hardwareInfo, recorderLicense, licenseSnapshot, acquireNewLicenses, ignoreAlreadyLicensed);
		AcquireFeedsForHardware(hardwareInfo, recorderLicense, licenseSnapshot, acquireNewLicenses, ignoreAlreadyLicensed);
	}

	private void AcquireHardwareLicense(IHardwareInfo hardwareInfo, RecorderLicense recorderLicense, ILicenseSnapshot licenseSnapshot, bool acquireNewLicenses, bool ignoreAlreadyLicensed)
	{
		int usedChangesWithoutActivation = _licenseCountManager.UsedChangesWithoutActivation;
		int totalAllowedChangesWithoutActivation = _totalAllowedChangesWithoutActivation;
		if (!hardwareInfo.Enabled)
		{
			licenseSnapshot.RemoveHardwareLicense(hardwareInfo);
			HandleChangesWithoutActivationForDisabledHardware(hardwareInfo);
			return;
		}
		long ticks = licenseSnapshot.GetHardwareLicenseExpireDate(hardwareInfo).Ticks;
		bool flag = ticks > 0;
		if ((!acquireNewLicenses && !flag) || (ignoreAlreadyLicensed && flag))
		{
			return;
		}
		if (!_licenseCountManager.IsHardwareLicensedOnSite(hardwareInfo.MacAddress))
		{
			int hardwareUsedOnSite = _licenseCountManager.GetHardwareUsedOnSite(hardwareInfo.IsInterconnectedProduct);
			int num = (hardwareInfo.IsInterconnectedProduct ? int.MaxValue : _licenseDocument.HardwareLicenseCount);
			if (hardwareUsedOnSite + 1 > num)
			{
				licenseSnapshot.RemoveHardwareLicense(hardwareInfo);
				return;
			}
			_licenseCountManager.AddLicensedHardware(hardwareInfo, recorderLicense.RecorderId, isFreeLicense: false);
		}
		if (_licenseDocument.IsMacAddressLicensed(hardwareInfo.MacAddress))
		{
			recorderLicense.AddHardwareLicense(hardwareInfo.Id, new PermanentLicense(hardwareInfo.MacAddress, _licenseDocument.GetDeviceLicenseKey(hardwareInfo.MacAddress)));
			_temporaryLicenseStorage.RemoveHardwareLicense(hardwareInfo.MacAddress);
			licenseSnapshot.SetHardwareLicenseExpire(hardwareInfo, new DateTime(DateTime.MaxValue.Ticks, DateTimeKind.Utc));
			_licenseCountManager.UpdateHardwareLicenseValue(hardwareInfo.MacAddress, DateTime.MaxValue.Ticks);
			return;
		}
		string text = MacAddressUtilities.CreateMacAddressHashString(hardwareInfo.MacAddress);
		string text2 = MacAddressUtilities.CreateMacAddressHashString(MacAddressUtilities.EmptyMacAddress);
		if (text2 == text)
		{
			_serverEnvironment.Server.Logger.LogError("Mac address must have the format 'NN:NN:NN:NN:NN:NN' where NN is a hex number and ':' is an optional separator character (any char). MAC Address: {MacAddress} Device Name: {DeviceName}", hardwareInfo.MacAddress, hardwareInfo.Name);
			licenseSnapshot.RemoveHardwareLicense(hardwareInfo);
			return;
		}
		bool flag2 = usedChangesWithoutActivation < totalAllowedChangesWithoutActivation;
		TemporaryLicense temporaryLicense = _temporaryLicenseStorage.FindLicense(hardwareInfo.MacAddress);
		if (temporaryLicense == null)
		{
			temporaryLicense = new TemporaryLicense(hardwareInfo.MacAddress, DateTime.UtcNow.AddDays(_licenseDocument.HardwareGracePeriod), null);
			if (flag2 && !hardwareInfo.IsInterconnectedProduct)
			{
				temporaryLicense.SetChangesWithoutActivationToken(_licenseDocument.ActivatedDate);
				_licenseCountManager.AddHardwareToChangesWithoutActivation(hardwareInfo.MacAddress);
			}
			_temporaryLicenseStorage.AddLicense(temporaryLicense);
			recorderLicense.AddHardwareLicense(hardwareInfo.Id, temporaryLicense);
			licenseSnapshot.SetHardwareLicenseExpire(hardwareInfo, temporaryLicense.ExpiryDateUtc);
			return;
		}
		if (!hardwareInfo.IsInterconnectedProduct)
		{
			if (flag2 && !temporaryLicense.IsChangedWithoutActivation())
			{
				temporaryLicense.SetChangesWithoutActivationToken(_licenseDocument.ActivatedDate);
			}
			else if (usedChangesWithoutActivation > totalAllowedChangesWithoutActivation && temporaryLicense.IsChangedWithoutActivation() && temporaryLicense.HasCurrentYearToken(_licenseDocument.ActivatedDate))
			{
				temporaryLicense.ClearChangesWithoutActivationToken();
				_licenseCountManager.RemoveHardwareFromChangesWithoutActivation(hardwareInfo.MacAddress);
			}
		}
		if (!temporaryLicense.IsExpired() || temporaryLicense.IsChangedWithoutActivation())
		{
			recorderLicense.AddHardwareLicense(hardwareInfo.Id, temporaryLicense);
		}
		if (temporaryLicense.IsChangedWithoutActivation() && temporaryLicense.HasCurrentYearToken(_licenseDocument.ActivatedDate))
		{
			_licenseCountManager.AddHardwareToChangesWithoutActivation(hardwareInfo.MacAddress);
		}
		licenseSnapshot.SetHardwareLicenseExpire(hardwareInfo, temporaryLicense.ExpiryDateUtc);
		_licenseCountManager.UpdateHardwareLicenseValue(hardwareInfo.MacAddress, temporaryLicense.ExpiryDateUtc.Ticks);
	}

	private void AcquireFeedsForHardware(IHardwareInfo hardware, RecorderLicense recorderLicense, ILicenseSnapshot licenseSnapshot, bool acquireNewLicenses, bool ignoreAlreadyLicensed)
	{
		if (!hardware.Enabled)
		{
			return;
		}
		foreach (IDeviceInfo device in hardware.Devices)
		{
			if (!device.Enabled)
			{
				SetDeviceNotLicensed(device, licenseSnapshot);
				continue;
			}
			bool flag = licenseSnapshot.GetDeviceLicenseExpireDate(device).Ticks > 0;
			if ((acquireNewLicenses || flag) && !(ignoreAlreadyLicensed && flag))
			{
				AcquireFeedsForHardware_ProcessDevice(hardware, device, recorderLicense, licenseSnapshot);
			}
		}
	}

	private void AcquireFeedsForHardware_ProcessDevice(IHardwareInfo hardwareInfo, IDeviceInfo deviceInfo, RecorderLicense recorderLicense, ILicenseSnapshot licenseSnapshot)
	{
		if (!RegisteredDeviceType.IsCamera(deviceInfo.DeviceType))
		{
			recorderLicense.AddDeviceLicense(hardwareInfo.Id, deviceInfo.Id, "ffac8a74-85c4-4ce9-8f99-8ce7a790387d", DateTime.MaxValue.Ticks);
			licenseSnapshot.SetDeviceLicenseExpire(deviceInfo, new DateTime(DateTime.MaxValue.Ticks, DateTimeKind.Utc));
			return;
		}
		if (!hardwareInfo.IsInterconnectedProduct)
		{
			SetDevicePermanentLicensed(deviceInfo, hardwareInfo, licenseSnapshot, recorderLicense);
			return;
		}
		int num = 1;
		if (_licenseCountManager.IsCameraFeedLicensedOnSite(hardwareInfo, deviceInfo))
		{
			SetCameraDeviceLicensedAsDuplicate(deviceInfo, hardwareInfo, licenseSnapshot, recorderLicense);
			return;
		}
		int cameraFeedsUsedOnSite = _licenseCountManager.GetCameraFeedsUsedOnSite(hardwareInfo.IsInterconnectedProduct);
		int feedLicenses = _licenseDocument.GetFeedLicenses("Camera", hardwareInfo.IsInterconnectedProduct);
		if (cameraFeedsUsedOnSite + num > feedLicenses)
		{
			SetDeviceNotLicensed(deviceInfo, licenseSnapshot);
			return;
		}
		int hardwareCamerasCount = _licenseDocument.GetHardwareCamerasCount(hardwareInfo.MacAddress);
		int cameraFeedsUsedByHardware = _licenseCountManager.GetCameraFeedsUsedByHardware(hardwareInfo.MacAddress);
		IRecorderInfo recorder = licenseSnapshot.GetRecorder(recorderLicense.RecorderId);
		if (hardwareCamerasCount > cameraFeedsUsedByHardware)
		{
			SetDevicePermanentLicensed(deviceInfo, hardwareInfo, licenseSnapshot, recorderLicense);
			_licenseCountManager.AddLicensedCameraFeed(recorder, hardwareInfo, deviceInfo, DateTime.MaxValue.Ticks, num);
			return;
		}
		TemporaryDeviceLicense temporaryDeviceLicense = _temporaryLicenseStorage.FindDeviceLicense(hardwareInfo.MacAddress, deviceInfo.UniqueIdentifier);
		if (temporaryDeviceLicense == null)
		{
			temporaryDeviceLicense = new TemporaryDeviceLicense(hardwareInfo.MacAddress, deviceInfo.UniqueIdentifier, DateTime.UtcNow.AddDays(_licenseDocument.HardwareGracePeriod), string.Empty);
			_temporaryLicenseStorage.AddDeviceLicense(temporaryDeviceLicense);
		}
		recorderLicense.AddDeviceLicense(hardwareInfo.Id, deviceInfo.Id, "ffac8a74-85c4-4ce9-8099-8ce7a790387d", temporaryDeviceLicense.ExpiryDateUtc.Ticks);
		licenseSnapshot.SetDeviceLicenseExpire(deviceInfo, temporaryDeviceLicense.ExpiryDateUtc);
		_licenseCountManager.AddLicensedCameraFeed(recorder, hardwareInfo, deviceInfo, temporaryDeviceLicense.ExpiryDateUtc.Ticks, num);
	}

	private void HandleChangesWithoutActivationForDisabledHardware(IHardwareInfo hardware)
	{
		TemporaryLicense temporaryLicense = _temporaryLicenseStorage.FindLicense(hardware.MacAddress);
		if (temporaryLicense != null)
		{
			int usedChangesWithoutActivation = _licenseCountManager.UsedChangesWithoutActivation;
			int totalAllowedChangesWithoutActivation = _totalAllowedChangesWithoutActivation;
			bool flag = usedChangesWithoutActivation < totalAllowedChangesWithoutActivation;
			bool flag2 = temporaryLicense.IsChangedWithoutActivation() && temporaryLicense.HasCurrentYearToken(_licenseDocument.ActivatedDate);
			if (flag2 && flag && !hardware.IsInterconnectedProduct)
			{
				_licenseCountManager.AddHardwareToChangesWithoutActivation(hardware.MacAddress);
			}
		}
	}

	private static void SetDeviceNotLicensed(IDeviceInfo deviceInfo, ILicenseSnapshot licenseSnapshot)
	{
		licenseSnapshot.RemoveDeviceLicense(deviceInfo);
	}

	private void SetDevicePermanentLicensed(IDeviceInfo deviceInfo, IHardwareInfo hardware, ILicenseSnapshot licenseSnapshot, RecorderLicense recorderLicense)
	{
		_temporaryLicenseStorage.RemoveDeviceLicense(hardware.MacAddress, deviceInfo.UniqueIdentifier);
		recorderLicense.AddDeviceLicense(hardware.Id, deviceInfo.Id, "ffac8a74-85c4-4ce9-8f99-8ce7a790387d", DateTime.MaxValue.Ticks);
		licenseSnapshot.SetDeviceLicenseExpire(deviceInfo, new DateTime(DateTime.MaxValue.Ticks, DateTimeKind.Utc));
	}

	private void SetCameraDeviceLicensedAsDuplicate(IDeviceInfo deviceInfo, IHardwareInfo hardware, ILicenseSnapshot licenseSnapshot, RecorderLicense recorderLicense)
	{
		TemporaryDeviceLicense temporaryDeviceLicense = _temporaryLicenseStorage.FindDeviceLicense(hardware.MacAddress, deviceInfo.UniqueIdentifier);
		long cameraFeedLicenseValue = _licenseCountManager.GetCameraFeedLicenseValue(hardware, deviceInfo);
		if (temporaryDeviceLicense != null)
		{
			recorderLicense.AddDeviceLicense(hardware.Id, deviceInfo.Id, "ffac8a74-85c4-4ce9-8099-8ce7a790387d", temporaryDeviceLicense.ExpiryDateUtc.Ticks);
			licenseSnapshot.SetDeviceLicenseExpire(deviceInfo, temporaryDeviceLicense.ExpiryDateUtc);
		}
		else
		{
			SetDevicePermanentLicensed(deviceInfo, hardware, licenseSnapshot, recorderLicense);
		}
	}
}
