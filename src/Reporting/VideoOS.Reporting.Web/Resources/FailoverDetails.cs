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
public class FailoverDetails
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
				ResourceManager resourceManager = new ResourceManager("Resources.FailoverDetails", Assembly.Load("App_GlobalResources"));
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

	public static string ColumnMonitoredRecordingServerCommunicationErrors => ResourceManager.GetString("ColumnMonitoredRecordingServerCommunicationErrors", resourceCulture);

	public static string ColumnMonitoredRecordingServerName => ResourceManager.GetString("ColumnMonitoredRecordingServerName", resourceCulture);

	public static string ColumnMonitoredRecordingServerPosition => ResourceManager.GetString("ColumnMonitoredRecordingServerPosition", resourceCulture);

	public static string FailoverIncidents => ResourceManager.GetString("FailoverIncidents", resourceCulture);

	public static string FailoverStartHeader => ResourceManager.GetString("FailoverStartHeader", resourceCulture);

	public static string FailoverStopHeader => ResourceManager.GetString("FailoverStopHeader", resourceCulture);

	public static string MonitoredRecordingServersHeader => ResourceManager.GetString("MonitoredRecordingServersHeader", resourceCulture);

	public static string RecorderHeader => ResourceManager.GetString("RecorderHeader", resourceCulture);

	public static string RecordingServerCommunicationErrorAuthorization => ResourceManager.GetString("RecordingServerCommunicationErrorAuthorization", resourceCulture);

	public static string RecordingServerCommunicationErrorNone => ResourceManager.GetString("RecordingServerCommunicationErrorNone", resourceCulture);

	public static string RecordingServerCommunicationErrorSsl => ResourceManager.GetString("RecordingServerCommunicationErrorSsl", resourceCulture);

	public FailoverDetails()
	{
	}
}
