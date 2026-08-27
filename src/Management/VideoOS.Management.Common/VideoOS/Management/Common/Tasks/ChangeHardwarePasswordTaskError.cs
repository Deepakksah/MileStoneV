namespace VideoOS.Management.Common.Tasks;

public static class ChangeHardwarePasswordTaskError
{
	public const int PasswordLengthTooBigError = 66000;

	public const int PasswordLengthTooSmallError = 66001;

	public const int PasswordContainsInvalidCharacter = 66002;

	public const int PasswordChangeUnsupported = 66003;

	public const int PasswordChangeGenericHardwareError = 66004;

	public const int PasswordChangeHardwareNotStarted = 66005;

	public const int PasswordChangeHardwareNotEnabled = 66006;

	public const int PasswordChangeHardwareBlocked = 66007;

	public const int PasswordChangeHardwareReadOnly = 66008;

	public const int CredentialsUpdateError = 66009;

	public const int PasswordChangeRecorderUnavailable = 66010;

	public const int PasswordChangeRecorderIncompatible = 66011;

	public const int PasswordChangeOperationTimedOut = 66012;

	public const int PasswordChangeOtherError = 66013;

	public const int PasswordChangeNetworkIssue = 66014;

	public const int PasswordChangeNoHardwareAccess = 66016;

	public const int PasswordChangeUserLevelAnonymousNotAllowed = 66017;

	public const int PasswordChangeUserNameNotRecognized = 66018;

	public const int PasswordChangePasswordOrUserLevelMayNotBeChanged = 66019;

	public const int PasswordChangeUserAccessDenied = 66020;
}
