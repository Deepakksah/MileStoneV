namespace VideoOS.Platform.Data.Speaker;

public enum ClientState
{
	ErrorDetected = -1,
	Disconnected,
	Connecting,
	Connected,
	Initializing,
	Initialized,
	BeginningSpeak,
	Speaking,
	EndingSpeak,
	UnInitializing
}
