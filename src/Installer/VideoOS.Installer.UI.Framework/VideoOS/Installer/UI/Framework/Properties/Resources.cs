using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace VideoOS.Installer.UI.Framework.Properties;

[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
[DebuggerNonUserCode]
[CompilerGenerated]
public class Resources
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
				resourceMan = new ResourceManager("VideoOS.Installer.UI.Framework.Properties.Resources", typeof(Resources).Assembly);
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

	public static string AccessDenied_ErrorOccurred => ResourceManager.GetString("AccessDenied_ErrorOccurred", resourceCulture);

	public static string btnCancel => ResourceManager.GetString("btnCancel", resourceCulture);

	public static string btnClose => ResourceManager.GetString("btnClose", resourceCulture);

	public static string btnFinish_Text => ResourceManager.GetString("btnFinish_Text", resourceCulture);

	public static string btnNext_Text => ResourceManager.GetString("btnNext_Text", resourceCulture);

	public static string btnPrevious => ResourceManager.GetString("btnPrevious", resourceCulture);

	public static string CopyLink => ResourceManager.GetString("CopyLink", resourceCulture);

	public static string Database_CoundNotRestartService => ResourceManager.GetString("Database_CoundNotRestartService", resourceCulture);

	public static string Database_CreateDbFailed => ResourceManager.GetString("Database_CreateDbFailed", resourceCulture);

	public static string Database_dbupdateException => ResourceManager.GetString("Database_dbupdateException", resourceCulture);

	public static string Database_dbupdateFailed => ResourceManager.GetString("Database_dbupdateFailed", resourceCulture);

	public static string Database_ErrorAdminRights => ResourceManager.GetString("Database_ErrorAdminRights", resourceCulture);

	public static string Database_ErrorDbNotFound => ResourceManager.GetString("Database_ErrorDbNotFound", resourceCulture);

	public static string Database_ErrorLogin => ResourceManager.GetString("Database_ErrorLogin", resourceCulture);

	public static string Database_ErrorOverwriteImpossible => ResourceManager.GetString("Database_ErrorOverwriteImpossible", resourceCulture);

	public static string Database_ErrorServerNotFound => ResourceManager.GetString("Database_ErrorServerNotFound", resourceCulture);

	public static string Database_Exists => ResourceManager.GetString("Database_Exists", resourceCulture);

	public static string Database_NoRightToCreateDb => ResourceManager.GetString("Database_NoRightToCreateDb", resourceCulture);

	public static string Database_NotSupportedSQLVersion => ResourceManager.GetString("Database_NotSupportedSQLVersion", resourceCulture);

	public static string Database_NotSupportedSQLVersion_Short => ResourceManager.GetString("Database_NotSupportedSQLVersion_Short", resourceCulture);

	public static string Database_RestartSQLServerNeededCaption => ResourceManager.GetString("Database_RestartSQLServerNeededCaption", resourceCulture);

	public static string Database_RestartSQLServerNeededText => ResourceManager.GetString("Database_RestartSQLServerNeededText", resourceCulture);

	public static string DatabaseInstallType_ConnectionErrorHeadLine => ResourceManager.GetString("DatabaseInstallType_ConnectionErrorHeadLine", resourceCulture);

	public static Bitmap defaultbkg => (Bitmap)ResourceManager.GetObject("defaultbkg", resourceCulture);

	public static string Error_CanceledByUser => ResourceManager.GetString("Error_CanceledByUser", resourceCulture);

	public static string ErrorMessage_FipsEnabledOnUpgrade => ResourceManager.GetString("ErrorMessage_FipsEnabledOnUpgrade", resourceCulture);

	public static string ErrorMessage_FipsEnabledOnUpgradeLinkText => ResourceManager.GetString("ErrorMessage_FipsEnabledOnUpgradeLinkText", resourceCulture);

	public static string ErrorMessage_FipsEnabledOnUpgradeTitle => ResourceManager.GetString("ErrorMessage_FipsEnabledOnUpgradeTitle", resourceCulture);

	public static string Farsi_Replacement_String => ResourceManager.GetString("Farsi_Replacement_String", resourceCulture);

	public static string IisService_CantStart => ResourceManager.GetString("IisService_CantStart", resourceCulture);

	public static string IisService_CantStartTimeOut => ResourceManager.GetString("IisService_CantStartTimeOut", resourceCulture);

	public static string IisService_ErrorDisabled => ResourceManager.GetString("IisService_ErrorDisabled", resourceCulture);

	public static string IisService_ErrorNotAutoStart => ResourceManager.GetString("IisService_ErrorNotAutoStart", resourceCulture);

	public static string IisService_ErrorOccured => ResourceManager.GetString("IisService_ErrorOccured", resourceCulture);

	public static string IISSetup_ErrorConfiguring => ResourceManager.GetString("IISSetup_ErrorConfiguring", resourceCulture);

	public static string IISSetup_ErrorPortInUse => ResourceManager.GetString("IISSetup_ErrorPortInUse", resourceCulture);

	public static string IISSetup_ErrorUnableStartWebSite => ResourceManager.GetString("IISSetup_ErrorUnableStartWebSite", resourceCulture);

	public static string IISSetup_MessageFilename => ResourceManager.GetString("IISSetup_MessageFilename", resourceCulture);

	public static string IISSetup_MessageProcessName => ResourceManager.GetString("IISSetup_MessageProcessName", resourceCulture);

	public static string IISSetup_MessageResolveStartProcess => ResourceManager.GetString("IISSetup_MessageResolveStartProcess", resourceCulture);

	public static string IISSetup_MessageUnableStartServiceSubLine => ResourceManager.GetString("IISSetup_MessageUnableStartServiceSubLine", resourceCulture);

	public static string IISSSetup_SetupErrorHeadLine => ResourceManager.GetString("IISSSetup_SetupErrorHeadLine", resourceCulture);

	public static string KeyStorageExceptionGenericMessage => ResourceManager.GetString("KeyStorageExceptionGenericMessage", resourceCulture);

	public static string Language_LabelHeadLine => ResourceManager.GetString("Language_LabelHeadLine", resourceCulture);

	public static string Language_LabelInstallerLanguage => ResourceManager.GetString("Language_LabelInstallerLanguage", resourceCulture);

	public static string Language_Preparing => ResourceManager.GetString("Language_Preparing", resourceCulture);

	public static string LicUpdate_Headline => ResourceManager.GetString("LicUpdate_Headline", resourceCulture);

	public static string MessageBox_UnableToVerifyAccountType => ResourceManager.GetString("MessageBox_UnableToVerifyAccountType", resourceCulture);

	public static string MessageBox_UnableToVerifyAccountType_Caption => ResourceManager.GetString("MessageBox_UnableToVerifyAccountType_Caption", resourceCulture);

	public static string ServerSelection_AddressNotValid => ResourceManager.GetString("ServerSelection_AddressNotValid", resourceCulture);

	public static string ServerSelection_ServerNotFound => ResourceManager.GetString("ServerSelection_ServerNotFound", resourceCulture);

	public static string ServiceAccount_BrowsButton => ResourceManager.GetString("ServiceAccount_BrowsButton", resourceCulture);

	public static string ServiceAccount_ConfirmPass => ResourceManager.GetString("ServiceAccount_ConfirmPass", resourceCulture);

	public static string ServiceAccount_Headline => ResourceManager.GetString("ServiceAccount_Headline", resourceCulture);

	public static string ServiceAccount_LocalServiceName => ResourceManager.GetString("ServiceAccount_LocalServiceName", resourceCulture);

	public static string ServiceAccount_LocalSystemName => ResourceManager.GetString("ServiceAccount_LocalSystemName", resourceCulture);

	public static string ServiceAccount_MessageAccessDenied => ResourceManager.GetString("ServiceAccount_MessageAccessDenied", resourceCulture);

	public static string ServiceAccount_MessageAccountLogOnFailed => ResourceManager.GetString("ServiceAccount_MessageAccountLogOnFailed", resourceCulture);

	public static string ServiceAccount_MessageAcountNameMissing => ResourceManager.GetString("ServiceAccount_MessageAcountNameMissing", resourceCulture);

	public static string ServiceAccount_MessageAddAcountRightFailed => ResourceManager.GetString("ServiceAccount_MessageAddAcountRightFailed", resourceCulture);

	public static string ServiceAccount_MessageNameNotFound => ResourceManager.GetString("ServiceAccount_MessageNameNotFound", resourceCulture);

	public static string ServiceAccount_MessagePasswordInvalid => ResourceManager.GetString("ServiceAccount_MessagePasswordInvalid", resourceCulture);

	public static string ServiceAccount_MessagePasswordMismatch => ResourceManager.GetString("ServiceAccount_MessagePasswordMismatch", resourceCulture);

	public static string ServiceAccount_MessageUnknownError => ResourceManager.GetString("ServiceAccount_MessageUnknownError", resourceCulture);

	public static string ServiceAccount_MessageValueInvalid => ResourceManager.GetString("ServiceAccount_MessageValueInvalid", resourceCulture);

	public static string ServiceAccount_NetworkServiceName => ResourceManager.GetString("ServiceAccount_NetworkServiceName", resourceCulture);

	public static string ServiceAccount_Password => ResourceManager.GetString("ServiceAccount_Password", resourceCulture);

	public static string ServiceAccount_PrefAccount => ResourceManager.GetString("ServiceAccount_PrefAccount", resourceCulture);

	public static string ServiceAccount_SubHeadLine => ResourceManager.GetString("ServiceAccount_SubHeadLine", resourceCulture);

	public static string ServiceAccount_ThisAccount => ResourceManager.GetString("ServiceAccount_ThisAccount", resourceCulture);

	public static string Update_RetrieveNewLic => ResourceManager.GetString("Update_RetrieveNewLic", resourceCulture);

	public static Bitmap Update_Success => (Bitmap)ResourceManager.GetObject("Update_Success", resourceCulture);

	public static Bitmap Update_Warning => (Bitmap)ResourceManager.GetObject("Update_Warning", resourceCulture);

	public Resources()
	{
	}
}
