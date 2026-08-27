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
public class ServerOverview
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
				ResourceManager resourceManager = new ResourceManager("Resources.ServerOverview", Assembly.Load("App_GlobalResources"));
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

	public static string CPU => ResourceManager.GetString("CPU", resourceCulture);

	public static string DiskReadSpeed => ResourceManager.GetString("DiskReadSpeed", resourceCulture);

	public static string HostName => ResourceManager.GetString("HostName", resourceCulture);

	public static string Memory => ResourceManager.GetString("Memory", resourceCulture);

	public static string NetworkLoad => ResourceManager.GetString("NetworkLoad", resourceCulture);

	public static string ServerType => ResourceManager.GetString("ServerType", resourceCulture);

	public static string StatusDetailsAttachPending => ResourceManager.GetString("StatusDetailsAttachPending", resourceCulture);

	public static string StatusDetailsConnectedToManagementServer => ResourceManager.GetString("StatusDetailsConnectedToManagementServer", resourceCulture);

	public static string StatusDetailsConnectingToManagementServer => ResourceManager.GetString("StatusDetailsConnectingToManagementServer", resourceCulture);

	public static string StatusDetailsDisconnectedFromManagementServer => ResourceManager.GetString("StatusDetailsDisconnectedFromManagementServer", resourceCulture);

	public static string StatusDetailsHeader => ResourceManager.GetString("StatusDetailsHeader", resourceCulture);

	public static string StatusDetailsReconnectingToManagementServer => ResourceManager.GetString("StatusDetailsReconnectingToManagementServer", resourceCulture);

	public static string StatusDetailsServerUnavailable => ResourceManager.GetString("StatusDetailsServerUnavailable", resourceCulture);

	public static string StatusFailoverCommunicationError => ResourceManager.GetString("StatusFailoverCommunicationError", resourceCulture);

	public static string StatusFailoverFatalError => ResourceManager.GetString("StatusFailoverFatalError", resourceCulture);

	public static string StatusFailoverInFailover => ResourceManager.GetString("StatusFailoverInFailover", resourceCulture);

	public static string StatusFailoverStarting => ResourceManager.GetString("StatusFailoverStarting", resourceCulture);

	public static string StatusFailoverStopped => ResourceManager.GetString("StatusFailoverStopped", resourceCulture);

	public static string StatusFailoverTooltipAuthorizationError => ResourceManager.GetString("StatusFailoverTooltipAuthorizationError", resourceCulture);

	public static string StatusFailoverTooltipSslError => ResourceManager.GetString("StatusFailoverTooltipSslError", resourceCulture);

	public static string StatusFailoverUnknown => ResourceManager.GetString("StatusFailoverUnknown", resourceCulture);

	public static string StatusFailoverWaitingWatching => ResourceManager.GetString("StatusFailoverWaitingWatching", resourceCulture);

	public static string StatusHistory => ResourceManager.GetString("StatusHistory", resourceCulture);

	public static string StatusOverallError => ResourceManager.GetString("StatusOverallError", resourceCulture);

	public static string StatusOverallHeader => ResourceManager.GetString("StatusOverallHeader", resourceCulture);

	public static string StatusOverallOk => ResourceManager.GetString("StatusOverallOk", resourceCulture);

	public static string StatusOverallUnkonwn => ResourceManager.GetString("StatusOverallUnkonwn", resourceCulture);

	public ServerOverview()
	{
	}
}
