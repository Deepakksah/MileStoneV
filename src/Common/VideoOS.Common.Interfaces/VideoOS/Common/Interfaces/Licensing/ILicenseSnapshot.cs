using System;
using System.Collections.Generic;

namespace VideoOS.Common.Interfaces.Licensing;

public interface ILicenseSnapshot
{
	IEnumerable<IRecorderInfo> Recorders { get; }

	IEnumerable<IRecorderLicenseInfo> RecorderLicensesWithAnyChange { get; }

	IEnumerable<IHardwareLicenseInfo> HardwareLicensesWithAnyChange { get; }

	IEnumerable<IDeviceLicenseInfo> DeviceLicensesWithAnyChange { get; }

	bool LicenseUpdated { get; }

	int HardwareLicenseCount { get; }

	IRecorderInfo GetRecorder(Guid recorderId);

	IEnumerable<IHardwareInfo> FindHardwareInGrace(Guid excludedRecorderId, bool isIcp);

	IHardwareInfo FindHardwareByMacAddress(string macAddress);

	IEnumerable<IRecorderLicenseInfo> RecorderLicensesByState(LicenseUpdateAction state);

	IEnumerable<IHardwareLicenseInfo> HardwareLicensesByState(LicenseUpdateAction state);

	IEnumerable<IDeviceLicenseInfo> DeviceLicensesByState(LicenseUpdateAction state);

	void SaveChanges();

	bool AddUnlicensedHardware(IHardwareInfo hardwareInfo);

	bool AddUnlicensedDevice(IDeviceInfo deviceInfo);

	void AddRecorderLicense(IRecorderLicenseInfo recorderLicenseInfo);

	IRecorderLicenseInfo GetRecorderLicense(Guid recorderId);

	void RemoveRecorderLicense(Guid recorderId);

	int RecorderLicenseCount();

	void AddHardwareLicense(IHardwareLicenseInfo hardwareLicenseInfo);

	IHardwareLicenseInfo GetHardwareLicense(IHardwareInfo hardwareInfo);

	void SetHardwareLicenseExpire(IHardwareInfo hardwareInfo, DateTime expireDate);

	void RemoveHardwareLicense(IHardwareInfo hardwareInfo);

	DateTime GetHardwareLicenseExpireDate(IHardwareInfo hardwareInfo);

	void AddDeviceLicense(IDeviceLicenseInfo deviceLicenseInfo);

	IDeviceLicenseInfo GetDeviceLicense(IDeviceInfo deviceInfo);

	DateTime GetDeviceLicenseExpireDate(IDeviceInfo deviceInfo);

	void SetDeviceLicenseExpire(IDeviceInfo deviceInfo, DateTime expireDate);

	void RemoveDeviceLicense(IDeviceInfo deviceInfo);

	void ResetUpdateStatuses();
}
