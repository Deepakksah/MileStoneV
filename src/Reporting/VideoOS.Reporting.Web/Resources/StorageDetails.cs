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
public class StorageDetails
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
				ResourceManager resourceManager = new ResourceManager("Resources.StorageDetails", Assembly.Load("App_GlobalResources"));
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

	public static string BankNameRec => ResourceManager.GetString("BankNameRec", resourceCulture);

	public static string Header => ResourceManager.GetString("Header", resourceCulture);

	public static string Name => ResourceManager.GetString("Name", resourceCulture);

	public static string PathLbl => ResourceManager.GetString("PathLbl", resourceCulture);

	public static string SizeLbl => ResourceManager.GetString("SizeLbl", resourceCulture);

	public static string UsedLbl => ResourceManager.GetString("UsedLbl", resourceCulture);

	public StorageDetails()
	{
	}
}
