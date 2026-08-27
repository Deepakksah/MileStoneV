using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace VideoOS.Platform.SDK.Export.Resources;

[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
[DebuggerNonUserCode]
[CompilerGenerated]
public class Strings
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
				resourceMan = new ResourceManager("VideoOS.Platform.SDK.Export.Resources.Strings", typeof(Strings).Assembly);
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

	public static string AVIExporterError_ExportAlreadyRunning => ResourceManager.GetString("AVIExporterError_ExportAlreadyRunning", resourceCulture);

	public static string AVIExporterError_FailedToCreateCameraConfigurationXML => ResourceManager.GetString("AVIExporterError_FailedToCreateCameraConfigurationXML", resourceCulture);

	public static string AVIExporterError_FilenameNotSet => ResourceManager.GetString("AVIExporterError_FilenameNotSet", resourceCulture);

	public static string AVIExporterError_NoExportSourcesSet => ResourceManager.GetString("AVIExporterError_NoExportSourcesSet", resourceCulture);

	public static string AVIExporterError_NoSequencesProvided => ResourceManager.GetString("AVIExporterError_NoSequencesProvided", resourceCulture);

	public static string AVIExporterError_NotAuthorized => ResourceManager.GetString("AVIExporterError_NotAuthorized", resourceCulture);

	public static string AVIExporterError_NoVideoInSelectedTimePeriod => ResourceManager.GetString("AVIExporterError_NoVideoInSelectedTimePeriod", resourceCulture);

	public static string AVIExporterError_NoVideoInSelectedTimePeriodForCamera => ResourceManager.GetString("AVIExporterError_NoVideoInSelectedTimePeriodForCamera", resourceCulture);

	public static string AVIExporterError_OverlayImageInvalidPosition => ResourceManager.GetString("AVIExporterError_OverlayImageInvalidPosition", resourceCulture);

	public static string AVIExporterError_OverlayImageNotSet => ResourceManager.GetString("AVIExporterError_OverlayImageNotSet", resourceCulture);

	public static string AVIExporterError_RecorderOffline => ResourceManager.GetString("AVIExporterError_RecorderOffline", resourceCulture);

	public static string AVIExporterError_RecorderOfflineForCamera => ResourceManager.GetString("AVIExporterError_RecorderOfflineForCamera", resourceCulture);

	public static string BookmarkExporterError_ServerCommunicationErrorDuringBookmarkExport => ResourceManager.GetString("BookmarkExporterError_ServerCommunicationErrorDuringBookmarkExport", resourceCulture);

	public static string BookmarkExporterError_UnknownErrorDuringBookmarkExport => ResourceManager.GetString("BookmarkExporterError_UnknownErrorDuringBookmarkExport", resourceCulture);

	public static string BookmarkExporterError_WriteErrorDuringBookmarkExport => ResourceManager.GetString("BookmarkExporterError_WriteErrorDuringBookmarkExport", resourceCulture);

	public static string DBExporterError_ExportFailed => ResourceManager.GetString("DBExporterError_ExportFailed", resourceCulture);

	public Strings()
	{
	}
}
