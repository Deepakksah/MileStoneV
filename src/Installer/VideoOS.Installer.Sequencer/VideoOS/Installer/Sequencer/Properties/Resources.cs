using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace VideoOS.Installer.Sequencer.Properties;

[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
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
				resourceMan = new ResourceManager("VideoOS.Installer.Sequencer.Properties.Resources", typeof(Resources).Assembly);
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

	public static string Install_CopingFiles => ResourceManager.GetString("Install_CopingFiles", resourceCulture);

	public static string InstallerStatus_Completed => ResourceManager.GetString("InstallerStatus_Completed", resourceCulture);

	public static string InstallerStatus_NotStarted => ResourceManager.GetString("InstallerStatus_NotStarted", resourceCulture);

	public static string InstallerStatus_Paused => ResourceManager.GetString("InstallerStatus_Paused", resourceCulture);

	public static string InstallerStatus_Started => ResourceManager.GetString("InstallerStatus_Started", resourceCulture);

	public static string InstallerStatus_Terminating => ResourceManager.GetString("InstallerStatus_Terminating", resourceCulture);

	public static string Message_IISInstRebootRequired => ResourceManager.GetString("Message_IISInstRebootRequired", resourceCulture);

	public static string Message_RestartAfterPrereqInstalled => ResourceManager.GetString("Message_RestartAfterPrereqInstalled", resourceCulture);

	public static string Sequencer_IisIsntallFailed => ResourceManager.GetString("Sequencer_IisIsntallFailed", resourceCulture);

	public static string Sequencer_IisIsntallFailedWorkstation => ResourceManager.GetString("Sequencer_IisIsntallFailedWorkstation", resourceCulture);

	public static string Sequencer_Installing => ResourceManager.GetString("Sequencer_Installing", resourceCulture);

	public static string Sequencer_Uninstalling => ResourceManager.GetString("Sequencer_Uninstalling", resourceCulture);

	public static string Sequencer_UninstallingFailed => ResourceManager.GetString("Sequencer_UninstallingFailed", resourceCulture);

	public static string Sequencer_UnknownFormat => ResourceManager.GetString("Sequencer_UnknownFormat", resourceCulture);

	public static string SSRegistrationTask_DisplayName => ResourceManager.GetString("SSRegistrationTask_DisplayName", resourceCulture);

	public Resources()
	{
	}
}
