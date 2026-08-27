using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace VideoOS.Management.VmoClient.Properties;

[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
[DebuggerNonUserCode]
[CompilerGenerated]
public class VmoMessageParamsResources
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
				resourceMan = new ResourceManager("VideoOS.Management.VmoClient.Properties.VmoMessageParamsResources", typeof(VmoMessageParamsResources).Assembly);
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

	public static string RecordingServerServiceControllerExceptions_Operation_Pause => ResourceManager.GetString("RecordingServerServiceControllerExceptions_Operation_Pause", resourceCulture);

	public static string RecordingServerServiceControllerExceptions_Operation_Resume => ResourceManager.GetString("RecordingServerServiceControllerExceptions_Operation_Resume", resourceCulture);

	public static string RecordingServerServiceControllerExceptions_Operation_Start => ResourceManager.GetString("RecordingServerServiceControllerExceptions_Operation_Start", resourceCulture);

	public static string RecordingServerServiceControllerExceptions_Operation_Stop => ResourceManager.GetString("RecordingServerServiceControllerExceptions_Operation_Stop", resourceCulture);

	public static string RecordingServerServiceControllerExceptions_Status_ContinuePending => ResourceManager.GetString("RecordingServerServiceControllerExceptions_Status_ContinuePending", resourceCulture);

	public static string RecordingServerServiceControllerExceptions_Status_Paused => ResourceManager.GetString("RecordingServerServiceControllerExceptions_Status_Paused", resourceCulture);

	public static string RecordingServerServiceControllerExceptions_Status_PausePending => ResourceManager.GetString("RecordingServerServiceControllerExceptions_Status_PausePending", resourceCulture);

	public static string RecordingServerServiceControllerExceptions_Status_Running => ResourceManager.GetString("RecordingServerServiceControllerExceptions_Status_Running", resourceCulture);

	public static string RecordingServerServiceControllerExceptions_Status_StartPending => ResourceManager.GetString("RecordingServerServiceControllerExceptions_Status_StartPending", resourceCulture);

	public static string RecordingServerServiceControllerExceptions_Status_Stopped => ResourceManager.GetString("RecordingServerServiceControllerExceptions_Status_Stopped", resourceCulture);

	public static string RecordingServerServiceControllerExceptions_Status_StopPending => ResourceManager.GetString("RecordingServerServiceControllerExceptions_Status_StopPending", resourceCulture);

	public VmoMessageParamsResources()
	{
	}
}
