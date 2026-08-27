namespace VideoOS.Platform.Data;

public enum ClientErrorCode
{
	Ok = 0,
	ConnectionLost = 1,
	WrongState = 2,
	AuthenticationFailed = 3,
	InternalError = 4,
	Max = 99
}
