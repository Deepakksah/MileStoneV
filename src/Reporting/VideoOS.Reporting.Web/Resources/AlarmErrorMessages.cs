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
public class AlarmErrorMessages
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
				ResourceManager resourceManager = new ResourceManager("Resources.AlarmErrorMessages", Assembly.Load("App_GlobalResources"));
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

	public static string ErrorMsg1 => ResourceManager.GetString("ErrorMsg1", resourceCulture);

	public static string ErrorMsg2 => ResourceManager.GetString("ErrorMsg2", resourceCulture);

	public static string ErrorMsg3 => ResourceManager.GetString("ErrorMsg3", resourceCulture);

	public static string ErrorMsg4 => ResourceManager.GetString("ErrorMsg4", resourceCulture);

	public static string ErrorMsg5 => ResourceManager.GetString("ErrorMsg5", resourceCulture);

	public static string ErrorMsg6 => ResourceManager.GetString("ErrorMsg6", resourceCulture);

	public static string ErrorMsg7 => ResourceManager.GetString("ErrorMsg7", resourceCulture);

	public static string ErrorMsg8 => ResourceManager.GetString("ErrorMsg8", resourceCulture);

	public AlarmErrorMessages()
	{
	}
}
