using System;

namespace VideoOS.Server.RecorderCommunication;

public sealed class ConnectionChangedEventArgs : EventArgs
{
	public Guid ServiceId { get; private set; }

	public string Uri { get; private set; }

	public ConnectionState ConnectionState { get; private set; }

	public string ExceptionMessage { get; private set; }

	public ConnectionChangedEventArgs(Guid serviceId, ConnectionState connectionState)
		: this(serviceId, connectionState, null)
	{
	}

	public ConnectionChangedEventArgs(Guid serviceId, ConnectionState connectionState, string exceptionMessage)
	{
		ServiceId = serviceId;
		ConnectionState = connectionState;
		ExceptionMessage = exceptionMessage;
	}

	public ConnectionChangedEventArgs(string uri, ConnectionState connectionState)
		: this(uri, connectionState, null)
	{
	}

	public ConnectionChangedEventArgs(string uri, ConnectionState connectionState, string exceptionMessage)
	{
		Uri = uri;
		ConnectionState = connectionState;
		ExceptionMessage = exceptionMessage;
	}
}
