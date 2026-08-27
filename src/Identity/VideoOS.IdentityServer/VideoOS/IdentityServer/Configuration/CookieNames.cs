namespace VideoOS.IdentityServer.Configuration;

public static class CookieNames
{
	private const string Prefix = ".IDP.";

	public const string Application = ".IDP.Application";

	public const string TempDataProvider = ".IDP.TempDataProvider";

	public const string Session = ".IDP.Session";

	public const string Antiforgery = ".IDP.XSRF-CSRF";

	public const string Culture = ".IDP.Culture";

	public const string ForceChangePassword = ".IDP.ForceChangePassword";
}
