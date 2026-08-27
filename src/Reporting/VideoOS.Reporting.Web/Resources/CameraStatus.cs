using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Resources;

[GeneratedCode("Microsoft.VisualStudio.Web.Application.StronglyTypedResourceProxyBuilder", "16.0.0.0")]
[DebuggerNonUserCode]
[CompilerGenerated]
public class CameraStatus
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
				ResourceManager resourceManager = new ResourceManager("Resources.CameraStatus", Assembly.Load("App_GlobalResources"));
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

	public static string LiveFpsDefaultHeader => ResourceManager.GetString("LiveFpsDefaultHeader", resourceCulture);

	public static string LiveVideoFormatHeader => ResourceManager.GetString("LiveVideoFormatHeader", resourceCulture);

	public static string MediaReceivedHeader => ResourceManager.GetString("MediaReceivedHeader", resourceCulture);

	public static string RecordingFps => ResourceManager.GetString("RecordingFps", resourceCulture);

	public static string RecordingHeader => ResourceManager.GetString("RecordingHeader", resourceCulture);

	public static string RecordingVideoFormatHeader => ResourceManager.GetString("RecordingVideoFormatHeader", resourceCulture);

	public static string StatusDisabled => ResourceManager.GetString("StatusDisabled", resourceCulture);

	public static string StatusError => ResourceManager.GetString("StatusError", resourceCulture);

	public static string StatusHeader => ResourceManager.GetString("StatusHeader", resourceCulture);

	public static string StatusOk => ResourceManager.GetString("StatusOk", resourceCulture);

	public static string StatusStopped => ResourceManager.GetString("StatusStopped", resourceCulture);

	public static string StatusUnknown => ResourceManager.GetString("StatusUnknown", resourceCulture);

	public static string StorageHeader => ResourceManager.GetString("StorageHeader", resourceCulture);

	public static string Total => ResourceManager.GetString("Total", resourceCulture);

	public static string UsedSpaceHeader => ResourceManager.GetString("UsedSpaceHeader", resourceCulture);

	public static string VideoFormatH263 => ResourceManager.GetString("VideoFormatH263", resourceCulture);

	public static string VideoFormatH264 => ResourceManager.GetString("VideoFormatH264", resourceCulture);

	public static string VideoFormatH265 => ResourceManager.GetString("VideoFormatH265", resourceCulture);

	public static string VideoFormatJpeg => ResourceManager.GetString("VideoFormatJpeg", resourceCulture);

	public static string VideoFormatMpeg => ResourceManager.GetString("VideoFormatMpeg", resourceCulture);

	public static string VideoFormatMpeg4 => ResourceManager.GetString("VideoFormatMpeg4", resourceCulture);

	public static string VideoFormatUnknown => ResourceManager.GetString("VideoFormatUnknown", resourceCulture);

	public static string VideoFormatVC1 => ResourceManager.GetString("VideoFormatVC1", resourceCulture);

	public CameraStatus()
	{
	}
}
