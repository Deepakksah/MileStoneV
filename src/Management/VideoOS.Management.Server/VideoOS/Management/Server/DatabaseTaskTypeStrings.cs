using System;
using System.Linq;

namespace VideoOS.Management.Server;

public class DatabaseTaskTypeStrings
{
	public const string AddHardware = "System.AddHardware";

	public const string MoveHardware = "System.MoveHardware";

	public const string HardwareScan = "System.HardwareScan";

	public const string HardwareScanExpress = "System.HardwareScanExpress";

	public const string DeleteHardware = "System.DeleteHardware";

	public const string UpdateHardware = "System.UpdateHardware";

	public const string ReplaceHardware = "System.ReplaceHardware";

	public const string ChangeHardwarePassword = "System.ChangeHardwarePassword";

	public const string ActivateLicense = "System.ActivateLicense";

	public const string FirmwareUpgrade = "System.FirmwareUpgrade";

	public const string TruncateDeviceStorage = "Database.TruncateDeviceStorage";

	public const string MoveStorage = "Database.MoveStorage";

	public const string MoveArchive = "Database.MoveArchive";

	public const string MountArchive = "Database.MountArchive";

	public const string UnmountArchive = "Database.UnmountArchive";

	public const string DeleteArchive = "Database.DeleteArchive";

	public const string DeleteStorage = "Database.DeleteStorage";

	public const string StorageRetrieval = "Edge.StorageRetrieval";

	public const string MoveDeviceDb = "Database.MoveDeviceDb";

	public const string SetEncryptionMethod = "Database.SetEncryptionMethod";

	public const string SetEncryptionPassword = "Database.SetEncryptionPassword";

	public const string Archiving = "Database.Archiving";

	public const string Repairing = "Database.Repairing2";

	public const string AddIdpAdmin = "System.AddIdpAdmin";

	public const string DeleteIdpAdmin = "System.DeleteIdpAdmin";

	public const string DeleteIdpUser = "System.DeleteIdpUser";

	public const string GetDevicePresets = "System.GetDevicePresets";

	public const string DeleteRecorder = "System.DeleteRecorder";

	public static Task.TaskType TaskTypeDbToTaskType(string taskTypeDb)
	{
		return TaskTypeInformation.TaskTypesDictionary.Values.First((TaskTypeInformation v) => string.Equals(v.DatabaseString, taskTypeDb, StringComparison.Ordinal)).TaskType;
	}

	public static string TaskTypeToTaskTypeDb(Task.TaskType taskType)
	{
		return TaskTypeInformation.TaskTypesDictionary[taskType].DatabaseString;
	}
}
