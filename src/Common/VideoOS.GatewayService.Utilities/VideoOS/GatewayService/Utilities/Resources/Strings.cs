using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace VideoOS.GatewayService.Utilities.Resources;

[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
[DebuggerNonUserCode]
[CompilerGenerated]
public class Strings
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
				resourceMan = new ResourceManager("VideoOS.GatewayService.Utilities.Resources.Strings", typeof(Strings).Assembly);
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

	public static string MgmtSrv_unsecure_warning_details => ResourceManager.GetString("MgmtSrv_unsecure_warning_details", resourceCulture);

	public static string MgmtSrv_unsecure_warning_subsection => ResourceManager.GetString("MgmtSrv_unsecure_warning_subsection", resourceCulture);

	public static string Warning => ResourceManager.GetString("Warning", resourceCulture);

	public static string What_to_do => ResourceManager.GetString("What_to_do", resourceCulture);

	public Strings()
	{
	}
}
