using System;

namespace VideoOS.Common.Interfaces.Licensing;

public interface IDeviceInfo
{
	Guid Id { get; }

	Guid OriginalDeviceId { get; }

	string DeviceType { get; }

	bool Enabled { get; }

	DeviceInfoLicenseState LicenseState { get; set; }

	int DeviceIndex { get; }

	string UniqueIdentifier { get; }

	IHardwareInfo HardwareInfo { get; }
}
