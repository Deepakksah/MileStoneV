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
public class SystemOverview
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
				ResourceManager resourceManager = new ResourceManager("Resources.SystemOverview", Assembly.Load("App_GlobalResources"));
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

	public static string EventServer => ResourceManager.GetString("EventServer", resourceCulture);

	public static string LogServer => ResourceManager.GetString("LogServer", resourceCulture);

	public static string LPRServer => ResourceManager.GetString("LPRServer", resourceCulture);

	public static string ManagementServer => ResourceManager.GetString("ManagementServer", resourceCulture);

	public static string MenuItemAdditionalServers => ResourceManager.GetString("MenuItemAdditionalServers", resourceCulture);

	public static string MenuItemCameras => ResourceManager.GetString("MenuItemCameras", resourceCulture);

	public static string MenuItemDisks => ResourceManager.GetString("MenuItemDisks", resourceCulture);

	public static string MenuItemFailoverServers => ResourceManager.GetString("MenuItemFailoverServers", resourceCulture);

	public static string MenuItemNetwork => ResourceManager.GetString("MenuItemNetwork", resourceCulture);

	public static string MenuItemRecordingServers => ResourceManager.GetString("MenuItemRecordingServers", resourceCulture);

	public static string MenuItemStorages => ResourceManager.GetString("MenuItemStorages", resourceCulture);

	public static string SelectCameraGroup => ResourceManager.GetString("SelectCameraGroup", resourceCulture);

	public static string SQLServer => ResourceManager.GetString("SQLServer", resourceCulture);

	public static string SystemOverviewHeader => ResourceManager.GetString("SystemOverviewHeader", resourceCulture);

	public static string UnknownServer => ResourceManager.GetString("UnknownServer", resourceCulture);

	public SystemOverview()
	{
	}
}
