namespace VideoOS.Common.Interfaces.Recorder;

public enum PtzCallbackResult
{
	Undefined,
	Ok,
	DeviceNotReady,
	TokenInvalid,
	NotPermitted,
	SessionNotLocked
}
