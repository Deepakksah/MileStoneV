using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VideoOS.Common.Integration.DeviceDiscovery;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

[EntityName("Task")]
[EntityName("TaskAddHardware")]
[EntityName("TaskHardwareScan")]
[EntityName("TaskHardwareScanExpress")]
[EntityName("TaskActivateLicense")]
[EntityName("TaskMoveHardware")]
[EntityName("TaskTruncateDeviceStorage")]
[EntityName("TaskMoveRecordingStorage")]
[EntityName("TaskMoveArchiveStorage")]
[EntityName("TaskMountArchiveStorage")]
[EntityName("TaskUnmountArchiveStorage")]
[EntityName("TaskDeleteStorage")]
[EntityName("TaskDeleteArchive")]
[EntityName("TaskRequestEdgeStorage")]
[EntityName("TaskMoveDevice")]
[EntityName("TaskSetEncryptionMethod")]
[EntityName("TaskSetEncryptionPassword")]
[EntityName("TaskArchiving")]
[EntityName("TaskRepairing")]
[EntityName("TaskDeleteHardware")]
[EntityName("TaskChangeHardwarePassword")]
[EntityName("TaskUpdateHardware")]
[EntityName("TaskReplaceHardware")]
[EntityName("TaskFirmwareUpgrade")]
[EntityName("TaskAddIdpAdmin")]
[EntityName("TaskDeleteIdpAdmin")]
[EntityName("TaskDeleteIdpUser")]
[EntityName("TaskGetDevicePresets")]
public class TaskGeneric : IGenericObjectSupport2
{
	public GenericObject2 QueryObject2(IGeminiRequestContext requestContext, string entityName, string id, string[] propertiesToLoad, int clientVersion)
	{
		Task task = null;
		try
		{
			task = Task.QueryTask(requestContext, id);
		}
		catch (UnauthorizedAccessException)
		{
		}
		return ConvertToGenericObject2(task);
	}

	public GenericObject2[] QueryObjects(IGeminiRequestContext requestContext, string[] ids, string[] propertiesToLoad, int clientVersion)
	{
		return ConvertToGenericObject2s(Task.QueryTasks(requestContext, ids));
	}

	public string[] QueryObjectCollectionIds(IGeminiRequestContext requestContext, string entityName, string parentEntityName, string parentId, string[] propertiesToLoad, int clientVersion)
	{
		string[] result = null;
		if (parentEntityName.Equals("RecordingServer", StringComparison.OrdinalIgnoreCase))
		{
			result = ((!entityName.Equals("Task", StringComparison.OrdinalIgnoreCase)) ? Task.QueryTaskIds(requestContext, EntityNameToTaskType(entityName), parentId) : Task.QueryTaskIds(requestContext, parentId));
		}
		if (parentEntityName.Equals("ManagementServer", StringComparison.Ordinal))
		{
			result = ((!entityName.Equals("Task", StringComparison.Ordinal)) ? Task.QueryTaskIds(requestContext, EntityNameToTaskType(entityName), "909749AE-88C4-4780-A381-383D691E98FA") : Task.QueryTaskIds(requestContext, "909749AE-88C4-4780-A381-383D691E98FA"));
		}
		return result;
	}

