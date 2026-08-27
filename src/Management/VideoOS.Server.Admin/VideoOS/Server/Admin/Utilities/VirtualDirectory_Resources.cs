using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace VideoOS.Server.Admin.Utilities;

[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
[DebuggerNonUserCode]
[CompilerGenerated]
public class VirtualDirectory_Resources
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
				resourceMan = new ResourceManager("VideoOS.Server.Admin.Utilities.VirtualDirectory.Resources", typeof(VirtualDirectory_Resources).Assembly);
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

	public static string Failed_to_enable_ASPNET_for_website_path => ResourceManager.GetString("Failed_to_enable_ASPNET_for_website_path", resourceCulture);

	public static string The_directory_was_not_found => ResourceManager.GetString("The_directory_was_not_found", resourceCulture);

	public static string Virtual_directory_with_name_0_was_not_found => ResourceManager.GetString("Virtual_directory_with_name_0_was_not_found", resourceCulture);

	public VirtualDirectory_Resources()
	{
	}
}
