namespace VideoOS.Mobile.SDK.Portable.Server.Base;

public enum ConnectionStates
{
	Disconnected,
	Connecting,
	Connected,
	LoggingIn,
	SecondStepVerificationRequired,
	LoggedIn,
	Disconnecting
}
