namespace VideoOS.WebService.Common;

public enum ServerAuthentication
{
	Unknown,
	LegacyBasic,
	LegacyWindowsCurrentUser,
	LegacyWindows,
	OAuthUnsecure,
	OAuthSecure
}
