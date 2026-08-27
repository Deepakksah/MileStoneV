namespace VideoOS.Platform.Messaging;

public enum ClientState
{
	Connecting,
	ReconnectingWithSession,
	ReconnectingWithoutSession,
	Closing,
	Connected,
	Closed
}
