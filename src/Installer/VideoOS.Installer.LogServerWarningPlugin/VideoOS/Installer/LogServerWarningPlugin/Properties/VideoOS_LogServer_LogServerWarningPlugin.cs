using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace VideoOS.Installer.LogServerWarningPlugin.Properties;

[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
[DebuggerNonUserCode]
[CompilerGenerated]
public class VideoOS_LogServer_LogServerWarningPlugin
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
				resourceMan = new ResourceManager("VideoOS.Installer.LogServerWarningPlugin.Properties.VideoOS.LogServer.LogServerWarningPlugin", typeof(VideoOS_LogServer_LogServerWarningPlugin).Assembly);
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

	public static string BodyTextMoreSecureLog => ResourceManager.GetString("BodyTextMoreSecureLog", resourceCulture);

	public static string BodyTextNoBackwardCompability => ResourceManager.GetString("BodyTextNoBackwardCompability", resourceCulture);

	public static string DetailTextMoreSecureLog => ResourceManager.GetString("DetailTextMoreSecureLog", resourceCulture);

	public static string DetailTextNoBackwardCompability => ResourceManager.GetString("DetailTextNoBackwardCompability", resourceCulture);

	public static string DUMMY_TEST_STRING => ResourceManager.GetString("DUMMY_TEST_STRING", resourceCulture);

	public static string HeadlineMoreSecureLog => ResourceManager.GetString("HeadlineMoreSecureLog", resourceCulture);

	public static string HeadlineNoBackwardCompability => ResourceManager.GetString("HeadlineNoBackwardCompability", resourceCulture);

	public static string HelpText => ResourceManager.GetString("HelpText", resourceCulture);

	public static string SubHeading => ResourceManager.GetString("SubHeading", resourceCulture);

	public VideoOS_LogServer_LogServerWarningPlugin()
	{
	}
}
