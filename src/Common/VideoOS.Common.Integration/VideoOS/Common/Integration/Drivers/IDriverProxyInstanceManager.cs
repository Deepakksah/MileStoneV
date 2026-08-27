using System;

namespace VideoOS.Common.Integration.Drivers;

public interface IDriverProxyInstanceManager
{
	string AddHardwareToProxy(Guid hardwareId, int streamCount, string proxyName);

	void RemoveHardwareFromProxy(Guid hardwareId, string proxyName);
}
