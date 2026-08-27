using System;

namespace VideoOS.Server.DataComponents.ConfigurationObjects;

public interface IDriverConfigDataComponent : IDisposable
{
	DriverConfigData QueryByRecordingServerServiceAndNumber(Guid recordingServerServiceId, int driverNumber);
}
