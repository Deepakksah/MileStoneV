using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace VideoOS.Installer.SafeKitAdapter;

[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
[DebuggerNonUserCode]
[CompilerGenerated]
public class SafeKitAdapterResources
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
				resourceMan = new ResourceManager("VideoOS.Installer.SafeKitAdapter.SafeKitAdapterResources", typeof(SafeKitAdapterResources).Assembly);
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

	public static string FailoverInstallerName => ResourceManager.GetString("FailoverInstallerName", resourceCulture);

	public static string FailoverPackageName => ResourceManager.GetString("FailoverPackageName", resourceCulture);

	public static string FailoverTransformName => ResourceManager.GetString("FailoverTransformName", resourceCulture);

	public SafeKitAdapterResources()
	{
	}
}
