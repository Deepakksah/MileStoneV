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
public class ErrorPage
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
				ResourceManager resourceManager = new ResourceManager("Resources.ErrorPage", Assembly.Load("App_GlobalResources"));
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

	public static string DefaultError => ResourceManager.GetString("DefaultError", resourceCulture);

	public static string Error => ResourceManager.GetString("Error", resourceCulture);

	public static string ErrorDefaultDescriptionValue => ResourceManager.GetString("ErrorDefaultDescriptionValue", resourceCulture);

	public static string ErrorDescription => ResourceManager.GetString("ErrorDescription", resourceCulture);

	public static string ErrorDetails => ResourceManager.GetString("ErrorDetails", resourceCulture);

	public static string ErrorHeader => ResourceManager.GetString("ErrorHeader", resourceCulture);

	public static string GeneralError_ReportServerError => ResourceManager.GetString("GeneralError_ReportServerError", resourceCulture);

	public static string GeneralError_ServiceInitiationFailed => ResourceManager.GetString("GeneralError_ServiceInitiationFailed", resourceCulture);

	public static string GeneralError_SystemError => ResourceManager.GetString("GeneralError_SystemError", resourceCulture);

	public static string SecurityError_UserNotAuthenticated => ResourceManager.GetString("SecurityError_UserNotAuthenticated", resourceCulture);

	public static string SecurityError_UserNotAuthorised => ResourceManager.GetString("SecurityError_UserNotAuthorised", resourceCulture);

	public ErrorPage()
	{
	}
}
