using System.Collections.Generic;
using VideoOS.Management.Server.TaskSecurityProviders;

namespace VideoOS.Management.Server;

public class TaskTypeInformation
{
	public static readonly IReadOnlyDictionary<Task.TaskType, TaskTypeInformation> TaskTypesDictionary = new Dictionary<Task.TaskType, TaskTypeInformation>
	{
		{
			Task.TaskType.AddHardware,
			new TaskTypeInformation(Task.TaskType.AddHardware, Task.RetentionPolicy.Default, "TaskAddHardware", "System.AddHardware", new TaskAddHardwareSecurityProvider())
		},
		{
			Task.TaskType.DeleteHardware,
			new TaskTypeInformation(Task.TaskType.DeleteHardware, Task.RetentionPolicy.Default, "TaskDeleteHardware", "System.DeleteHardware", new TaskDeleteHardwareSecurityProvider())
		},
		{
			Task.TaskType.UpdateHardware,
			new TaskTypeInformation(Task.TaskType.UpdateHardware, Task.RetentionPolicy.Default, "TaskUpdateHardware", "System.UpdateHardware", new TaskUpdateHardwareSecurityProvider())
		},
		{
			Task.TaskType.ReplaceHardware,
			new TaskTypeInformation(Task.TaskType.ReplaceHardware, Task.RetentionPolicy.Default, "TaskReplaceHardware", "System.ReplaceHardware", new TaskReplaceHardwareSecurityProvider())
		},
		{
			Task.TaskType.MoveHardware,
			new TaskTypeInformation(Task.TaskType.MoveHardware, Task.RetentionPolicy.Default, "TaskMoveHardware", "System.MoveHardware", new TaskMoveHardwareSecurityProvider())
		},
		{
			Task.TaskType.HardwareScan,
			new TaskTypeInformation(Task.TaskType.HardwareScan, Task.RetentionPolicy.Default, "TaskHardwareScan", "System.HardwareScan", new TaskHardwareScanSecurityProvider())
		},
		{
			Task.TaskType.HardwareScanExpress,
			new TaskTypeInformation(Task.TaskType.HardwareScanExpress, Task.RetentionPolicy.Default, "TaskHardwareScanExpress", "System.HardwareScanExpress", new TaskHardwareScanSecurityProvider())
		},
		{
			Task.TaskType.ChangeHardwarePassword,
			new TaskTypeInformation(Task.TaskType.ChangeHardwarePassword, Task.RetentionPolicy.Default, "TaskChangeHardwarePassword", "System.ChangeHardwarePassword", new TaskChangeHardwarePasswordSecurityProvider())
		},
		{
			Task.TaskType.ActivateLicense,
			new TaskTypeInformation(Task.TaskType.ActivateLicense, Task.RetentionPolicy.Default, "TaskActivateLicense", "System.ActivateLicense", new TaskActivateLicenseSecurityProvider())
		},
		{
			Task.TaskType.TruncateDeviceStorage,
			new TaskTypeInformation(Task.TaskType.TruncateDeviceStorage, Task.RetentionPolicy.Default, "TaskTruncateDeviceStorage", "Database.TruncateDeviceStorage", new TaskTrucateDeviceStorageSecurityProvider())
		},
		{
			Task.TaskType.MoveRecordingStorage,
			new TaskTypeInformation(Task.TaskType.MoveRecordingStorage, Task.RetentionPolicy.Default, "TaskMoveRecordingStorage", "Database.MoveStorage", new TaskStorageSecurityProvider())
		},
		{
			Task.TaskType.MoveArchive,
			new TaskTypeInformation(Task.TaskType.MoveArchive, Task.RetentionPolicy.Default, "TaskMoveArchiveStorage", "Database.MoveArchive", new TaskStorageSecurityProvider())
		},
		{
			Task.TaskType.UnmountArchive,
			new TaskTypeInformation(Task.TaskType.UnmountArchive, Task.RetentionPolicy.Default, "TaskUnmountArchiveStorage", "Database.UnmountArchive", new TaskStorageSecurityProvider())
		},
		{
			Task.TaskType.MountArchive,
			new TaskTypeInformation(Task.TaskType.MountArchive, Task.RetentionPolicy.Default, "TaskMountArchiveStorage", "Database.MountArchive", new TaskStorageSecurityProvider())
		},
		{
			Task.TaskType.DeleteArchive,
			new TaskTypeInformation(Task.TaskType.DeleteArchive, Task.RetentionPolicy.Default, "TaskDeleteArchive", "Database.DeleteArchive", new TaskStorageSecurityProvider())
		},
		{
			Task.TaskType.DeleteStorage,
			new TaskTypeInformation(Task.TaskType.DeleteStorage, Task.RetentionPolicy.Default, "TaskDeleteStorage", "Database.DeleteStorage", new TaskStorageSecurityProvider())
		},
		{
			Task.TaskType.StorageRetrieval,
			new TaskTypeInformation(Task.TaskType.StorageRetrieval, Task.RetentionPolicy.KeepForever, "TaskRequestEdgeStorage", "Edge.StorageRetrieval", new TaskEdgeStorageRetrivalSecurityProvider())
		},
		{
			Task.TaskType.MoveDeviceDb,
			new TaskTypeInformation(Task.TaskType.MoveDeviceDb, Task.RetentionPolicy.Default, "TaskMoveDevice", "Database.MoveDeviceDb", new TaskStorageSecurityProvider())
		},
		{
			Task.TaskType.SetEncryptionMethod,
			new TaskTypeInformation(Task.TaskType.SetEncryptionMethod, Task.RetentionPolicy.Default, "TaskSetEncryptionMethod", "Database.SetEncryptionMethod", new TaskStorageSecurityProvider())
		},
		{
			Task.TaskType.SetEncryptionPassword,
			new TaskTypeInformation(Task.TaskType.SetEncryptionPassword, Task.RetentionPolicy.Default, "TaskSetEncryptionPassword", "Database.SetEncryptionPassword", new TaskStorageSecurityProvider())
		},
		{
			Task.TaskType.Archiving,
			new TaskTypeInformation(Task.TaskType.Archiving, Task.RetentionPolicy.KeepForever, "TaskArchiving", "Database.Archiving", new TaskStorageSecurityProvider())
		},
		{
			Task.TaskType.Repairing,
			new TaskTypeInformation(Task.TaskType.Repairing, Task.RetentionPolicy.KeepForever, "TaskRepairing", "Database.Repairing2", new TaskStorageSecurityProvider())
		},
		{
			Task.TaskType.FirmwareUpgrade,
			new TaskTypeInformation(Task.TaskType.FirmwareUpgrade, Task.RetentionPolicy.Default, "TaskFirmwareUpgrade", "System.FirmwareUpgrade", new TaskFirmwareUpgradeSecurityProvider())
		},
		{
			Task.TaskType.AddIdpAdmin,
			new TaskTypeInformation(Task.TaskType.AddIdpAdmin, Task.RetentionPolicy.Default, "TaskAddIdpAdmin", "System.AddIdpAdmin", new TaskAddIdpAdminSecurityProvider())
		},
		{
			Task.TaskType.DeleteIdpAdmin,
			new TaskTypeInformation(Task.TaskType.DeleteIdpAdmin, Task.RetentionPolicy.Default, "TaskDeleteIdpAdmin", "System.DeleteIdpAdmin", new TaskDeleteIdpAdminSecurityProvider())
		},
		{
			Task.TaskType.DeleteIdpUser,
			new TaskTypeInformation(Task.TaskType.DeleteIdpUser, Task.RetentionPolicy.Default, "TaskDeleteIdpUser", "System.DeleteIdpUser", new TaskDeleteIdpUserSecurityProvider())
		},
		{
			Task.TaskType.GetDevicePresets,
			new TaskTypeInformation(Task.TaskType.GetDevicePresets, Task.RetentionPolicy.KeepForever, "TaskGetDevicePresets", "System.GetDevicePresets", new TaskGetDevicePresetsSecurityProvider())
		},
		{
			Task.TaskType.DeleteRecorder,
			new TaskTypeInformation(Task.TaskType.DeleteRecorder, Task.RetentionPolicy.Default, "TaskDeleteRecorder", "System.DeleteRecorder", new TaskDeleteRecorderSecurityProvider())
		}
	};

	public Task.TaskType TaskType { get; private set; }

	public string EntityName { get; private set; }

	public string DatabaseString { get; private set; }

	public ITaskSecurityProvider TaskSecurityProvider { get; private set; }

	public Task.RetentionPolicy RetentionPolicy { get; private set; }

	private TaskTypeInformation(Task.TaskType taskType, Task.RetentionPolicy retentionPolicy, string entityName, string databaseString, ITaskSecurityProvider taskSecurityProvider)
	{
		TaskType = taskType;
		EntityName = entityName;
		DatabaseString = databaseString;
		TaskSecurityProvider = taskSecurityProvider;
		RetentionPolicy = retentionPolicy;
	}
}
