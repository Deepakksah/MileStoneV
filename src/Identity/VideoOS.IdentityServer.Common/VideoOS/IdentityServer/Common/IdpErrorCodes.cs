namespace VideoOS.IdentityServer.Common;

public static class IdpErrorCodes
{
	public static class AuthenticationErrors
	{
		public const string ChangePassword = "ChangePassword";

		public const string LockedOut = "LockedOut";

		public const string InvalidCredentials = "InvalidCredentials";
	}

	public const string ConcurrencyFailure = "ConcurrencyFailure";

	public const string DefaultError = "DefaultError";

	public const string DuplicateEmail = "DuplicateEmail";

	public const string DuplicateUserName = "DuplicateUserName";

	public const string ErrorStrongPassword = "ErrorStrongPassword";

	public const string ErrorTheSamePasswords = "ErrorTheSamePasswords";

	public const string InvalidEmail = "InvalidEmail";

	public const string InvalidRoleName = "InvalidRoleName";

	public const string InvalidToken = "InvalidToken";

	public const string InvalidUserName = "InvalidUserName";

	public const string LoginAlreadyAssociated = "LoginAlreadyAssociated";

	public const string PasswordMismatch = "PasswordMismatch";

	public const string PasswordRequiresDigit = "PasswordRequiresDigit";

	public const string PasswordRequiresLower = "PasswordRequiresLower";

	public const string PasswordRequiresNonAlphanumeric = "PasswordRequiresNonAlphanumeric";

	public const string PasswordRequiresUniqueChars = "PasswordRequiresUniqueChars";

	public const string PasswordRequiresUpper = "PasswordRequiresUpper";

	public const string PasswordTooShort = "PasswordTooShort";

	public const string RecoveryCodeRedemptionFailed = "RecoveryCodeRedemptionFailed";

	public const string UserAlreadyHasPassword = "UserAlreadyHasPassword";

	public const string UserLockoutNotEnabled = "UserLockoutNotEnabled";

	public const string NotAllowed = "NotAllowed";

	public const string UserNotFound = "UserNotFound";

	public const string ArgumentsNotResolved = "ArgumentsNotResolved";

	public const string DuplicateAdminIdentity = "DuplicateAdminIdentity";

	public const string AdminIdentityNotFound = "AdminIdentityNotFound";

	public const string DuplicateIssuer = "DuplicateIssuer";

	public const string DuplicateAddress = "DuplicateAddress";

	public const string LoginProviderNotFound = "LoginProviderNotFound";

	public const string ThereCanBeOnlyOne = "ThereCanBeOnlyOne";

	public const string DuplicateLoginProvider = "DuplicateLoginProvider";

	public const string DuplicateDisplayName = "DuplicateDisplayName";

	public const string DuplicateAuthority = "DuplicateAuthority";

	public const string InvalidScopes = "InvalidScopes";
}
