namespace VideoOS.IdentityServer.Areas.Identity.Pages;

public static class AreaConstants
{
	public class FullPagePaths
	{
		public const string Login = "/Account/Login/Index";

		public const string ChangePassword = "/Account/Manage/ChangePassword";

		public const string PasswordChanged = "/Account/Manage/PasswordChanged";

		public const string LoggedOut = "/Account/Logout/LoggedOut";

		public const string Error = "/Error";
	}

	public const string IdentityArea = "Identity";

	public const string IdentityLoginPath = "/Identity/Account/Login";

	public const string IdentityLogoutPath = "/Identity/Account/Logout/Index";

	public const string ChangePasswordAuthenticationScheme = "IDP.ForcePasswordChange";
}
