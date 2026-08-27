using System;
using VideoOS.Management.Common;
using VideoOS.Server.DataComponents.ConfigurationObjects;

namespace VideoOS.Server.Failover;

public sealed class FailoverStatusChangedEventArgs : EventArgs
{
	public FailoverServer FailoverServer { get; private set; }

	public FailoverStatusDto CurrentStatus { get; private set; }

	public FailoverStatusDto PreviousStatus { get; private set; }

	public FailoverStatusChangedEventArgs(FailoverServer failoverServer, FailoverStatusDto currentStatus, FailoverStatusDto previousStatus)
	{
		FailoverServer = failoverServer;
		CurrentStatus = currentStatus;
		PreviousStatus = previousStatus;
	}
}
