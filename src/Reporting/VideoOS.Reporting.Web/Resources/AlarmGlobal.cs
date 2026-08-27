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
public class AlarmGlobal
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
				ResourceManager resourceManager = new ResourceManager("Resources.AlarmGlobal", Assembly.Load("App_GlobalResources"));
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

	public static string DateFormat => ResourceManager.GetString("DateFormat", resourceCulture);

	public static string Day => ResourceManager.GetString("Day", resourceCulture);

	public static string Incomplete => ResourceManager.GetString("Incomplete", resourceCulture);

	public static string Month => ResourceManager.GetString("Month", resourceCulture);

	public static string SixMonth => ResourceManager.GetString("SixMonth", resourceCulture);

	public static string TimeFormat => ResourceManager.GetString("TimeFormat", resourceCulture);

	public static string UnrespSvr => ResourceManager.GetString("UnrespSvr", resourceCulture);

	public static string Week => ResourceManager.GetString("Week", resourceCulture);

	public static string Year => ResourceManager.GetString("Year", resourceCulture);

	public AlarmGlobal()
	{
	}
}
