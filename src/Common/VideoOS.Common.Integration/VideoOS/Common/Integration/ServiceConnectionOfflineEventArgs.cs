using System;

namespace VideoOS.Common.Integration;

public class ServiceConnectionOfflineEventArgs : EventArgs
{
	public Guid ServiceId { get; private set; }

	public bool Timeout { get; private set; }

	public ServiceConnectionOfflineEventArgs(Guid serviceId, bool timeout)
	{
		ServiceId = serviceId;
		Timeout = timeout;
	}
}
