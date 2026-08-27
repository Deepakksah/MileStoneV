using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace VideoOS.Server.Service.TrayController.Resources;

[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
[DebuggerNonUserCode]
[CompilerGenerated]
public class Menu
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
				ResourceManager resourceManager = new ResourceManager("VideoOS.Server.Service.TrayController.Menu", typeof(Menu).Assembly);
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

	public static string About => ResourceManager.GetString("About", resourceCulture);

	public static string ChangeLicense => ResourceManager.GetString("ChangeLicense", resourceCulture);

	public static string ConfigureMsFailover => ResourceManager.GetString("ConfigureMsFailover", resourceCulture);

	public static string EncryptionConfiguration => ResourceManager.GetString("EncryptionConfiguration", resourceCulture);

	public static string EnterTheSystemRecoveryPassword => ResourceManager.GetString("EnterTheSystemRecoveryPassword", resourceCulture);

	public static string Exit => ResourceManager.GetString("Exit", resourceCulture);

	public static string Help => ResourceManager.GetString("Help", resourceCulture);

	public static string ResetTheSystemRecoveryPassword => ResourceManager.GetString("ResetTheSystemRecoveryPassword", resourceCulture);

	public static string Restore => ResourceManager.GetString("Restore", resourceCulture);

	public static string SelectBackupFolder => ResourceManager.GetString("SelectBackupFolder", resourceCulture);

	public static string Separator => ResourceManager.GetString("Separator", resourceCulture);

	public static string ShowStatusMessages => ResourceManager.GetString("ShowStatusMessages", resourceCulture);

	public static string StartService => ResourceManager.GetString("StartService", resourceCulture);

	public static string StopService => ResourceManager.GetString("StopService", resourceCulture);

	public Menu()
	{
	}
}
