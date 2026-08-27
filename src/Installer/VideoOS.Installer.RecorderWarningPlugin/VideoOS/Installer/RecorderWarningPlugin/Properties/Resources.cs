using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace VideoOS.Installer.RecorderWarningPlugin.Properties;

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
				resourceMan = new ResourceManager("VideoOS.Installer.RecorderWarningPlugin.Properties.Resources", typeof(Resources).Assembly);
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

	public static string StringCheckBoxAccept => ResourceManager.GetString("StringCheckBoxAccept", resourceCulture);

	public static string StringHeading => ResourceManager.GetString("StringHeading", resourceCulture);

	public static string StringNextAction => ResourceManager.GetString("StringNextAction", resourceCulture);

	public static string StringReason => ResourceManager.GetString("StringReason", resourceCulture);

	public static string StringRecorderKBLinkDescription => ResourceManager.GetString("StringRecorderKBLinkDescription", resourceCulture);

	public static string WarningPageBodyDetails => ResourceManager.GetString("WarningPageBodyDetails", resourceCulture);

	public static string WarningPageBodySubsection => ResourceManager.GetString("WarningPageBodySubsection", resourceCulture);

	public static string WarningPageHeadingDetails => ResourceManager.GetString("WarningPageHeadingDetails", resourceCulture);

	public static string WarningPageHeadingSubsection => ResourceManager.GetString("WarningPageHeadingSubsection", resourceCulture);

	public Resources()
	{
	}
}
