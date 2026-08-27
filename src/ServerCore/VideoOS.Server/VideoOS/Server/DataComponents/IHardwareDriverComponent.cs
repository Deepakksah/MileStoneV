using System;

namespace VideoOS.Server.DataComponents;

public interface IHardwareDriverComponent : IDisposable
{
	HardwareDriverInfo[] QueryActiveMIPDrivers();
}
