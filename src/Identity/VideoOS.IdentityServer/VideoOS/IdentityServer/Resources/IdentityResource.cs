using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace VideoOS.IdentityServer.Resources;

[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
[DebuggerNonUserCode]
[CompilerGenerated]
public class IdentityResource
{
	private static ResourceManager resourceMan;

	private static CultureInfo resourceCulture;

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static ResourceManager ResourceManager
	{
		get
		{
			if (resourceMan == null)
			{
				ResourceManager resourceManager = new ResourceManager("VideoOS.IdentityServer.Resources.IdentityResource", typeof(IdentityResource).Assembly);
				resourceMan = resourceManager;
			}
			return resourceMan;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static CultureInfo Culture
	{
		get
		{
			return resourceCulture;
		}
		set
		{
			resourceCulture = value;
		}
	}

	public static string ButtonCancel => ResourceManager.GetString("ButtonCancel", resourceCulture);

	public static string ButtonContinueToLogin => ResourceManager.GetString("ButtonContinueToLogin", resourceCulture);

	public static string ButtonContinueWith => ResourceManager.GetString("ButtonContinueWith", resourceCulture);

	public static string ButtonNext => ResourceManager.GetString("ButtonNext", resourceCulture);

	public static string ButtonReturnTo => ResourceManager.GetString("ButtonReturnTo", resourceCulture);

	public static string ButtonSave => ResourceManager.GetString("ButtonSave", resourceCulture);

	public static string ButtonSignin => ResourceManager.GetString("ButtonSignin", resourceCulture);

	public static string ButtonYes => ResourceManager.GetString("ButtonYes", resourceCulture);

	public static string CancelLoginSessionMessage => ResourceManager.GetString("CancelLoginSessionMessage", resourceCulture);

	public static string ChangePasswordPageTitle => ResourceManager.GetString("ChangePasswordPageTitle", resourceCulture);

	public static string ChangePasswordTitle => ResourceManager.GetString("ChangePasswordTitle", resourceCulture);

	public static string Clarification => ResourceManager.GetString("Clarification", resourceCulture);

	public static string ClientInitiatedLoginPageTitle => ResourceManager.GetString("ClientInitiatedLoginPageTitle", resourceCulture);

	public static string CloseWindow => ResourceManager.GetString("CloseWindow", resourceCulture);

	public static string ConfirmLogoutTitle => ResourceManager.GetString("ConfirmLogoutTitle", resourceCulture);

	public static string ConfirmPassword => ResourceManager.GetString("ConfirmPassword", resourceCulture);

	public static string CurrentCredentialsSubtitle => ResourceManager.GetString("CurrentCredentialsSubtitle", resourceCulture);

	public static string CurrentCredentialsTitle => ResourceManager.GetString("CurrentCredentialsTitle", resourceCulture);

	public static string CurrentPassword => ResourceManager.GetString("CurrentPassword", resourceCulture);

	public static string DirectLoginPageTitle => ResourceManager.GetString("DirectLoginPageTitle", resourceCulture);

	public static string ErrorDontMatchPasswords => ResourceManager.GetString("ErrorDontMatchPasswords", resourceCulture);

	public static string ErrorEmptyField => ResourceManager.GetString("ErrorEmptyField", resourceCulture);

	public static string ErrorEmptyPassword => ResourceManager.GetString("ErrorEmptyPassword", resourceCulture);

	public static string ErrorLockAccount => ResourceManager.GetString("ErrorLockAccount", resourceCulture);

	public static string ErrorStrongPassword => ResourceManager.GetString("ErrorStrongPassword", resourceCulture);

	public static string ErrorTheSamePasswords => ResourceManager.GetString("ErrorTheSamePasswords", resourceCulture);

	public static string ErrorWrongField => ResourceManager.GetString("ErrorWrongField", resourceCulture);

	public static string ExpiredLoginSessionMessage => ResourceManager.GetString("ExpiredLoginSessionMessage", resourceCulture);

	public static string LoggedOutTitle => ResourceManager.GetString("LoggedOutTitle", resourceCulture);

	public static string LoginTitle => ResourceManager.GetString("LoginTitle", resourceCulture);

	public static string LogoutConfirmation => ResourceManager.GetString("LogoutConfirmation", resourceCulture);

	public static string ManageAccount => ResourceManager.GetString("ManageAccount", resourceCulture);

	public static string MessageLoggedOut => ResourceManager.GetString("MessageLoggedOut", resourceCulture);

	public static string NewPassword => ResourceManager.GetString("NewPassword", resourceCulture);

	public static string PageForbidden => ResourceManager.GetString("PageForbidden", resourceCulture);

	public static string PageNotFound => ResourceManager.GetString("PageNotFound", resourceCulture);

	public static string Password => ResourceManager.GetString("Password", resourceCulture);

	public static string PasswordChangedPageTitle => ResourceManager.GetString("PasswordChangedPageTitle", resourceCulture);

	public static string PasswordChangedTitle => ResourceManager.GetString("PasswordChangedTitle", resourceCulture);

	public static string PasswordInfo => ResourceManager.GetString("PasswordInfo", resourceCulture);

	public static string PasswordMismatch => ResourceManager.GetString("PasswordMismatch", resourceCulture);

	public static string PasswordRequireementMaxLength => ResourceManager.GetString("PasswordRequireementMaxLength", resourceCulture);

	public static string PasswordRequirementDigits => ResourceManager.GetString("PasswordRequirementDigits", resourceCulture);

	public static string PasswordRequirementLowercaseLetters => ResourceManager.GetString("PasswordRequirementLowercaseLetters", resourceCulture);

	public static string PasswordRequirementMinimumUnique => ResourceManager.GetString("PasswordRequirementMinimumUnique", resourceCulture);

	public static string PasswordRequirementMinPasswordLength => ResourceManager.GetString("PasswordRequirementMinPasswordLength", resourceCulture);

	public static string PasswordRequirementSpecialCharacters => ResourceManager.GetString("PasswordRequirementSpecialCharacters", resourceCulture);

	public static string PasswordRequirementUppercaseLetters => ResourceManager.GetString("PasswordRequirementUppercaseLetters", resourceCulture);

	public static string SetPasswordTitle => ResourceManager.GetString("SetPasswordTitle", resourceCulture);

	public static string SuccessAuthentication => ResourceManager.GetString("SuccessAuthentication", resourceCulture);

	public static string TitleAuthentication => ResourceManager.GetString("TitleAuthentication", resourceCulture);

	public static string UnexpectedError => ResourceManager.GetString("UnexpectedError", resourceCulture);

	public static string UnexpectedErrorFooter => ResourceManager.GetString("UnexpectedErrorFooter", resourceCulture);

	public static string UserName => ResourceManager.GetString("UserName", resourceCulture);

	public IdentityResource()
	{
	}
}
