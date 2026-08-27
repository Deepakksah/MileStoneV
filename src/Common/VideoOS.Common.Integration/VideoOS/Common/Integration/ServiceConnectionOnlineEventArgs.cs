using System;

namespace VideoOS.Common.Integration;

public class ServiceConnectionOnlineEventArgs : EventArgs
{
	public Guid ServiceId { get; private set; }

	public ServiceConnectionOnlineEventArgs(Guid serviceId)
	{
		ServiceId = serviceId;
	}
}
