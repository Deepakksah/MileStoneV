namespace VideoOS.Platform.Data;

public enum RestrictedMediaResponseCode
{
	UnknownError,
	None,
	Warning,
	Unauthorized,
	DeviceNotFound,
	RestrictedMediasNotSupported,
	AlreadyInRestrictedLiveMode,
	NotInRestrictedLiveMode
}