	public GenericObject2 CreateObject2(IGeminiRequestContext requestContext, string entityName, ObjectProperty2[] properties, CustomProperty[] customProperties, int clientVersion)
	{
		return entityName switch
		{
			"TaskAddHardware" => CreateAddHardwareTask(requestContext, properties), 
			"TaskHardwareScan" => CreateHardwareScanTask(requestContext, properties), 
			"TaskHardwareScanExpress" => CreateHardwareScanExpressTask(requestContext, properties), 
			"TaskMoveHardware" => CreateMoveHardwareTask(requestContext, properties), 
			"TaskDeleteHardware" => CreateTaskDeleteHardware(requestContext, properties), 
			"TaskChangeHardwarePassword" => CreateTaskChangeHardwarePassword(requestContext, properties), 
			"TaskActivateLicense" => CreateActivateLicenseTask(requestContext, properties), 
			"TaskTruncateDeviceStorage" => CreateTaskTruncateDeviceStorage(requestContext, properties), 
			"TaskMoveRecordingStorage" => CreateTaskMoveRecordingStorage(requestContext, properties), 
			"TaskMoveArchiveStorage" => CreateTaskMoveArchiveStorage(requestContext, properties), 
			"TaskDeleteStorage" => CreateTaskDeleteStorage(requestContext, properties), 
			"TaskDeleteArchive" => CreateTaskDeleteArchive(requestContext, properties), 
			"TaskRequestEdgeStorage" => CreateTaskRequestEdgeStorage(requestContext, properties), 
			"TaskMoveDevice" => CreateTaskMoveDevice(requestContext, properties), 
			"TaskSetEncryptionMethod" => CreateTaskSetEncryptionMethod(requestContext, properties), 
			"TaskSetEncryptionPassword" => CreateTaskSetEncryptionPassword(requestContext, properties), 
			"TaskUpdateHardware" => CreateTaskUpdateHardware(requestContext, properties), 
			"TaskReplaceHardware" => CreateTaskReplaceHardware(requestContext, properties), 
			"TaskFirmwareUpgrade" => CreateTaskFirmwareUpgrade(requestContext, properties), 
			"TaskAddIdpAdmin" => CreateAddIdpAdminTask(requestContext, properties), 
			"TaskDeleteIdpAdmin" => CreateDeleteIdpAdminTask(requestContext, properties), 
			"TaskDeleteIdpUser" => CreateDeleteIdpUserTask(requestContext, properties), 
			"TaskGetDevicePresets" => CreateGetDevicePresetsTask(requestContext, properties), 
			_ => throw new NotSupportedException(), 
		};
	}

	private static GenericObject2 CreateTaskDeleteArchive(IGeminiRequestContext requestContext, ObjectProperty2[] properties)
	{
		string nestedPropertyOrDefault = GenericObjectHelper2.GetNestedPropertyOrDefault<string>(properties, "InputParameters", "idArchive");
		bool nestedPropertyOrDefault2 = GenericObjectHelper2.GetNestedPropertyOrDefault<bool>(properties, "InputParameters", "deleteIfOffline");
		return ConvertToGenericObject2(Task.DeleteArchiveStorage(requestContext, nestedPropertyOrDefault, nestedPropertyOrDefault2));
	}

	private static GenericObject2 CreateTaskDeleteStorage(IGeminiRequestContext requestContext, ObjectProperty2[] properties)
	{
		string nestedPropertyOrDefault = GenericObjectHelper2.GetNestedPropertyOrDefault<string>(properties, "InputParameters", "idStorage");
		bool nestedPropertyOrDefault2 = GenericObjectHelper2.GetNestedPropertyOrDefault<bool>(properties, "InputParameters", "deleteIfOffline");
		return ConvertToGenericObject2(Task.DeleteRecordingStorage(requestContext, nestedPropertyOrDefault, nestedPropertyOrDefault2));
	}

	private static GenericObject2 CreateTaskDeleteHardware(IGeminiRequestContext requestContext, ObjectProperty2[] properties)
	{
		string nestedPropertyOrDefault = GenericObjectHelper2.GetNestedPropertyOrDefault<string>(properties, "InputParameters", "hardwareId");
		return ConvertToGenericObject2(Task.DeleteHardware(requestContext, nestedPropertyOrDefault));
	}

	private static GenericObject2 CreateTaskChangeHardwarePassword(IGeminiRequestContext requestContext, ObjectProperty2[] properties)
	{
		string nestedPropertyOrDefault = GenericObjectHelper2.GetNestedPropertyOrDefault<string>(properties, "InputParameters", "hardwareId");
		string nestedPropertyOrDefault2 = GenericObjectHelper2.GetNestedPropertyOrDefault<string>(properties, "InputParameters", "userName");
		string nestedPropertyOrDefault3 = GenericObjectHelper2.GetNestedPropertyOrDefault<string>(properties, "InputParameters", "password");
		return ConvertToGenericObject2(Task.ChangeHardwarePassword(requestContext, nestedPropertyOrDefault, nestedPropertyOrDefault2, nestedPropertyOrDefault3));
	}

	private static GenericObject2 CreateTaskFirmwareUpgrade(IGeminiRequestContext requestContext, ObjectProperty2[] properties)
	{
		string nestedPropertyOrDefault = GenericObjectHelper2.GetNestedPropertyOrDefault<string>(properties, "InputParameters", "hardwareId");
		string nestedPropertyOrDefault2 = GenericObjectHelper2.GetNestedPropertyOrDefault<string>(properties, "InputParameters", "fileId");
		return ConvertToGenericObject2(Task.FirmwareUpgrade(requestContext, nestedPropertyOrDefault, nestedPropertyOrDefault2));
	}

