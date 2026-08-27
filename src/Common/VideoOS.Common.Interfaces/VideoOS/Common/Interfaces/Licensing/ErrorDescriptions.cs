namespace VideoOS.Common.Interfaces.Licensing;

public static class ErrorDescriptions
{
	public const string AccessDenied = "Access denied.";

	public const string MismatchOnSid = "The license has been activated on multiple systems.";

	public const string SlcRegisteredOnOtherLogOn = "The specified user is not allowed to activate this system.";

	public const string SlcNotFound = "The SLC was not registered.";

	public const string UserNameAndPasswordNotFound = "The specified user name or password was not correct.";

	public const string LicenseNotAllowed = "The requested license could not be granted.";

	public const string LimitExceeded = "Too much hardware has been replaced.";

	public const string FeatureAlreadyInUse = "The SLC for an installed product was already registered on another system.";

	public const string FeatureNotRegistered = "The SLC for an installed product was not registered.";

	public const string TooManyRecordersRequested = "Too many recording server licenses requested.";

	public const string TooManyInterconnectedCameras = "Too many licenses for {0} camera feeds requested.";

	public const string TooManyHardwareDevices = "Too many licenses for hardware requested.";

	public const string TooManyCamerasRequested = "Too many licenses for camera feeds requested.";

	public const string InternalServerError = "Internal server error.";

	public const string RequestFormatInvalid = "The format of the activation request was invalid.";

	public const string GenericError = "Could not obtain a new license.";

	public const string CustomerIsNotCdsValidated = "Cannot activate your licenses. You need to register your SLC first.";
}
