using System;

namespace VideoOS.Common.Integration;

public class ServerCommunicationBrokenEventArgs : EventArgs
{
	public ServerCommunicationBrokenReason Reason { get; private set; }

	public Guid ServiceId { get; private set; }

	public ServerCommunicationBrokenEventArgs(ServerCommunicationBrokenReason reason, Guid serviceId)
	{
		Reason = reason;
		ServiceId = serviceId;
	}
}
