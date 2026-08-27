using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace VideoOS.Management.VmoClient.BusinessObjects.ServerTask;

[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
[DebuggerNonUserCode]
[CompilerGenerated]
public class TaskResources
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
				resourceMan = new ResourceManager("VideoOS.Management.VmoClient.BusinessObjects.ServerTask.TaskResources", typeof(TaskResources).Assembly);
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

	public static string EntityNameManagementServer => ResourceManager.GetString("EntityNameManagementServer", resourceCulture);

	public static string NameOfUnknownStorage => ResourceManager.GetString("NameOfUnknownStorage", resourceCulture);

	public static string TaskActivateLicense => ResourceManager.GetString("TaskActivateLicense", resourceCulture);

	public static string TaskAddHardware => ResourceManager.GetString("TaskAddHardware", resourceCulture);

	public static string TaskAddIdpAdmin => ResourceManager.GetString("TaskAddIdpAdmin", resourceCulture);

	public static string TaskArchiving => ResourceManager.GetString("TaskArchiving", resourceCulture);

	public static string TaskChangeHardwarePassword => ResourceManager.GetString("TaskChangeHardwarePassword", resourceCulture);

	public static string TaskDeleteArchive => ResourceManager.GetString("TaskDeleteArchive", resourceCulture);

	public static string TaskDeleteHardware => ResourceManager.GetString("TaskDeleteHardware", resourceCulture);

	public static string TaskDeleteIdpAdmin => ResourceManager.GetString("TaskDeleteIdpAdmin", resourceCulture);

	public static string TaskDeleteIdpUser => ResourceManager.GetString("TaskDeleteIdpUser", resourceCulture);

	public static string TaskDeleteRecorder => ResourceManager.GetString("TaskDeleteRecorder", resourceCulture);

	public static string TaskDeleteStorage => ResourceManager.GetString("TaskDeleteStorage", resourceCulture);

	public static string TaskFirmwareUpgrade => ResourceManager.GetString("TaskFirmwareUpgrade", resourceCulture);

	public static string TaskGetDevicePresets => ResourceManager.GetString("TaskGetDevicePresets", resourceCulture);

	public static string TaskHardwareScan => ResourceManager.GetString("TaskHardwareScan", resourceCulture);

	public static string TaskHardwareScanExpress => ResourceManager.GetString("TaskHardwareScanExpress", resourceCulture);

	public static string TaskMountArchiveStorage => ResourceManager.GetString("TaskMountArchiveStorage", resourceCulture);

	public static string TaskMoveArchiveStorage => ResourceManager.GetString("TaskMoveArchiveStorage", resourceCulture);

	public static string TaskMoveDevice => ResourceManager.GetString("TaskMoveDevice", resourceCulture);

	public static string TaskMoveHardware => ResourceManager.GetString("TaskMoveHardware", resourceCulture);

	public static string TaskMoveRecordingStorage => ResourceManager.GetString("TaskMoveRecordingStorage", resourceCulture);

	public static string TaskRepairing => ResourceManager.GetString("TaskRepairing", resourceCulture);

	public static string TaskReplaceHardware => ResourceManager.GetString("TaskReplaceHardware", resourceCulture);

	public static string TaskRetrievingEdgeRecordings => ResourceManager.GetString("TaskRetrievingEdgeRecordings", resourceCulture);

	public static string TaskSetEncryptionMethod => ResourceManager.GetString("TaskSetEncryptionMethod", resourceCulture);

	public static string TaskSetEncryptionMethodInvalidMethod => ResourceManager.GetString("TaskSetEncryptionMethodInvalidMethod", resourceCulture);

	public static string TaskSetEncryptionPassword => ResourceManager.GetString("TaskSetEncryptionPassword", resourceCulture);

	public static string TaskTruncateDeviceStorage => ResourceManager.GetString("TaskTruncateDeviceStorage", resourceCulture);

	public static string TaskUnmountArchiveStorage => ResourceManager.GetString("TaskUnmountArchiveStorage", resourceCulture);

	public static string TaskUpdateHardware => ResourceManager.GetString("TaskUpdateHardware", resourceCulture);

	public TaskResources()
	{
	}
}
