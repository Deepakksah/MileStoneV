using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace VideoOS.Server.Properties;

[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
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
				ResourceManager resourceManager = new ResourceManager("VideoOS.Server.Properties.Resources", typeof(Resources).Assembly);
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

	public static string AuthenticationBrokenMessage => ResourceManager.GetString("AuthenticationBrokenMessage", resourceCulture);

	public static string DatabaseConfigExtraRows => ResourceManager.GetString("DatabaseConfigExtraRows", resourceCulture);

	public static string DatabaseConfigMissingRow => ResourceManager.GetString("DatabaseConfigMissingRow", resourceCulture);

	public static string EventGeneratorSourceExternal => ResourceManager.GetString("EventGeneratorSourceExternal", resourceCulture);

	public static string EventGeneratorSourceServer => ResourceManager.GetString("EventGeneratorSourceServer", resourceCulture);

	public static string EventGeneratorSourceSystem => ResourceManager.GetString("EventGeneratorSourceSystem", resourceCulture);

	public static string EventGeneratorSourceUnknown => ResourceManager.GetString("EventGeneratorSourceUnknown", resourceCulture);

	public static string EventNameUnknown => ResourceManager.GetString("EventNameUnknown", resourceCulture);

	public static string EvidenceLock_Restored => ResourceManager.GetString("EvidenceLock_Restored", resourceCulture);

	public static string EvidenceLockCommand_TimeoutReceived => ResourceManager.GetString("EvidenceLockCommand_TimeoutReceived", resourceCulture);

	public static string ExternalPluginConfigDirNull => ResourceManager.GetString("ExternalPluginConfigDirNull", resourceCulture);

	public static string GenerelSqlError => ResourceManager.GetString("GenerelSqlError", resourceCulture);

	public static string InvalidClientVersionConfigDirectory => ResourceManager.GetString("InvalidClientVersionConfigDirectory", resourceCulture);

	public static string InvalidTemporaryLicenseFile => ResourceManager.GetString("InvalidTemporaryLicenseFile", resourceCulture);

	public static string IServerApiExpiredToken => ResourceManager.GetString("IServerApiExpiredToken", resourceCulture);

	public static string IServerApiInvalidToken => ResourceManager.GetString("IServerApiInvalidToken", resourceCulture);

	public static string IServerFeatureNotLicensed => ResourceManager.GetString("IServerFeatureNotLicensed", resourceCulture);

	public static string IServerInternalError => ResourceManager.GetString("IServerInternalError", resourceCulture);

	public static string IServerSqlConcurrencyError => ResourceManager.GetString("IServerSqlConcurrencyError", resourceCulture);

	public static string IServerSqlError => ResourceManager.GetString("IServerSqlError", resourceCulture);

	public static string IServerSqlUniqueIndexOrConstraintViolation => ResourceManager.GetString("IServerSqlUniqueIndexOrConstraintViolation", resourceCulture);

	public static string LookupSidException => ResourceManager.GetString("LookupSidException", resourceCulture);

	public static string NoNullAllowedException => ResourceManager.GetString("NoNullAllowedException", resourceCulture);

	public static string NotificationManager_CustomEvent => ResourceManager.GetString("NotificationManager_CustomEvent", resourceCulture);

	public static string NotificationManager_SystemEvent => ResourceManager.GetString("NotificationManager_SystemEvent", resourceCulture);

	public static string NotificationManager_TimesEvent => ResourceManager.GetString("NotificationManager_TimesEvent", resourceCulture);

	public static string SLCInvalidInLicense => ResourceManager.GetString("SLCInvalidInLicense", resourceCulture);

	public static string SNMPSourceName => ResourceManager.GetString("SNMPSourceName", resourceCulture);

	public static string SNMPSourceType => ResourceManager.GetString("SNMPSourceType", resourceCulture);

	public static string StatusActivateRecorderFailed => ResourceManager.GetString("StatusActivateRecorderFailed", resourceCulture);

	public static string StatusActivateRecorderSuccess => ResourceManager.GetString("StatusActivateRecorderSuccess", resourceCulture);

	public static string StatusCommandProcessorShutdown => ResourceManager.GetString("StatusCommandProcessorShutdown", resourceCulture);

	public static string StatusErrorRegistrationIdpServer => ResourceManager.GetString("StatusErrorRegistrationIdpServer", resourceCulture);

	public static string StatusInitializeCommunicationClients => ResourceManager.GetString("StatusInitializeCommunicationClients", resourceCulture);

	public static string StatusInitializeCommunicationClientsFailed => ResourceManager.GetString("StatusInitializeCommunicationClientsFailed", resourceCulture);

	public static string StatusInitializeRuleProcessor => ResourceManager.GetString("StatusInitializeRuleProcessor", resourceCulture);

	public static string StatusLicenseUpdated => ResourceManager.GetString("StatusLicenseUpdated", resourceCulture);

	public static string StatusNoDomainUserOnDomainComputer => ResourceManager.GetString("StatusNoDomainUserOnDomainComputer", resourceCulture);

	public static string StatusRuleProcessorShutdown => ResourceManager.GetString("StatusRuleProcessorShutdown", resourceCulture);

	public static string StatusSendCompleteConfiguration => ResourceManager.GetString("StatusSendCompleteConfiguration", resourceCulture);

	public static string StatusServerShutdown => ResourceManager.GetString("StatusServerShutdown", resourceCulture);

	public static string StatusSNMPManagerStarted => ResourceManager.GetString("StatusSNMPManagerStarted", resourceCulture);

	public static string StatusStartApplyConfiguration => ResourceManager.GetString("StatusStartApplyConfiguration", resourceCulture);

	public static string StatusStartCommandProcessor => ResourceManager.GetString("StatusStartCommandProcessor", resourceCulture);

	public static string StatusStartDatabase => ResourceManager.GetString("StatusStartDatabase", resourceCulture);

	public static string StatusStartExternalPluginConfig => ResourceManager.GetString("StatusStartExternalPluginConfig", resourceCulture);

	public static string StatusStartIdp => ResourceManager.GetString("StatusStartIdp", resourceCulture);

	public static string StatusStartLicenseManager => ResourceManager.GetString("StatusStartLicenseManager", resourceCulture);

	public static string StatusStartLogManager => ResourceManager.GetString("StatusStartLogManager", resourceCulture);

	public static string StatusStartRecorderCommunication => ResourceManager.GetString("StatusStartRecorderCommunication", resourceCulture);

	public static string StatusStartRuleProcessor => ResourceManager.GetString("StatusStartRuleProcessor", resourceCulture);

	public static string StatusStartSNMPManager => ResourceManager.GetString("StatusStartSNMPManager", resourceCulture);

	public static string StatusStartTaskManager => ResourceManager.GetString("StatusStartTaskManager", resourceCulture);

	public static string StatusStartVmoClient => ResourceManager.GetString("StatusStartVmoClient", resourceCulture);

	public static string StatusStopSNMPManager => ResourceManager.GetString("StatusStopSNMPManager", resourceCulture);

	public static string StatusWaitForIdpServer => ResourceManager.GetString("StatusWaitForIdpServer", resourceCulture);

	public static string StatusWaitForSqlServer => ResourceManager.GetString("StatusWaitForSqlServer", resourceCulture);

	public static string UnauthorizedAccessException => ResourceManager.GetString("UnauthorizedAccessException", resourceCulture);

	public static string VmoDatabaseConfigExtraRows => ResourceManager.GetString("VmoDatabaseConfigExtraRows", resourceCulture);

	public static string VmoDatabaseConfigMissingRow => ResourceManager.GetString("VmoDatabaseConfigMissingRow", resourceCulture);

	public static string VmoGetSidException => ResourceManager.GetString("VmoGetSidException", resourceCulture);

	public static string VmoInvalidDomain => ResourceManager.GetString("VmoInvalidDomain", resourceCulture);

	public Resources()
	{
	}
}