	private static GenericObject2 CreateTaskMoveArchiveStorage(IGeminiRequestContext requestContext, ObjectProperty2[] properties)
	{
		string nestedPropertyOrDefault = GenericObjectHelper2.GetNestedPropertyOrDefault<string>(properties, "InputParameters", "idArchive");
		string nestedPropertyOrDefault2 = GenericObjectHelper2.GetNestedPropertyOrDefault<string>(properties, "InputParameters", "pathTarget");
		return ConvertToGenericObject2(Task.MoveArchiveStorage(requestContext, nestedPropertyOrDefault, nestedPropertyOrDefault2));
	}

	private static GenericObject2 CreateTaskMoveRecordingStorage(IGeminiRequestContext requestContext, ObjectProperty2[] properties)
	{
		string nestedPropertyOrDefault = GenericObjectHelper2.GetNestedPropertyOrDefault<string>(properties, "InputParameters", "idStorage");
		string nestedPropertyOrDefault2 = GenericObjectHelper2.GetNestedPropertyOrDefault<string>(properties, "InputParameters", "pathTarget");
		return ConvertToGenericObject2(Task.MoveRecordingStorage(requestContext, nestedPropertyOrDefault, nestedPropertyOrDefault2));
	}

	private static GenericObject2 CreateTaskTruncateDeviceStorage(IGeminiRequestContext requestContext, ObjectProperty2[] properties)
	{
		string nestedPropertyOrDefault = GenericObjectHelper2.GetNestedPropertyOrDefault<string>(properties, "InputParameters", "idDevice");
		string nestedPropertyOrDefault2 = GenericObjectHelper2.GetNestedPropertyOrDefault<string>(properties, "InputParameters", "idRecorder");
		return ConvertToGenericObject2(Task.TruncateDeviceStorage(requestContext, nestedPropertyOrDefault, nestedPropertyOrDefault2));
	}

	private static GenericObject2 CreateTaskRequestEdgeStorage(IGeminiRequestContext requestContext, ObjectProperty2[] properties)
	{
		string nestedPropertyOrDefault = GenericObjectHelper2.GetNestedPropertyOrDefault<string>(properties, "InputParameters", "idDevice");
		DateTime startTime = DateTime.ParseExact(GenericObjectHelper2.GetNestedPropertyOrDefault<string>(properties, "InputParameters", "startTime"), "O", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal);
		DateTime endTime = DateTime.ParseExact(GenericObjectHelper2.GetNestedPropertyOrDefault<string>(properties, "InputParameters", "endTime"), "O", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal);
		string nestedPropertyOrDefault2 = GenericObjectHelper2.GetNestedPropertyOrDefault<string>(properties, "InputParameters", "metadata");
		return ConvertToGenericObject2(Task.RequestEdgeStorageRetrieval(requestContext, nestedPropertyOrDefault, startTime, endTime, nestedPropertyOrDefault2));
	}

	private static GenericObject2 CreateTaskMoveDevice(IGeminiRequestContext requestContext, ObjectProperty2[] properties)
	{
		string nestedPropertyOrDefault = GenericObjectHelper2.GetNestedPropertyOrDefault<string>(properties, "InputParameters", "idDevice");
		string nestedPropertyOrDefault2 = GenericObjectHelper2.GetNestedPropertyOrDefault<string>(properties, "InputParameters", "idStorageTarget");
		return ConvertToGenericObject2(Task.ChangeDeviceRecordingStorage(requestContext, nestedPropertyOrDefault, nestedPropertyOrDefault2));
	}

