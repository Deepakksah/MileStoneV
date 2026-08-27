using System;

namespace VideoOS.Server.DataComponents.ConfigurationObjects;

public sealed class DeviceEvidenceLockData
{
	public Guid IdEvidenceLock { get; set; }

	public Guid IdRecorder { get; set; }

	public Guid IdDevice { get; set; }
}
