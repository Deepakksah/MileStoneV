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
public class DiskDetails
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
				ResourceManager resourceManager = new ResourceManager("Resources.DiskDetails", Assembly.Load("App_GlobalResources"));
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

	public static string Drive => ResourceManager.GetString("Drive", resourceCulture);

	public static string FreeOf => ResourceManager.GetString("FreeOf", resourceCulture);

	public static string FreeSpace => ResourceManager.GetString("FreeSpace", resourceCulture);

	public static string Header => ResourceManager.GetString("Header", resourceCulture);

	public static string MediaRecording => ResourceManager.GetString("MediaRecording", resourceCulture);

	public static string OtherData => ResourceManager.GetString("OtherData", resourceCulture);

	public DiskDetails()
	{
	}
}
