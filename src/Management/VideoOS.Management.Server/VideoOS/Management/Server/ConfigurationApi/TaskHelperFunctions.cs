using System;
using System.Collections.Generic;
using VideoOS.ConfigurationAPI;
using VideoOS.Management.Server.Properties;
using VideoOS.Management.VmoClient.BusinessObjects.ServerTask;

namespace VideoOS.Management.Server.ConfigurationApi;

public class TaskHelperFunctions
{
	private static readonly Dictionary<Task.TaskType, string> TaskTypeToDisplayName = new Dictionary<Task.TaskType, string>
	{
		{
			Task.TaskType.ActivateLicense,
			TaskResources.TaskActivateLicense
		},
		{
			Task.TaskType.AddHardware,
			TaskResources.TaskAddHardware
		},
		{
			Task.TaskType.AddIdpAdmin,
			TaskResources.TaskAddIdpAdmin
		},
		{
			Task.TaskType.Archiving,
			TaskResources.TaskArchiving
		},
		{
			Task.TaskType.ChangeHardwarePassword,
			TaskResources.TaskChangeHardwarePassword
		},
		{
			Task.TaskType.DeleteArchive,
			TaskResources.TaskDeleteArchive
		},
		{
			Task.TaskType.DeleteHardware,
			TaskResources.TaskDeleteHardware
		},
		{
			Task.TaskType.DeleteIdpAdmin,
			TaskResources.TaskDeleteIdpAdmin
		},
		{
			Task.TaskType.DeleteIdpUser,
			TaskResources.TaskDeleteIdpUser
		},
		{
			Task.TaskType.DeleteRecorder,
			TaskResources.TaskDeleteRecorder
		},
		{
			Task.TaskType.DeleteStorage,
			TaskResources.TaskDeleteStorage
		},
		{
			Task.TaskType.FirmwareUpgrade,
			TaskResources.TaskFirmwareUpgrade
		},
		{
			Task.TaskType.GetDevicePresets,
			TaskResources.TaskGetDevicePresets
		},
		{
			Task.TaskType.HardwareScan,
			TaskResources.TaskHardwareScan
		},
		{
			Task.TaskType.HardwareScanExpress,
			TaskResources.TaskHardwareScanExpress
		},
		{
			Task.TaskType.MountArchive,
			TaskResources.TaskMountArchiveStorage
		},
		{
			Task.TaskType.MoveArchive,
			TaskResources.TaskMoveArchiveStorage
		},
		{
			Task.TaskType.MoveDeviceDb,
			TaskResources.TaskMoveDevice
		},
		{
			Task.TaskType.MoveHardware,
			TaskResources.TaskMoveHardware
		},
		{
			Task.TaskType.MoveRecordingStorage,
			TaskResources.TaskMoveRecordingStorage
		},
		{
			Task.TaskType.Repairing,
			TaskResources.TaskRepairing
		},
		{
			Task.TaskType.ReplaceHardware,
			TaskResources.TaskReplaceHardware
		},
		{
			Task.TaskType.SetEncryptionMethod,
			TaskResources.TaskSetEncryptionMethod
		},
		{
			Task.TaskType.SetEncryptionPassword,
			TaskResources.TaskSetEncryptionPassword
		},
		{
			Task.TaskType.StorageRetrieval,
			TaskResources.TaskRetrievingEdgeRecordings
		},
		{
			Task.TaskType.TruncateDeviceStorage,
			TaskResources.TaskTruncateDeviceStorage
		},
		{
			Task.TaskType.UnmountArchive,
			TaskResources.TaskUnmountArchiveStorage
		},
		{
			Task.TaskType.UpdateHardware,
			TaskResources.TaskUpdateHardware
		}
	};

	public static ConfigurationItem CreateTaskInvokeResultConfigurationItem(Task task, ConfigurationItem item, string displayName = "Result")
	{
		ConfigurationItem configurationItem = CreateInvokeResult(item, displayName, ItemCategories.Item);
		configurationItem.ServerCookie = task.Id.ToString();
		List<Property> list = CreateInitialProperties(item.Properties);
		list.Add(new Property("State", ConfigurationApiResources.InvokeState, ConvertTaskState(task)));
		list.Add(new Property("Path", ConfigurationApiResources.InvokeAddTaskPath, ConfigurationItemPath.CreateItemPath("Task", task.Id).Text)
		{
			ValueType = "Path",
			ItemReference = true
		});
		configurationItem.Properties = list.ToArray();
		return configurationItem;
	}

	public static ConfigurationItem CreateTasksInvokeResultConfigurationItem(IGeminiRequestContext requestContext, Task[] tasks, ConfigurationItem item, string displayName = "Result")
	{
		ConfigurationItem configurationItem = CreateInvokeResult(item, displayName, ItemCategories.Group);
		configurationItem.ChildrenFilled = true;
		List<Property> list = CreateInitialProperties(item.Properties);
		configurationItem.Properties = list.ToArray();
		List<ConfigurationItem> list2 = new List<ConfigurationItem>();
		foreach (Task task in tasks)
		{
			ConfigurationItemPath path = new ConfigurationItemPathBuilder("Task", task.Id).CreatePath();
			try
			{
				ConfigurationItem item2 = TaskConfigurationApi.ConvertTaskToConfigurationItem(requestContext, path, task, ServerInformation.GetServerInformation);
				list2.Add(item2);
			}
			catch (NotSupportedException)
			{
			}
		}
		configurationItem.Children = list2.ToArray();
		return configurationItem;
	}

