using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace VideoOS.Installer.DotNetHostingPlugin.Properties;

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
				resourceMan = new ResourceManager("VideoOS.Installer.DotNetHostingPlugin.Properties.Resources", typeof(Resources).Assembly);
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

	public static string DotNetHosting_InstallationException => ResourceManager.GetString("DotNetHosting_InstallationException", resourceCulture);

	public static string DotNetHosting_Save => ResourceManager.GetString("DotNetHosting_Save", resourceCulture);

	public static string DotNetHosting_StartInstallation => ResourceManager.GetString("DotNetHosting_StartInstallation", resourceCulture);

	public Resources()
	{
	}
}
