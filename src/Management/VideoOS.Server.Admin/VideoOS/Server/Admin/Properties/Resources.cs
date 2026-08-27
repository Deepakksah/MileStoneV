using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace VideoOS.Server.Admin.Properties;

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
				resourceMan = new ResourceManager("VideoOS.Server.Admin.Properties.Resources", typeof(Resources).Assembly);
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

	public static string AccountNameNullOrEmpty => ResourceManager.GetString("AccountNameNullOrEmpty", resourceCulture);

	public static string AspNetInstallFailed => ResourceManager.GetString("AspNetInstallFailed", resourceCulture);

	public static string FailedToConfigureServer => ResourceManager.GetString("FailedToConfigureServer", resourceCulture);

	public static string FailedToConfigureServiceAccount => ResourceManager.GetString("FailedToConfigureServiceAccount", resourceCulture);

	public static string FailedToCreateLogin => ResourceManager.GetString("FailedToCreateLogin", resourceCulture);

	public static string FailedToGetIISVersion => ResourceManager.GetString("FailedToGetIISVersion", resourceCulture);

	public static string FailedToSetDatabaseOwner => ResourceManager.GetString("FailedToSetDatabaseOwner", resourceCulture);

	public static string FaultToConfigureDatabaseAccount => ResourceManager.GetString("FaultToConfigureDatabaseAccount", resourceCulture);

	public static string GrantAccessFailed => ResourceManager.GetString("GrantAccessFailed", resourceCulture);

	public static string IISResetNotFound => ResourceManager.GetString("IISResetNotFound", resourceCulture);

	public static string InvalidAccount => ResourceManager.GetString("InvalidAccount", resourceCulture);

	public static string NotSufficientPrivileges => ResourceManager.GetString("NotSufficientPrivileges", resourceCulture);

	public static string NullStoredProcedure => ResourceManager.GetString("NullStoredProcedure", resourceCulture);

	public static string ServerUninstallConfiguration => ResourceManager.GetString("ServerUninstallConfiguration", resourceCulture);

	public static string SetConfigFilePathFailed => ResourceManager.GetString("SetConfigFilePathFailed", resourceCulture);

	public static string SetupHtmlHelpVirtualDirFailed => ResourceManager.GetString("SetupHtmlHelpVirtualDirFailed", resourceCulture);

	public static string SetupInstallationAdminVirtualDirFailed => ResourceManager.GetString("SetupInstallationAdminVirtualDirFailed", resourceCulture);

	public static string SetupInstallationVirtualDirFailed => ResourceManager.GetString("SetupInstallationVirtualDirFailed", resourceCulture);

	public static string SetupMgmtServerVirtualDirFailed => ResourceManager.GetString("SetupMgmtServerVirtualDirFailed", resourceCulture);

	public static string SetupServerConnectorVirtualDirFailed => ResourceManager.GetString("SetupServerConnectorVirtualDirFailed", resourceCulture);

	public static string SetupServerVirtualDirFailed => ResourceManager.GetString("SetupServerVirtualDirFailed", resourceCulture);

	public static string SetupSystemInformationVirtualDirFailed => ResourceManager.GetString("SetupSystemInformationVirtualDirFailed", resourceCulture);

	public static string StartIISFailed => ResourceManager.GetString("StartIISFailed", resourceCulture);

	public static string UpdateIIS7HandlerPreConditionFailed => ResourceManager.GetString("UpdateIIS7HandlerPreConditionFailed", resourceCulture);

	public static string UpdateUrlFilesFailed => ResourceManager.GetString("UpdateUrlFilesFailed", resourceCulture);

	public static string WindowsCommunicationFoundationInstallFailed => ResourceManager.GetString("WindowsCommunicationFoundationInstallFailed", resourceCulture);

	public static string WriteServerConfigFailed => ResourceManager.GetString("WriteServerConfigFailed", resourceCulture);

	public static string WriteWebConfigFailed => ResourceManager.GetString("WriteWebConfigFailed", resourceCulture);

	public Resources()
	{
	}
}
