using System;
using System.Diagnostics;

namespace VideoOS.Platform.Client.Hid;

public sealed class InstanceDiscoveryEventArgs : EventArgs
{
	public Guid InstanceId { get; }

	[DebuggerStepThrough]
	public InstanceDiscoveryEventArgs(Guid instanceId)
	{
		InstanceId = instanceId;
	}
}
