using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace VideoOS.Installer.DatabasePlugin.Properties;

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
				resourceMan = new ResourceManager("VideoOS.Installer.DatabasePlugin.Properties.Resources", typeof(Resources).Assembly);
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

	public static string Database_ConfirmMessageOverwriteCaption => ResourceManager.GetString("Database_ConfirmMessageOverwriteCaption", resourceCulture);

	public static string Database_ConfirmMessageOverwriteText => ResourceManager.GetString("Database_ConfirmMessageOverwriteText", resourceCulture);

	public static string Database_CoundNotRestartService => ResourceManager.GetString("Database_CoundNotRestartService", resourceCulture);

	public static string Database_dbupdateException => ResourceManager.GetString("Database_dbupdateException", resourceCulture);

	public static string Database_dbupdateFailed => ResourceManager.GetString("Database_dbupdateFailed", resourceCulture);

	public static string Database_ErrorAdminRights => ResourceManager.GetString("Database_ErrorAdminRights", resourceCulture);

	public static string Database_ErrorDbNotFound => ResourceManager.GetString("Database_ErrorDbNotFound", resourceCulture);

	public static string Database_ErrorLogin => ResourceManager.GetString("Database_ErrorLogin", resourceCulture);

	public static string Database_ErrorOverwriteImpossible => ResourceManager.GetString("Database_ErrorOverwriteImpossible", resourceCulture);

	public static string Database_ErrorServerNotFound => ResourceManager.GetString("Database_ErrorServerNotFound", resourceCulture);

	public static string Database_Exists => ResourceManager.GetString("Database_Exists", resourceCulture);

	public static string Database_NotSupportedSQLVersion => ResourceManager.GetString("Database_NotSupportedSQLVersion", resourceCulture);

	public static string Database_RestartSQLServerNeededText => ResourceManager.GetString("Database_RestartSQLServerNeededText", resourceCulture);

	public static string DatabaseAdvancedSetup_AuthenticationTypeHeader => ResourceManager.GetString("DatabaseAdvancedSetup_AuthenticationTypeHeader", resourceCulture);

	public static string DatabaseAdvancedSetup_DatabaseHeader => ResourceManager.GetString("DatabaseAdvancedSetup_DatabaseHeader", resourceCulture);

	public static string DatabaseAdvancedSetup_Description => ResourceManager.GetString("DatabaseAdvancedSetup_Description", resourceCulture);

	public static string DatabaseAdvancedSetup_Headline => ResourceManager.GetString("DatabaseAdvancedSetup_Headline", resourceCulture);

	public static string DatabaseAdvancedSetup_IdentityServer => ResourceManager.GetString("DatabaseAdvancedSetup_IdentityServer", resourceCulture);

	public static string DatabaseAdvancedSetup_IncidentManager => ResourceManager.GetString("DatabaseAdvancedSetup_IncidentManager", resourceCulture);

	public static string DatabaseAdvancedSetup_LogServer => ResourceManager.GetString("DatabaseAdvancedSetup_LogServer", resourceCulture);

	public static string DatabaseAdvancedSetup_ManagementServer => ResourceManager.GetString("DatabaseAdvancedSetup_ManagementServer", resourceCulture);

	public static string DatabaseAdvancedSetup_MicrosoftEntraIntegrated => ResourceManager.GetString("DatabaseAdvancedSetup_MicrosoftEntraIntegrated", resourceCulture);

	public static string DatabaseAdvancedSetup_MicrosoftEntraManagedIdentity => ResourceManager.GetString("DatabaseAdvancedSetup_MicrosoftEntraManagedIdentity", resourceCulture);

	public static string DatabaseAdvancedSetup_ServerHeader => ResourceManager.GetString("DatabaseAdvancedSetup_ServerHeader", resourceCulture);

	public static string DatabaseAdvancedSetup_TryConnect_Fail => ResourceManager.GetString("DatabaseAdvancedSetup_TryConnect_Fail", resourceCulture);

	public static string DatabaseAdvancedSetup_TryConnect_MissingDatabaseName => ResourceManager.GetString("DatabaseAdvancedSetup_TryConnect_MissingDatabaseName", resourceCulture);

	public static string DatabaseAdvancedSetup_TryConnect_MissingServerName => ResourceManager.GetString("DatabaseAdvancedSetup_TryConnect_MissingServerName", resourceCulture);

	public static string DatabaseAdvancedSetup_TryConnect_Success => ResourceManager.GetString("DatabaseAdvancedSetup_TryConnect_Success", resourceCulture);

	public static string DatabaseAdvancedSetup_TryConnect_Unexpected => ResourceManager.GetString("DatabaseAdvancedSetup_TryConnect_Unexpected", resourceCulture);

	public static string DatabaseAdvancedSetup_WindowsAuthentication => ResourceManager.GetString("DatabaseAdvancedSetup_WindowsAuthentication", resourceCulture);

	public static string DatabaseAdvancedSetup_WindowsAuthenticationTrusted => ResourceManager.GetString("DatabaseAdvancedSetup_WindowsAuthenticationTrusted", resourceCulture);

	public static string DatabaseAdvancedSetupStandalone_ConnectionStringLabel => ResourceManager.GetString("DatabaseAdvancedSetupStandalone_ConnectionStringLabel", resourceCulture);

	public static string DatabaseExistType_CreateNewdb => ResourceManager.GetString("DatabaseExistType_CreateNewdb", resourceCulture);

	public static string DatabaseExistType_DatabaseName => ResourceManager.GetString("DatabaseExistType_DatabaseName", resourceCulture);

	public static string DatabaseExistType_ExistDataKeep => ResourceManager.GetString("DatabaseExistType_ExistDataKeep", resourceCulture);

	public static string DatabaseExistType_ExistDataOverwrite => ResourceManager.GetString("DatabaseExistType_ExistDataOverwrite", resourceCulture);

	public static string DatabaseExistType_FailedLoadServers => ResourceManager.GetString("DatabaseExistType_FailedLoadServers", resourceCulture);

	public static string DatabaseExistType_HeadLine => ResourceManager.GetString("DatabaseExistType_HeadLine", resourceCulture);

	public static string DatabaseExistType_SubLine => ResourceManager.GetString("DatabaseExistType_SubLine", resourceCulture);

	public static string DatabaseExistType_UseExistDb => ResourceManager.GetString("DatabaseExistType_UseExistDb", resourceCulture);

	public static string DatabaseExistType_UseExistOptions => ResourceManager.GetString("DatabaseExistType_UseExistOptions", resourceCulture);

	public static string DatabaseExternallyManaged_Description => ResourceManager.GetString("DatabaseExternallyManaged_Description", resourceCulture);

	public static string DatabaseExternallyManaged_Headline => ResourceManager.GetString("DatabaseExternallyManaged_Headline", resourceCulture);

	public static string DatabaseExternallyManaged_PreCreated => ResourceManager.GetString("DatabaseExternallyManaged_PreCreated", resourceCulture);

	public static string DatabaseExternallyManaged_PreCreated_Description => ResourceManager.GetString("DatabaseExternallyManaged_PreCreated_Description", resourceCulture);

	public static string DatabaseExternallyManaged_XProtect => ResourceManager.GetString("DatabaseExternallyManaged_XProtect", resourceCulture);

	public static string DatabaseExternallyManaged_XProtect_Description => ResourceManager.GetString("DatabaseExternallyManaged_XProtect_Description", resourceCulture);

	public static string DatabaseInstall_HeadLine => ResourceManager.GetString("DatabaseInstall_HeadLine", resourceCulture);

	public static string DatabaseInstall_InstalledVersionNotSupported => ResourceManager.GetString("DatabaseInstall_InstalledVersionNotSupported", resourceCulture);

	public static string DatabaseInstall_InstalledVersionNotSupportedHeadLine => ResourceManager.GetString("DatabaseInstall_InstalledVersionNotSupportedHeadLine", resourceCulture);

	public static string DatabaseInstall_InstallNew => ResourceManager.GetString("DatabaseInstall_InstallNew", resourceCulture);

	public static string DatabaseInstall_ManualSelectServer => ResourceManager.GetString("DatabaseInstall_ManualSelectServer", resourceCulture);

	public static string DatabaseInstall_ServerNotFoundError => ResourceManager.GetString("DatabaseInstall_ServerNotFoundError", resourceCulture);

	public static string DatabaseInstall_SqlServerName => ResourceManager.GetString("DatabaseInstall_SqlServerName", resourceCulture);

	public static string DatabaseInstall_SubLine => ResourceManager.GetString("DatabaseInstall_SubLine", resourceCulture);

	public static string DatabaseInstall_UseExisting => ResourceManager.GetString("DatabaseInstall_UseExisting", resourceCulture);

	public static string DatabaseInstall_UseInstalled => ResourceManager.GetString("DatabaseInstall_UseInstalled", resourceCulture);

	public static string DatabaseInstall_UserRightsError => ResourceManager.GetString("DatabaseInstall_UserRightsError", resourceCulture);

	public static string DatabaseInstallType_ConnectionErrorHeadLine => ResourceManager.GetString("DatabaseInstallType_ConnectionErrorHeadLine", resourceCulture);

	public static string DatabaseInstallType_InsertServerAddress => ResourceManager.GetString("DatabaseInstallType_InsertServerAddress", resourceCulture);

	public static string DatabaseInstallType_PrepareSQLServers => ResourceManager.GetString("DatabaseInstallType_PrepareSQLServers", resourceCulture);

	public static string DatabaseInstallType_UnsupportedComponent => ResourceManager.GetString("DatabaseInstallType_UnsupportedComponent", resourceCulture);

	public static string DatabaseRestore_BackupDescription_Header => ResourceManager.GetString("DatabaseRestore_BackupDescription_Header", resourceCulture);

	public static string DatabaseRestore_ButtonRestore_Description => ResourceManager.GetString("DatabaseRestore_ButtonRestore_Description", resourceCulture);

	public static string DatabaseRestore_ButtonRestore_Header => ResourceManager.GetString("DatabaseRestore_ButtonRestore_Header", resourceCulture);

	public static string DatabaseRestore_ConfirmApply_Description => ResourceManager.GetString("DatabaseRestore_ConfirmApply_Description", resourceCulture);

	public static string DatabaseRestore_ConfirmApply_Header => ResourceManager.GetString("DatabaseRestore_ConfirmApply_Header", resourceCulture);

	public static string DatabaseRestore_ConfirmDelete_Description => ResourceManager.GetString("DatabaseRestore_ConfirmDelete_Description", resourceCulture);

	public static string DatabaseRestore_ConfirmDelete_Header => ResourceManager.GetString("DatabaseRestore_ConfirmDelete_Header", resourceCulture);

	public static string DatabaseRestore_Delete => ResourceManager.GetString("DatabaseRestore_Delete", resourceCulture);

	public static string DatabaseRestore_Description => ResourceManager.GetString("DatabaseRestore_Description", resourceCulture);

	public static string DatabaseRestore_Failure_Description => ResourceManager.GetString("DatabaseRestore_Failure_Description", resourceCulture);

	public static string DatabaseRestore_Failure_Header => ResourceManager.GetString("DatabaseRestore_Failure_Header", resourceCulture);

	public static string DatabaseRestore_Headline => ResourceManager.GetString("DatabaseRestore_Headline", resourceCulture);

	public static string DatabaseRestore_Skip_Description => ResourceManager.GetString("DatabaseRestore_Skip_Description", resourceCulture);

	public static string DatabaseRestore_Skip_Header => ResourceManager.GetString("DatabaseRestore_Skip_Header", resourceCulture);

	public static string DatabaseRestore_Success_Description => ResourceManager.GetString("DatabaseRestore_Success_Description", resourceCulture);

	public static string DatabaseRestore_Success_Header => ResourceManager.GetString("DatabaseRestore_Success_Header", resourceCulture);

	public static string KeyManagementConfirmPassword => ResourceManager.GetString("KeyManagementConfirmPassword", resourceCulture);

	public static string KeyManagementConfirmPasswordPageHeader => ResourceManager.GetString("KeyManagementConfirmPasswordPageHeader", resourceCulture);

	public static string KeyManagementConfirmPasswordPageInfo => ResourceManager.GetString("KeyManagementConfirmPasswordPageInfo", resourceCulture);

	public static string KeyManagementDoNotAssignPassword => ResourceManager.GetString("KeyManagementDoNotAssignPassword", resourceCulture);

	public static string KeyManagementErrorIncorrectPassword => ResourceManager.GetString("KeyManagementErrorIncorrectPassword", resourceCulture);

	public static string KeyManagementNoExistingPassword => ResourceManager.GetString("KeyManagementNoExistingPassword", resourceCulture);

	public static string KeyManagementPassword => ResourceManager.GetString("KeyManagementPassword", resourceCulture);

	public static string KeyManagementPasswordPageHeader => ResourceManager.GetString("KeyManagementPasswordPageHeader", resourceCulture);

	public static string KeyManagementPasswordPageInfo => ResourceManager.GetString("KeyManagementPasswordPageInfo", resourceCulture);

	public static string KeyManagementPasswordPageInfoWarning => ResourceManager.GetString("KeyManagementPasswordPageInfoWarning", resourceCulture);

	public static string KeyStorageExceptionGenericMessage => ResourceManager.GetString("KeyStorageExceptionGenericMessage", resourceCulture);

	public static string NewPasswordDoesNotMeetComplexityPolicy => ResourceManager.GetString("NewPasswordDoesNotMeetComplexityPolicy", resourceCulture);

	public static string NewPasswordEmpty => ResourceManager.GetString("NewPasswordEmpty", resourceCulture);

	public static string NewPasswordInvalid => ResourceManager.GetString("NewPasswordInvalid", resourceCulture);

	public static string NewPasswordPasswordsDoNotMatch => ResourceManager.GetString("NewPasswordPasswordsDoNotMatch", resourceCulture);

	public static string NewPasswordTooLong => ResourceManager.GetString("NewPasswordTooLong", resourceCulture);

	public static string NewPasswordTooShort => ResourceManager.GetString("NewPasswordTooShort", resourceCulture);

	public Resources()
	{
	}
}
