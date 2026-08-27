namespace VideoOS.Management.Common.Tasks;

public static class ActivateLicenseTaskError
{
	public const int GeneralLicenseError = 64000;

	public const int LicenseExpired = 64003;

	public const int LicenseFileInvalid = 64004;

	public const int UserNameAndPasswordNotFound = 64006;

	public const int SlcNotFound = 64007;

	public const int SlcRegisteredOnOtherLogOn = 64008;

	public const int MismatchOnComputerSid = 64009;

	public const int AccessDenied = 64050;

	public const int TooManyCamerasRequested = 64051;

	public const int TooManyHardwareDevicesRequested = 64052;

	public const int TooManyInterconnectedCamerasRequested = 64053;

	public const int TooManyRecordersRequested = 64054;

	public const int SlcNotRegistered = 64055;

	public const int SlcAlreadyInUse = 64056;

	public const int ActivationLimitExceeded = 64057;

	public const int CommunicationError = 64058;

	public const int CustomerIsNotCdsValidated = 64059;
}
