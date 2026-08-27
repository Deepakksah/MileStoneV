using System;

namespace VideoOS.Platform.Messaging;

public class ConnectionStateChangedEventArgs : EventArgs
{
	public ServerId ServerId { get; set; }

	public Uri Uri { get; set; }

	public Guid ServiceType { get; set; }

	public ConnectionStateChangedEventArgs(ServerId serverId, Uri uri, Guid serviceType)
	{
		ServerId = serverId;
		Uri = uri;
		ServiceType = serviceType;
	}
}
