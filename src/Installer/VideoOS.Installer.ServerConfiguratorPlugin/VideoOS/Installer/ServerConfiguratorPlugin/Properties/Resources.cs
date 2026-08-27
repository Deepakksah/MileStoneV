using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace VideoOS.Installer.ServerConfiguratorPlugin.Properties;

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
				resourceMan = new ResourceManager("VideoOS.Installer.ServerConfiguratorPlugin.Properties.Resources", typeof(Resources).Assembly);
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

	public static string ConfigurationPlugin_ErrorOccured => ResourceManager.GetString("ConfigurationPlugin_ErrorOccured", resourceCulture);

	public static string WarningPageBodyDetails => ResourceManager.GetString("WarningPageBodyDetails", resourceCulture);

	public static string WarningPageBodySubsection => ResourceManager.GetString("WarningPageBodySubsection", resourceCulture);

	public static string WarningPageHeadingDetails => ResourceManager.GetString("WarningPageHeadingDetails", resourceCulture);

	public static string WarningPageHeadingSubsection => ResourceManager.GetString("WarningPageHeadingSubsection", resourceCulture);

	public Resources()
	{
	}
}
