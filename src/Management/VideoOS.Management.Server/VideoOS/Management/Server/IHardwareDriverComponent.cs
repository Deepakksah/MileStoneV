using System;

namespace VideoOS.Management.Server;

public interface IHardwareDriverComponent : IDisposable
{
	HardwareDriver[] QueryActiveHardwareDrivers(string recordingServerId);

	HardwareDriver QueryActiveHardwareDriver(string recordingServerId, string driverId);

	HardwareDriver[] QueryFailoverServerHardwareDrivers(string failoverServerId);

	HardwareDriver[] QueryRecorderHardwareDrivers(string recordingServerId, bool usedOnly);
}
