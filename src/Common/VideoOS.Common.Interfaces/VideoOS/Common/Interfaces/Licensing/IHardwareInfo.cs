using System;
using System.Collections.Generic;

namespace VideoOS.Common.Interfaces.Licensing;

public interface IHardwareInfo
{
	Guid Id { get; }

	Guid RecorderId { get; }

	string Name { get; }

	string MacAddress { get; }

	string MacAddressHashed { get; }

	string SerialNumber { get; }

	string DriverName { get; }

	string DriverNameUpper { get; }

	string DriverVersion { get; }

	int DriverNumber { get; }

	string DetectedModel { get; }

	string DetectedModelUpper { get; }

	bool IsInterconnectedProduct { get; }

	string LicenseMethod { get; set; }

	int LicenseCount { get; set; }

	HardwareWhitelistStatus WhitelistStatus { get; set; }

	int ChannelLicenseCount { get; set; }

	string VendorProductId { get; }

	bool Enabled { get; }

	IFeedDictionary Feeds { get; }

	IPortDictionary Ports { get; }

	List<IDeviceInfo> Devices { get; }

	HardwareLicenseState LicenseState { get; set; }

	List<IDeviceInfo> EnabledCameras { get; }

	List<IDeviceInfo> EnabledDevices { get; }

	List<IDeviceInfo> CameraDevices { get; }

	List<IDeviceInfo> EnabledNonCameraDevices { get; }

	bool HasEnabledCameras { get; }

	bool HasActivatedCameras { get; }

	bool HasCameraDevices { get; }

	bool HasLicensedCameras { get; }

	int[] EnabledCameraIndexes { get; }
}