	private static GenericObject2 CreateTaskSetEncryptionMethod(IGeminiRequestContext requestContext, ObjectProperty2[] properties)
	{
		string nestedPropertyOrDefault = GenericObjectHelper2.GetNestedPropertyOrDefault<string>(properties, "InputParameters", "idStorage");
		if (!Enum.TryParse<EncryptionMethod>(GenericObjectHelper2.GetNestedPropertyOrDefault<string>(properties, "InputParameters", "encryptionMethod"), out var result))
		{
			throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO62026, "encryptionMethod"));
		}
		string nestedPropertyOrDefault2 = GenericObjectHelper2.GetNestedPropertyOrDefault<string>(properties, "InputParameters", "password");
		return ConvertToGenericObject2(Task.ChangeRecordingStorageEncryptionMethod(requestContext, nestedPropertyOrDefault, result, nestedPropertyOrDefault2));
	}

	private static GenericObject2 CreateTaskSetEncryptionPassword(IGeminiRequestContext requestContext, ObjectProperty2[] properties)
	{
		string nestedPropertyOrDefault = GenericObjectHelper2.GetNestedPropertyOrDefault<string>(properties, "InputParameters", "idStorage");
		string nestedPropertyOrDefault2 = GenericObjectHelper2.GetNestedPropertyOrDefault<string>(properties, "InputParameters", "oldPassword");
		string nestedPropertyOrDefault3 = GenericObjectHelper2.GetNestedPropertyOrDefault<string>(properties, "InputParameters", "newPassword");
		return ConvertToGenericObject2(Task.ChangeRecordingStorageEncryptionPassword(requestContext, nestedPropertyOrDefault, nestedPropertyOrDefault2, nestedPropertyOrDefault3));
	}

	private static GenericObject2 CreateMoveHardwareTask(IGeminiRequestContext requestContext, ObjectProperty2[] properties)
	{
		string nestedPropertyOrDefault = GenericObjectHelper2.GetNestedPropertyOrDefault<string>(properties, "InputParameters", "hardwareId");
		string nestedPropertyOrDefault2 = GenericObjectHelper2.GetNestedPropertyOrDefault<string>(properties, "InputParameters", "destinationRecorderId");
		string nestedPropertyOrDefault3 = GenericObjectHelper2.GetNestedPropertyOrDefault<string>(properties, "InputParameters", "destinationStorageId");
		bool ignoreSourceRecordingServer = GenericObjectHelper2.GetNestedPropertyOrDefault<int>(properties, "InputParameters", "ignoreSourceRecordingServer") != 0;
		return ConvertToGenericObject2(Task.MoveHardware(requestContext, nestedPropertyOrDefault, nestedPropertyOrDefault2, nestedPropertyOrDefault3, ignoreSourceRecordingServer));
	}

	private static GenericObject2 CreateTaskUpdateHardware(IGeminiRequestContext requestContext, ObjectProperty2[] properties)
	{
		string nestedPropertyOrDefault = GenericObjectHelper2.GetNestedPropertyOrDefault<string>(properties, "InputParameters", "hardwareId");
		long sessionDataId = long.Parse(GenericObjectHelper2.GetNestedPropertyOrDefault<string>(properties, "InputParameters", "sessionDataId"), CultureInfo.InvariantCulture);
		return ConvertToGenericObject2(Task.UpdateHardware(requestContext, nestedPropertyOrDefault, sessionDataId));
	}

	private static GenericObject2 CreateTaskReplaceHardware(IGeminiRequestContext requestContext, ObjectProperty2[] properties)
	{
		string nestedPropertyOrDefault = GenericObjectHelper2.GetNestedPropertyOrDefault<string>(properties, "InputParameters", "hardwareId");
		string nestedPropertyOrDefault2 = GenericObjectHelper2.GetNestedPropertyOrDefault<string>(properties, "InputParameters", "hardwareAddress");
		int nestedPropertyOrDefault3 = GenericObjectHelper2.GetNestedPropertyOrDefault<int>(properties, "InputParameters", "port");
		bool nestedPropertyOrDefault4 = GenericObjectHelper2.GetNestedPropertyOrDefault<bool>(properties, "InputParameters", "useHttps");
		int nestedPropertyOrDefault5 = GenericObjectHelper2.GetNestedPropertyOrDefault<int>(properties, "InputParameters", "httpsPort");
		string nestedPropertyOrDefault6 = GenericObjectHelper2.GetNestedPropertyOrDefault<string>(properties, "InputParameters", "userName");
		string nestedPropertyOrDefault7 = GenericObjectHelper2.GetNestedPropertyOrDefault<string>(properties, "InputParameters", "password");
		int driverNumber = int.Parse(GenericObjectHelper2.GetNestedPropertyOrDefault<string>(properties, "InputParameters", "driverNumber"), CultureInfo.InvariantCulture);
		string nestedPropertyOrDefault8 = GenericObjectHelper2.GetNestedPropertyOrDefault<string>(properties, "InputParameters", "customDriverData");
		bool nestedPropertyOrDefault9 = GenericObjectHelper2.GetNestedPropertyOrDefault<bool>(properties, "InputParameters", "allowDeletingDisabledDevices");
		return ConvertToGenericObject2(Task.ReplaceHardware(requestContext, nestedPropertyOrDefault, nestedPropertyOrDefault2, nestedPropertyOrDefault3, nestedPropertyOrDefault4, nestedPropertyOrDefault5, nestedPropertyOrDefault6, nestedPropertyOrDefault7, driverNumber, nestedPropertyOrDefault8, nestedPropertyOrDefault9));
	}

	private static GenericObject2 CreateActivateLicenseTask(IGeminiRequestContext requestContext, ObjectProperty2[] properties)
	{
		string nestedPropertyOrDefault = GenericObjectHelper2.GetNestedPropertyOrDefault<string>(properties, "InputParameters", "userName");
		string text = GenericObjectHelper2.GetNestedPropertyOrDefault<string>(properties, "InputParameters", "password");
		string encryptedValue = "";
		if (requestContext.GetEncryptionService().TryDecrypt(text, out encryptedValue))
		{
			text = encryptedValue;
		}
		return ConvertToGenericObject2(Task.ActivateLicense(requestContext, nestedPropertyOrDefault, text));
	}

	private static GenericObject2 CreateAddHardwareTask(IGeminiRequestContext requestContext, ObjectProperty2[] properties)
	{
		string nestedPropertyOrDefault = GenericObjectHelper2.GetNestedPropertyOrDefault<string>(properties, "InputParameters", "idRecorder");
		string nestedPropertyOrDefault2 = GenericObjectHelper2.GetNestedPropertyOrDefault<string>(properties, "InputParameters", "address");
		string nestedPropertyOrDefault3 = GenericObjectHelper2.GetNestedPropertyOrDefault<string>(properties, "InputParameters", "userName");
		string nestedPropertyOrDefault4 = GenericObjectHelper2.GetNestedPropertyOrDefault<string>(properties, "InputParameters", "password");
		int nestedPropertyOrDefault5 = GenericObjectHelper2.GetNestedPropertyOrDefault<int>(properties, "InputParameters", "driverNumber");
		string nestedPropertyOrDefault6 = GenericObjectHelper2.GetNestedPropertyOrDefault<string>(properties, "InputParameters", "definitionXml");
		return ConvertToGenericObject2(Task.AddHardware(requestContext, nestedPropertyOrDefault, nestedPropertyOrDefault2, nestedPropertyOrDefault3, nestedPropertyOrDefault4, nestedPropertyOrDefault5, nestedPropertyOrDefault6, string.Empty));
	}

	private static GenericObject2 CreateHardwareScanTask(IGeminiRequestContext requestContext, ObjectProperty2[] properties)
	{
		string nestedPropertyOrDefault = GenericObjectHelper2.GetNestedPropertyOrDefault<string>(properties, "InputParameters", "idRecorder");
		string nestedPropertyOrDefault2 = GenericObjectHelper2.GetNestedPropertyOrDefault<string>(properties, "InputParameters", "address");
		string nestedPropertyOrDefault3 = GenericObjectHelper2.GetNestedPropertyOrDefault<string>(properties, "InputParameters", "drivers");
		string nestedPropertyOrDefault4 = GenericObjectHelper2.GetNestedPropertyOrDefault<string>(properties, "InputParameters", "userName");
		string nestedPropertyOrDefault5 = GenericObjectHelper2.GetNestedPropertyOrDefault<string>(properties, "InputParameters", "password");
		bool nestedPropertyOrDefault6 = GenericObjectHelper2.GetNestedPropertyOrDefault<bool>(properties, "InputParameters", "useDefaultCredentials");
		Task task;
		if (string.IsNullOrEmpty(nestedPropertyOrDefault3))
		{
			task = Task.HardwareScan(requestContext, nestedPropertyOrDefault, nestedPropertyOrDefault2, nestedPropertyOrDefault4, nestedPropertyOrDefault5, nestedPropertyOrDefault6);
		}
		else
		{
			int[] selectedHardwareDriverIds = (from f in nestedPropertyOrDefault3.Trim().Split(';')
				select int.Parse(f, CultureInfo.InvariantCulture)).ToArray();
			task = Task.HardwareScan(requestContext, nestedPropertyOrDefault, nestedPropertyOrDefault2, nestedPropertyOrDefault4, nestedPropertyOrDefault5, nestedPropertyOrDefault6, selectedHardwareDriverIds);
		}
		return ConvertToGenericObject2(task);
	}

	private static GenericObject2 CreateHardwareScanExpressTask(IGeminiRequestContext requestContext, ObjectProperty2[] properties)
	{
		string nestedPropertyOrDefault = GenericObjectHelper2.GetNestedPropertyOrDefault<string>(properties, "InputParameters", "idRecorder");
		string nestedPropertyOrDefault2 = GenericObjectHelper2.GetNestedPropertyOrDefault<string>(properties, "InputParameters", "credentialsListXml");
		Credentials credentials = new Credentials();
		if (!string.IsNullOrEmpty(nestedPropertyOrDefault2))
		{
			foreach (ServerCameraCredential item in ServerCameraCredentials.GetFromXml(nestedPropertyOrDefault2))
			{
				credentials.Items.Add(new Credential(item.UserName, item.Password));
			}
		}
		bool nestedPropertyOrDefault3 = GenericObjectHelper2.GetNestedPropertyOrDefault<bool>(properties, "InputParameters", "useDefaultCredentials");
		bool nestedPropertyOrDefault4 = GenericObjectHelper2.GetNestedPropertyOrDefault<bool>(properties, "InputParameters", "useSecurePort");
		return ConvertToGenericObject2(Task.HardwareScanExpress(requestContext, nestedPropertyOrDefault, credentials, nestedPropertyOrDefault3, nestedPropertyOrDefault4));
	}

	private static GenericObject2 CreateAddIdpAdminTask(IGeminiRequestContext requestContext, ObjectProperty2[] properties)
	{
		string nestedPropertyOrDefault = GenericObjectHelper2.GetNestedPropertyOrDefault<string>(properties, "InputParameters", "adminId");
		return ConvertToGenericObject2(Task.AddIdpAdmin(requestContext, nestedPropertyOrDefault));
	}

	private static GenericObject2 CreateDeleteIdpAdminTask(IGeminiRequestContext requestContext, ObjectProperty2[] properties)
	{
		string nestedPropertyOrDefault = GenericObjectHelper2.GetNestedPropertyOrDefault<string>(properties, "InputParameters", "adminId");
		return ConvertToGenericObject2(Task.DeleteIdpAdmin(requestContext, nestedPropertyOrDefault));
	}

	private static GenericObject2 CreateDeleteIdpUserTask(IGeminiRequestContext requestContext, ObjectProperty2[] properties)
	{
		string nestedPropertyOrDefault = GenericObjectHelper2.GetNestedPropertyOrDefault<string>(properties, "InputParameters", "id");
		return ConvertToGenericObject2(Task.DeleteIdpUser(requestContext, nestedPropertyOrDefault));
	}

	private static GenericObject2 CreateGetDevicePresetsTask(IGeminiRequestContext requestContext, ObjectProperty2[] properties)
	{
		string nestedPropertyOrDefault = GenericObjectHelper2.GetNestedPropertyOrDefault<string>(properties, "InputParameters", "idRecorder");
		string nestedPropertyOrDefault2 = GenericObjectHelper2.GetNestedPropertyOrDefault<string>(properties, "InputParameters", "idDevice");
		long sessionDataId = long.Parse(GenericObjectHelper2.GetNestedPropertyOrDefault<string>(properties, "InputParameters", "sessionDataId"), CultureInfo.InvariantCulture);
		return ConvertToGenericObject2(Task.GetDevicePresets(requestContext, nestedPropertyOrDefault, nestedPropertyOrDefault2, sessionDataId));
	}

	public GenericObject2 UpdateObject2(IGeminiRequestContext requestContext, GenericObject2 genericObject, CustomProperty[] customProperties, int clientVersion)
	{
		throw new NotSupportedException();
	}

	public void DeleteObject2(IGeminiRequestContext requestContext, string id, int clientVersion)
	{
		throw new NotSupportedException();
	}

	public static void DeleteTasksByDeviceBeforeDt(IGeminiRequestContext requestContext, string deviceId, DateTime dt)
	{
		Task.DeleteTasksByDeviceBeforeDt(requestContext, deviceId, dt);
	}

	public static GenericObject2 ConvertToGenericObject2(Task task)
	{
		if (task == null)
		{
			return null;
		}
		List<ObjectProperty2> list = new List<ObjectProperty2>();
		list.Add(new ObjectProperty2("Id", task.Id));
		list.Add(new ObjectProperty2("Target", task.Target));
		list.Add(new ObjectProperty2("Information", task.Information));
		list.Add(new ObjectProperty2("Status", task.AsyncStatus.ExceptionStatus.ToString()));
		list.Add(new ObjectProperty2("AsyncOperation", task.AsyncOperation));
		list.Add(new ObjectProperty2("StartTime", task.StartTime));
		list.Add(new ObjectProperty2("EndTime", task.EndTime));
		list.Add(new ObjectProperty2("PercentCompleted", task.PercentCompleted));
		switch (task.Type)
		{
		case Task.TaskType.DeleteHardware:
		case Task.TaskType.TruncateDeviceStorage:
		case Task.TaskType.MoveRecordingStorage:
		case Task.TaskType.MoveArchive:
		case Task.TaskType.MountArchive:
		case Task.TaskType.UnmountArchive:
		case Task.TaskType.DeleteArchive:
		case Task.TaskType.DeleteStorage:
		case Task.TaskType.StorageRetrieval:
		case Task.TaskType.MoveDeviceDb:
		case Task.TaskType.SetEncryptionMethod:
		case Task.TaskType.SetEncryptionPassword:
		case Task.TaskType.Archiving:
		case Task.TaskType.Repairing:
		case Task.TaskType.UpdateHardware:
		case Task.TaskType.ReplaceHardware:
			if (!task.TaskProperties.Any((KeyValue kv) => kv.Key.Equals("idRecorder", StringComparison.OrdinalIgnoreCase)))
			{
				task.TaskProperties.Add(new KeyValue
				{
					Key = "idRecorder",
					Value = task.Target
				});
			}
			break;
		case Task.TaskType.HardwareScan:
		{
			KeyValue keyValue2 = task.TaskProperties.FirstOrDefault((KeyValue p) => p.Key.Equals("selectedDriversXml"));
			if (keyValue2 != null)
			{
				ScanPriorities scanPriorities = ScanPriorities.FromXml(keyValue2.Value);
				string value = string.Join(";", scanPriorities.Items.Select((ScanPriority i) => i.DriverNumber).ToArray());
				task.TaskProperties.Add(new KeyValue
				{
					Key = "drivers",
					Value = value
				});
			}
			break;
		}
		case Task.TaskType.HardwareScanExpress:
		{
			KeyValue keyValue = task.TaskProperties.FirstOrDefault((KeyValue p) => p.Key.Equals("credentialsListXml"));
			if (keyValue == null)
			{
				break;
			}
			Credentials credentials = Credentials.FromXml(keyValue.Value);
			ServerCameraCredentials serverCameraCredentials = new ServerCameraCredentials();
			foreach (Credential item in credentials.Items)
			{
				serverCameraCredentials.Add(new ServerCameraCredential(item.UserName, item.Password));
			}
			keyValue.Value = ServerCameraCredentials.GetAsXml(serverCameraCredentials);
			break;
		}
		}
		list.Add(new ObjectProperty2("StatusDetails", task.AsyncStatus.LastException));
		ObjectProperty2[] propertyValue = task.TaskProperties.Select((KeyValue tp) => new ObjectProperty2(tp.Key, tp.Value)).ToArray();
		list.Add(new ObjectProperty2("InputParameters", propertyValue));
		return new GenericObject2(task.Id, TaskTypeToEntityName(task.Type), list.ToArray(), new RelationProperty[0]);
	}

	public static GenericObject2[] ConvertToGenericObject2s(Task[] task)
	{
		return task?.Select((Task t) => ConvertToGenericObject2(t)).ToArray();
	}

	public static string TaskTypeToEntityName(Task.TaskType taskType)
	{
		return TaskTypeInformation.TaskTypesDictionary[taskType].EntityName;
	}

	public static Task.TaskType EntityNameToTaskType(string entityName)
	{
		return TaskTypeInformation.TaskTypesDictionary.Values.First((TaskTypeInformation v) => string.Equals(v.EntityName, entityName, StringComparison.Ordinal)).TaskType;
	}
}
