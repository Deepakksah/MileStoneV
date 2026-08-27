using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace VideoOS.Common.Interfaces.Licensing;

public interface ILicenseCountManager
{
	Dictionary<Guid, DeviceInfoLicenseState> DeviceStatus { get; }

	int UsedChangesWithoutActivation { get; }

	void AddLicensedHardware(IHardwareInfo hardwareInfo, Guid recorderId, bool isFreeLicense);

	void UpdateHardwareLicenseValue(string mac, long licenseValue);

	long GetHardwareLicenseValue(string mac);

	bool IsHardwareLicensedOnSite(string mac);

	bool IsHardwareLicensedOnRecorder(string mac, Guid recorderId);

	bool IsHardwareWithoutEnabledCamerasLicensed(IHardwareInfo hardwareInfo);

	int CountOfLicensesUsed(bool isInterconnected);

	int GetHardwareUsedOnSite(bool icp);

	void AddLicensedCameraFeed(IRecorderInfo recorderInfo, IHardwareInfo hardwareInfo, IDeviceInfo deviceInfo, long licenseValue, int feedCount);

	void UpdateCameraFeedLicensedValue(IHardwareInfo hardwareInfo, IDeviceInfo deviceInfo, long licenseValue);

	long GetCameraFeedLicenseValue(IHardwareInfo hardwareInfo, IDeviceInfo deviceInfo);

	Guid GetFirstCameraFeedDeviceId(string mac);

	bool IsAnyCameraFeedLicensedForHardware(string mac);

	bool IsCameraFeedLicensedOnSite(IHardwareInfo hardwareInfo, IDeviceInfo deviceInfo);

	bool IsCameraFeedLicensedOnRecorder(IHardwareInfo hardwareInfo, IDeviceInfo deviceInfo, Guid recorderId);

	int GetCameraFeedsUsedOnSite(bool icp);

	int GetCameraFeedsUsedByRecorder(Guid recorderId, bool icp);

	int GetCameraFeedsUsedByHardware(string mac);

	bool IsHardwareInChangesWithoutActivation(string mac);

	void AddHardwareToChangesWithoutActivation(string mac);

	bool IsDeviceInChangesWithoutActivation(IHardwareInfo hardwareInfo, IDeviceInfo deviceInfo);

	void RemoveDevicesFromChangesWithoutActivation(Collection<IHardwareDeviceItem> hardwareDeviceItems);

	void RemoveDevicesFromChangesWithoutActivation(IHardwareInfo hardwareInfo);

	void RemoveDeviceFromChangesWithoutActivation(IHardwareDeviceItem hardwareDeviceItem);

	void AddDeviceToChangesWithoutActivation(IHardwareInfo hardwareInfo, IDeviceInfo deviceInfo);

	void RemoveHardwareFromChangesWithoutActivation(Collection<string> macAddresses);

	void RemoveHardwareFromChangesWithoutActivation(string macAddress);

	void RegisterHardwareRecorderRelation(Guid recorderId, string mac);

	IEnumerable<Guid> GetRelatedRecordingServers(string mac);

	void ClearByRecorder(Guid recorderId);

	void ClearChangesWithoutActivation(ITemporaryChangesWithoutActivation temporaryChangesWithoutActivation, DateTime licensedActivatedDate);

	Collection<string> FindNonIcpHardwareInGrace(Guid recorderId);

	Collection<string> FindActiveHardwareInChangesWithoutActivation(Guid recorderId);

	void ValidateRelatedRecordingServerLicenses(ILicenseManager licenseManager, ILicenseSnapshot licenseSnapshot, Guid currentRecorderId);
}
