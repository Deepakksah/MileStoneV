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
public class Text
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
				ResourceManager resourceManager = new ResourceManager("VideoOS.Server.Service.TrayController.Text", typeof(Text).Assembly);
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

	public static string MsgBoxAboutBuildNumber => ResourceManager.GetString("MsgBoxAboutBuildNumber", resourceCulture);

	public static string MsgBoxAboutTitle => ResourceManager.GetString("MsgBoxAboutTitle", resourceCulture);

	public static string MsgBoxAboutVersion => ResourceManager.GetString("MsgBoxAboutVersion", resourceCulture);

	public static string MsgBoxAboutVersionDefaultText => ResourceManager.GetString("MsgBoxAboutVersionDefaultText", resourceCulture);

	public static string MsgBoxOnlyOneInstanceAllowedCaption => ResourceManager.GetString("MsgBoxOnlyOneInstanceAllowedCaption", resourceCulture);

	public static string MsgBoxOnlyOneInstanseAllowedPerUser => ResourceManager.GetString("MsgBoxOnlyOneInstanseAllowedPerUser", resourceCulture);

	public static string MsgBoxUacErrorCaption => ResourceManager.GetString("MsgBoxUacErrorCaption", resourceCulture);

	public static string TrayIconServiceStatusIndeterminate => ResourceManager.GetString("TrayIconServiceStatusIndeterminate", resourceCulture);

	public static string TrayIconServiceStatusRunning => ResourceManager.GetString("TrayIconServiceStatusRunning", resourceCulture);

	public static string TrayIconServiceStatusStartPending => ResourceManager.GetString("TrayIconServiceStatusStartPending", resourceCulture);

	public static string TrayIconServiceStatusStopped => ResourceManager.GetString("TrayIconServiceStatusStopped", resourceCulture);

	public static string TrayIconServiceStatusStopPending => ResourceManager.GetString("TrayIconServiceStatusStopPending", resourceCulture);

	public static string TrayIconToolTipStatus => ResourceManager.GetString("TrayIconToolTipStatus", resourceCulture);

	public Text()
	{
	}
}
