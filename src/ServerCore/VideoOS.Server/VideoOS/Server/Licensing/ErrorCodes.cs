namespace VideoOS.Server.Licensing;

public static class ErrorCodes
{
	public const string UserNameAlreadyUsed = "UsernameAllreadyUsed";

	public const string UserNameAndPasswordNotFound = "UsernameAndPasswordNotFound";

	public const string SlcNotFound = "SlcNotFound";

	public const string SlcRegisteredOnOtherLogOn = "SlcRegisteredOnOtherLogin";

	public const string MismatchOnComputerSid = "MismatchOnComputerSID";

	public const string TooManyCamerasRequested = "ToManyCamerasRequested";

	public const string TooManyHardwareDevicesRequested = "ToManyHardwareDevicesRequested";

	public const string TooManyInterconnectedCamerasRequested = "ToManyInterconnectedCamerasRequested";

	public const string TooManyRecordersRequested = "ToManyRecordersRequested";

	public const string FeatureNotRegistered = "FeatureNotRegistered";

	public const string FeatureAlreadyInUse = "FeatureAlreadyInUse";

	public const string DlkRemovalExceeded = "DlkRemovalExceeded";

	public const string ActivationLimitExceeded = "ActivationLimitExceeded";
}
