using System;
using System.Collections.Generic;

namespace VideoOS.Common.Interfaces.Licensing;

public interface IRecorderInfo
{
	Guid Id { get; }

	bool Enabled { get; }

	string DevicePackInfo { get; }

	IReadOnlyList<IHardwareInfo> Hardware { get; }

	void AddHardware(IHardwareInfo hardwareInfo);

	void ClearExistingLicenseInformation();
}