	private static ConfigurationItem CreateInvokeResult(ConfigurationItem item, string displayName, string itemCategory)
	{
		return new ConfigurationItem
		{
			ItemType = "InvokeResult",
			DisplayName = displayName,
			UIHint = "",
			ItemCategory = itemCategory,
			ParentPath = item.ParentPath,
			Path = item.Path,
			MethodIds = new string[0]
		};
	}

	private static List<Property> CreateInitialProperties(Property[] itemProperties)
	{
		List<Property> list = new List<Property>(itemProperties);
		foreach (Property item in list)
		{
			if (item.UIImportance != UIImportance.Password)
			{
				item.UIImportance = UIImportance.Disabled;
			}
		}
		return list;
	}

	private static string ConvertTaskState(Task task)
	{
		switch (task.AsyncStatus.ExceptionStatus)
		{
		case ExceptionStatus.InProgress:
			return "InProgress";
		case ExceptionStatus.Inactive:
			return "Idle";
		case ExceptionStatus.Failed:
		case ExceptionStatus.Stopping:
		case ExceptionStatus.Stopped:
			return "Error";
		case ExceptionStatus.Succeeded:
			return "Success";
		default:
			return "Unknown";
		}
	}

	public static Property[] CreateTaskResultProperties(IGeminiRequestContext requestContext, Task task)
	{
		return task.Type switch
		{
			Task.TaskType.AddHardware => HardwareTaskResultDefinition.CreateAddHardwareResultProperties(requestContext, task), 
			Task.TaskType.SetEncryptionMethod => RecordingStorageTaskResultDefinition.CreateSetEncryptionMethodProperties(task), 
			Task.TaskType.SetEncryptionPassword => RecordingStorageTaskResultDefinition.CreateSetEncryptionPasswordProperties(task), 
			Task.TaskType.MoveArchive => ArchiveStorageTaskResultDefinition.MoveArchiveStorageMethodProperties(task), 
			Task.TaskType.MountArchive => ArchiveStorageTaskResultDefinition.MountArchiveStorageMethodProperties(task), 
			Task.TaskType.UnmountArchive => ArchiveStorageTaskResultDefinition.UnmountArchiveStorageMethodProperties(task), 
			Task.TaskType.TruncateDeviceStorage => DeviceTaskResultDefinition.TruncateDeviceStorageResultProperties(requestContext, task), 
			Task.TaskType.MoveDeviceDb => DeviceTaskResultDefinition.MoveDeviceStorageProperties(requestContext, task), 
			Task.TaskType.HardwareScan => RecordingServerTaskResultDefinition.CreateHardwareScanResultProperties(requestContext, task), 
			Task.TaskType.MoveHardware => HardwareTaskResultDefinition.CreateMoveHardwareResultProperties(task), 
			Task.TaskType.DeleteHardware => HardwareTaskResultDefinition.CreateDeleteHardwareResultProperties(task), 
			Task.TaskType.UpdateHardware => HardwareTaskResultDefinition.CreateUpdateHardwareResultProperties(task), 
			Task.TaskType.ReplaceHardware => HardwareTaskResultDefinition.CreateReplaceHardwareResultProperties(task), 
			Task.TaskType.FirmwareUpgrade => HardwareTaskResultDefinition.CreateFirmwareUpgradeResultProperties(task), 
			Task.TaskType.GetDevicePresets => GetDevicePresetsTaskResultDefinition.CreateGetDevicePresetsResultProperties(task), 
			Task.TaskType.DeleteRecorder => RecordingServerTaskResultDefinition.CreateDeleteRecorderResultProperties(task), 
			_ => new Property[0], 
		};
	}

	public static ConfigurationItem[] CreateTaskChildItems(IGeminiRequestContext requestContext, Task task, string parentPath, ConfigurationItemPath path)
	{
		switch (task.Type)
		{
		case Task.TaskType.HardwareScanExpress:
			return RecordingServerTaskResultDefinition.CreateHardwareScanExpressResultChildItems(requestContext, task, parentPath, path);
		case Task.TaskType.UpdateHardware:
			return HardwareTaskResultDefinition.CreateUpdateHardwareResultChildItems(task, parentPath, path);
		case Task.TaskType.ReplaceHardware:
			return HardwareTaskResultDefinition.CreateReplaceHardwareResultChildItems(task, path);
		case Task.TaskType.GetDevicePresets:
			if (task.AsyncStatus.ExceptionStatus == ExceptionStatus.Succeeded)
			{
				return GetDevicePresetsTaskResultDefinition.ConvertTaskResponse(task.AsyncStatus.LastException);
			}
			return null;
		case Task.TaskType.DeleteRecorder:
			return RecordingServerTaskResultDefinition.CreateDeleteRecorderResultChildItems(requestContext, task, parentPath, path);
		default:
			return null;
		}
	}

	public static string CreateDisplayName(Task task, string defaultDisplayName = "invokeResult")
	{
		if (TaskTypeToDisplayName.TryGetValue(task.Type, out var value))
		{
			return value;
		}
		return defaultDisplayName;
	}
}
