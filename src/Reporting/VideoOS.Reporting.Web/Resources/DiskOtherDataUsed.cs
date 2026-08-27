using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Resources;

[GeneratedCode("Microsoft.VisualStudio.Web.Application.StronglyTypedResourceProxyBuilder", "15.0.0.0")]
[DebuggerNonUserCode]
[CompilerGenerated]
public class DiskOtherDataUsed
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
				ResourceManager resourceManager = new ResourceManager("Resources.DiskOtherDataUsed", Assembly.Load("App_GlobalResources"));
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

	public static string AllInstances => ResourceManager.GetString("AllInstances", resourceCulture);

	public static string ChartTitle => ResourceManager.GetString("ChartTitle", resourceCulture);

	public static string ChooseInstance => ResourceManager.GetString("ChooseInstance", resourceCulture);

	public static string DataTitle => ResourceManager.GetString("DataTitle", resourceCulture);

	public static string Units => ResourceManager.GetString("Units", resourceCulture);

	public DiskOtherDataUsed()
	{
	}
}
