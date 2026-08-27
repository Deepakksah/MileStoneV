using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace VideoOS.Installer.RecorderPlugins.Properties;

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
				resourceMan = new ResourceManager("VideoOS.Installer.RecorderPlugins.Properties.Resources", typeof(Resources).Assembly);
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

	public static string ErrorMessage_Caption => ResourceManager.GetString("ErrorMessage_Caption", resourceCulture);

	public static string ErrorMessage_ConnectionFailed => ResourceManager.GetString("ErrorMessage_ConnectionFailed", resourceCulture);

	public static string ErrorMessage_EmptyMediaDBPath => ResourceManager.GetString("ErrorMessage_EmptyMediaDBPath", resourceCulture);

	public static string ErrorMessage_InvalidMediaDBPath => ResourceManager.GetString("ErrorMessage_InvalidMediaDBPath", resourceCulture);

	public static string ErrorMessage_InvalidServerCertificateSelection => ResourceManager.GetString("ErrorMessage_InvalidServerCertificateSelection", resourceCulture);

	public static string ErrorMessage_InvalidServerURL => ResourceManager.GetString("ErrorMessage_InvalidServerURL", resourceCulture);

	public static string ErrorMessage_MustInstallBeforeChangingTracks => ResourceManager.GetString("ErrorMessage_MustInstallBeforeChangingTracks", resourceCulture);

	public static string ErrorMessage_ServerAddressEmpty => ResourceManager.GetString("ErrorMessage_ServerAddressEmpty", resourceCulture);

	public static string ErrorMessage_ServerNameEmpty => ResourceManager.GetString("ErrorMessage_ServerNameEmpty", resourceCulture);

	public static string ErrorMessage_ServerNotFound => ResourceManager.GetString("ErrorMessage_ServerNotFound", resourceCulture);

	public static string ErrorMessage_ServiceLogOnSetupDialogPasswordInvalid => ResourceManager.GetString("ErrorMessage_ServiceLogOnSetupDialogPasswordInvalid", resourceCulture);

	public static string ErrroMessage_AddAccountRightGranted => ResourceManager.GetString("ErrroMessage_AddAccountRightGranted", resourceCulture);

	public static string ErrroMessage_AddAcountRightError => ResourceManager.GetString("ErrroMessage_AddAcountRightError", resourceCulture);

	public static string ErrroMessage_AddAcountRightUserNotFound => ResourceManager.GetString("ErrroMessage_AddAcountRightUserNotFound", resourceCulture);

	public static string ErrroMessage_ServiceLogOnSetupDialogErrorCode => ResourceManager.GetString("ErrroMessage_ServiceLogOnSetupDialogErrorCode", resourceCulture);

	public static string InvalidConfigFileException => ResourceManager.GetString("InvalidConfigFileException", resourceCulture);

	public static string Logging_ServiceAccountAddRightGranted => ResourceManager.GetString("Logging_ServiceAccountAddRightGranted", resourceCulture);

	public static string MediaDatabaseLocation_LabelSpaceAvailable => ResourceManager.GetString("MediaDatabaseLocation_LabelSpaceAvailable", resourceCulture);

	public static string MediaDatabaseLocation_LabelSpaceAvailableMb => ResourceManager.GetString("MediaDatabaseLocation_LabelSpaceAvailableMb", resourceCulture);

	public static string MediaDatabaseLocation_LabelSpaceAvailableTb => ResourceManager.GetString("MediaDatabaseLocation_LabelSpaceAvailableTb", resourceCulture);

	public static string MessageBox_MakeSureUserHasAccess => ResourceManager.GetString("MessageBox_MakeSureUserHasAccess", resourceCulture);

	public static string MessageBox_MakeSureUserHasAccess_Caption => ResourceManager.GetString("MessageBox_MakeSureUserHasAccess_Caption", resourceCulture);

	public static string MessageBox_UnableToVerifyAccountType => ResourceManager.GetString("MessageBox_UnableToVerifyAccountType", resourceCulture);

	public static string MessageBox_UnableToVerifyAccountType_Caption => ResourceManager.GetString("MessageBox_UnableToVerifyAccountType_Caption", resourceCulture);

	public static string ServerConfig_BrowseButton => ResourceManager.GetString("ServerConfig_BrowseButton", resourceCulture);

	public static string ServerConfig_FailoverLicenseNotSupported => ResourceManager.GetString("ServerConfig_FailoverLicenseNotSupported", resourceCulture);

	public static string ServerConfig_HeadLine => ResourceManager.GetString("ServerConfig_HeadLine", resourceCulture);

	public static string ServerConfig_ManagementServerExample => ResourceManager.GetString("ServerConfig_ManagementServerExample", resourceCulture);

	public static string ServerConfig_ManagementServerURL => ResourceManager.GetString("ServerConfig_ManagementServerURL", resourceCulture);

	public static string ServerConfig_ManagementServerURLInfo => ResourceManager.GetString("ServerConfig_ManagementServerURLInfo", resourceCulture);

	public static string ServerConfig_MediaDb => ResourceManager.GetString("ServerConfig_MediaDb", resourceCulture);

	public static string ServerConfig_MediaDbInfo => ResourceManager.GetString("ServerConfig_MediaDbInfo", resourceCulture);

	public static string ServerConfig_RetentionDaysLabel => ResourceManager.GetString("ServerConfig_RetentionDaysLabel", resourceCulture);

	public static string ServerConfig_RetentionTimeLabel => ResourceManager.GetString("ServerConfig_RetentionTimeLabel", resourceCulture);

	public static string ServerConfig_ServerName => ResourceManager.GetString("ServerConfig_ServerName", resourceCulture);

	public static string ServerUninstall_HeadLine => ResourceManager.GetString("ServerUninstall_HeadLine", resourceCulture);

	public static string ServerUninstall_KeepDp => ResourceManager.GetString("ServerUninstall_KeepDp", resourceCulture);

	public static string ServerUninstall_KeepMediaArch => ResourceManager.GetString("ServerUninstall_KeepMediaArch", resourceCulture);

	public static string ServiceAccount_FailoverSubLine => ResourceManager.GetString("ServiceAccount_FailoverSubLine", resourceCulture);

	public static string ServiceAccount_HeadLine => ResourceManager.GetString("ServiceAccount_HeadLine", resourceCulture);

	public static string ServiceAccount_LocalServiceName => ResourceManager.GetString("ServiceAccount_LocalServiceName", resourceCulture);

	public static string ServiceAccount_LocalSystemName => ResourceManager.GetString("ServiceAccount_LocalSystemName", resourceCulture);

	public static string ServiceAccount_NetworkServiceName => ResourceManager.GetString("ServiceAccount_NetworkServiceName", resourceCulture);

	public static string WarningMessage_ConnectionFailed => ResourceManager.GetString("WarningMessage_ConnectionFailed", resourceCulture);

	public static string WarningMessage_GdprRetention => ResourceManager.GetString("WarningMessage_GdprRetention", resourceCulture);

	public static string WarningPageHeadingDetails => ResourceManager.GetString("WarningPageHeadingDetails", resourceCulture);

	public Resources()
	{
	}
}
