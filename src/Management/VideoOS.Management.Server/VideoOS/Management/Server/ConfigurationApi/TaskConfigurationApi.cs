using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using VideoOS.ConfigurationAPI;
using VideoOS.ConfigurationAPI.ConfigurationFaultException;
using VideoOS.Management.Common;
using VideoOS.Management.Common.Tasks;
using VideoOS.Management.Server.Properties;
using VideoOS.Management.Server.TaskSecurityProviders;

namespace VideoOS.Management.Server.ConfigurationApi;

[EntityName("ConfigAPITask")]
public class TaskConfigurationApi : ConfigurationApiBase, IConfigurationApiSupport
{
	public const string PropertyTaskType = "TaskType";

	public const string AlreadyCleanedKey = "AlreadyCleaned";

	public const string TaskStartTime = "TaskStartTime";

	public const string TaskEndTime = "TaskEndTime";

	public const string TargetPath = "TargetPath";

	public ValidateResult SetItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		throw new NotSupportedException();
	}

	public ValidateResult ValidateItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		throw new NotSupportedException();
	}

	public ConfigurationItem[] GetChildItems(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		throw new NotSupportedException();
	}

	public ConfigurationItem GetItem(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		return GetItemInternal(requestContext, path, Task.QueryTask, ServerInformation.GetServerInformation);
	}

	public static ConfigurationItem GetItemInternal(IGeminiRequestContext requestContext, ConfigurationItemPath path, Func<IGeminiRequestContext, string, Task> retrieveTask, Func<IGeminiRequestContext, ServerInformation> retrieveServerInformation)
	{
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		if (string.Equals(path.ItemType, "Task", StringComparison.Ordinal))
		{
			ThrowIfPathInvalid(path);
			try
			{
				Task task = retrieveTask(requestContext, path.Id);
				return ConvertTaskToConfigurationItem(requestContext, path, task, retrieveServerInformation);
			}
			catch (UnauthorizedAccessException)
			{
				throw new PathNotFoundException("Invalid task: " + path.Text);
			}
			catch (DatabaseConfigExtraRowsException ex2)
			{
				throw ConfigurationApiExceptionUtilities.CreateServerException(ex2);
			}
			catch (Exception ex3)
			{
				throw ConfigurationApiExceptionUtilities.HandleException(ex3);
			}
		}
		throw new ArgumentException("Unsupported ItemType in path", "path");
	}

	public static ConfigurationItem ConvertTaskToConfigurationItem(IGeminiRequestContext requestContext, ConfigurationItemPath path, Task task, Func<IGeminiRequestContext, ServerInformation> retrieveServerInformation)
	{
		if (task == null)
		{
			return ConstructCompletedTask(path);
		}
		ThrowIfTypeIsInvalid(task.Type);
		Property progress = GetProgress(0);
		string text = "Unknown";
		List<Property> list = new List<Property>();
		switch (task.AsyncStatus.ExceptionStatus)
		{
		case ExceptionStatus.InProgress:
		case ExceptionStatus.Stopping:
			text = "InProgress";
			progress.Value = task.PercentCompleted.ToString(CultureInfo.InvariantCulture);
			list.Add(progress);
			break;
		case ExceptionStatus.Inactive:
			text = "Idle";
			break;
		case ExceptionStatus.Stopped:
			text = "Completed";
			progress.Value = task.PercentCompleted.ToString(CultureInfo.InvariantCulture);
			list.Add(progress);
			if (!string.IsNullOrEmpty(task.AsyncStatus.LastException))
			{
				try
				{
					TaskError taskError2 = TaskError.FromXml(task.AsyncStatus.LastException);
					PrepareErrorMessage(taskError2);
					list.Add(GetErrorCode(taskError2.ErrorCode));
					list.Add(GetErrorText(taskError2.ErrorText));
				}
				catch (InvalidOperationException)
				{
				}
			}
			break;
		case ExceptionStatus.Failed:
			text = "Error";
			try
			{
				TaskError taskError = TaskError.FromXml(task.AsyncStatus.LastException);
				PrepareErrorMessage(taskError);
				progress.Value = task.PercentCompleted.ToString(CultureInfo.InvariantCulture);
				list.Add(progress);
				list.Add(GetErrorCode(taskError.ErrorCode));
				list.Add(GetErrorText(taskError.ErrorText));
			}
			catch (InvalidOperationException)
			{
				progress.Value = "100";
				list.Add(progress);
				list.Add(GetErrorText(task.AsyncStatus.LastException));
			}
			break;
		case ExceptionStatus.Succeeded:
			text = "Success";
			progress.Value = "100";
			list.Add(progress);
			break;
		default:
			throw new InternalServerException("Unknown task state");
		}
		list.Add(GetTaskType(task.Type));
		list.Add(GetTargetPath(requestContext, task.Target, retrieveServerInformation));
		list.Add(GetAlreadyCleaned(alreadyCleaned: false));
		list.Add(TaskStartTimeProperty(task.StartTime));
		list.Add(TaskEndTimeProperty(task.EndTime));
		if (task.Type == Task.TaskType.StorageRetrieval)
		{
			list.Add(RecordingDeviceMethodPropertyDefinition.StartTimeProperty(DateTime.Parse(task.TaskProperties.First((KeyValue p) => p.Key == "startTime").Value, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal)));
			list.Add(RecordingDeviceMethodPropertyDefinition.EndTimeProperty(DateTime.Parse(task.TaskProperties.First((KeyValue p) => p.Key == "endTime").Value, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal)));
			list.Add(RecordingDeviceMethodPropertyDefinition.DeviceProperty(ConfigurationItemPath.CreateItemPath("Camera", task.TaskProperties.First((KeyValue p) => p.Key == "idDevice").Value)));
			KeyValue keyValue = task.TaskProperties.FirstOrDefault((KeyValue p) => p.Key == "metadata");
			if (keyValue != null && EdgeRetrievalMetadata.Read(HttpUtility.HtmlDecode(keyValue.Value), out var userName, out var creationTime, out var identifier, out var relatedTasks))
			{
				list.Add(RecordingDeviceMethodPropertyDefinition.IdentifierProperty(identifier));
				list.Add(RecordingDeviceMethodPropertyDefinition.UserNameProperty(userName));
				list.Add(RecordingDeviceMethodPropertyDefinition.CreationTimeProperty(creationTime));
				list.Add(RecordingDeviceMethodPropertyDefinition.RelatedTasksProperty(relatedTasks));
			}
		}
		Property[] array = TaskHelperFunctions.CreateTaskResultProperties(requestContext, task);
		if (array != null)
		{
			list.AddRange(array);
		}
		bool managementServer;
		string id = ValidateServerId(task, out managementServer);
		string parentPath = ((!managementServer) ? new ConfigurationItemPathBuilder("RecordingServer", id).CreatePath().Text : "/");
		ConfigurationItem[] childItems = TaskHelperFunctions.CreateTaskChildItems(requestContext, task, parentPath, path);
		list.Add(new Property("State", text));
		string displayName = TaskHelperFunctions.CreateDisplayName(task);
		bool flag = false;
		try
		{
			TaskSecurityProviderFactory.GetSecurityProvider(task.Type).CheckStop(requestContext, task);
			flag = true;
		}
		catch (NotSupportedException)
		{
			flag = false;
		}
		catch (UnauthorizedAccessException)
		{
			flag = true;
		}
		List<string> list2 = new List<string>();
		list2.Add("TaskCleanup");
		if (flag)
		{
			list2.Add("TaskStop");
		}
		return ConstructTaskResult(list, parentPath, path, list2.ToArray(), childItems, displayName);
	}

	private static ConfigurationItem ConstructCompletedTask(ConfigurationItemPath path)
	{
		return ConstructTaskResult(new List<Property>
		{
			new Property("State", "Completed"),
			GetAlreadyCleaned(alreadyCleaned: true)
		}, "/", path, new List<string> { "TaskCleanup" }.ToArray(), "");
	}

	private static string ValidateServerId(Task task, out bool managementServer)
	{
		KeyValue keyValue = task.TaskProperties.FirstOrDefault((KeyValue p) => p.Key.Equals("idRecorder", StringComparison.Ordinal) || p.Key.Equals("recordingServerId", StringComparison.Ordinal) || p.Key.Equals("sourceRecorderId", StringComparison.Ordinal));
		if (keyValue != null)
		{
			managementServer = false;
			return keyValue.Value;
		}
		KeyValue keyValue2 = task.TaskProperties.FirstOrDefault((KeyValue p) => p.Key.Equals("target", StringComparison.Ordinal));
		if (keyValue2 != null)
		{
			managementServer = true;
			return keyValue2.Value;
		}
		throw new InternalServerException("Unable to find id of management server or recorder in task properties");
	}

	private static void ThrowIfTypeIsInvalid(Task.TaskType taskType)
	{
		if (!new List<Task.TaskType>
		{
			Task.TaskType.AddHardware,
			Task.TaskType.HardwareScan,
			Task.TaskType.HardwareScanExpress,
			Task.TaskType.DeleteStorage,
			Task.TaskType.DeleteArchive,
			Task.TaskType.SetEncryptionPassword,
			Task.TaskType.StorageRetrieval,
			Task.TaskType.MoveArchive,
			Task.TaskType.MountArchive,
			Task.TaskType.UnmountArchive,
			Task.TaskType.MoveRecordingStorage,
			Task.TaskType.SetEncryptionMethod,
			Task.TaskType.TruncateDeviceStorage,
			Task.TaskType.MoveDeviceDb,
			Task.TaskType.DeleteHardware,
			Task.TaskType.MoveHardware,
			Task.TaskType.UpdateHardware,
			Task.TaskType.ReplaceHardware,
			Task.TaskType.ActivateLicense,
			Task.TaskType.FirmwareUpgrade,
			Task.TaskType.DeleteIdpUser,
			Task.TaskType.GetDevicePresets,
			Task.TaskType.ChangeHardwarePassword,
			Task.TaskType.DeleteRecorder
		}.Contains(taskType))
		{
			throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, "Task not supported for {0}", taskType));
		}
	}

	public ConfigurationItem InvokeMethod(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath, string methodId)
	{
		if (methodId == null)
		{
			throw new ArgumentNullException("methodId");
		}
		if (!item.MethodIds.Contains(methodId))
		{
			throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Invalid MethodId {0} on ItemType: {1}", methodId, item.ItemType), "methodId");
		}
		if (!string.Equals(path.ItemType, "Task", StringComparison.Ordinal))
		{
			throw new ArgumentException("Incorrect item used for task Invoke", "item");
		}
		if (!(methodId == "TaskCleanup"))
		{
			if (methodId == "TaskStop")
			{
				return StopTask(requestContext, item, path);
			}
			throw new ArgumentException("Unknown methodId:" + methodId);
		}
		return CleanupTask(requestContext, item, path);
	}

	private static void ThrowIfPathInvalid(ConfigurationItemPath path)
	{
		if (path.IsFolder)
		{
			throw new PathInvalidException("Invalid id. Path: " + path.Text);
		}
		if (!int.TryParse(path.Id, out var _))
		{
			throw new PathInvalidException("Invalid id. Path: " + path.Text);
		}
	}

	private static ConfigurationItem CleanupTask(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		ThrowIfPathInvalid(path);
		try
		{
			bool alreadyCleaned = true;
			try
			{
				Task task = Task.QueryTask(requestContext, path.Id);
				if (task != null)
				{
					alreadyCleaned = false;
					Task.CleanupTask(requestContext, task.Id);
				}
			}
			catch (UnauthorizedAccessException)
			{
			}
			item.Properties = new Property[1] { GetAlreadyCleaned(alreadyCleaned) };
			return MethodTypesHelper.CreateInvokeResult(item, null, "Result", "Success", true);
		}
		catch (Exception)
		{
			throw;
		}
	}

	private static ConfigurationItem StopTask(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		ThrowIfPathInvalid(path);
		try
		{
			try
			{
				Task task = Task.QueryTask(requestContext, path.Id);
				if (task != null)
				{
					if (task.Type == Task.TaskType.StorageRetrieval)
					{
						Task.RequestStopTaskStopEdgeStorageRetrieval(requestContext, task);
					}
					else
					{
						if (task.Type != Task.TaskType.HardwareScan && task.Type != Task.TaskType.HardwareScanExpress)
						{
							throw new NotSupportedException();
						}
						Task.RequestStopTask(requestContext, task);
					}
				}
			}
			catch (UnauthorizedAccessException)
			{
			}
			return MethodTypesHelper.CreateInvokeResultWithoutItemPropertiesAndChildren(item);
		}
		catch (Exception)
		{
			throw;
		}
	}

	private static string ResolveErrorMessage(string message, int code, ArgumentCollection arguments)
	{
		if (message == null)
		{
			throw new ArgumentNullException("message");
		}
		if (arguments == null)
		{
			throw new ArgumentNullException("arguments");
		}
		List<string> list = new List<string>();
		foreach (Argument argument in arguments)
		{
			list.Add(argument.Value);
		}
		try
		{
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			object[] args = list.ToArray();
			return string.Format(invariantCulture, message, args);
		}
		catch (FormatException)
		{
			return message;
		}
	}

	private static void PrepareErrorMessage(TaskError taskError)
	{
		if (taskError != null)
		{
			taskError.ErrorText = ResolveErrorMessage(taskError.ErrorText, taskError.ErrorCode, taskError.Arguments);
		}
	}

	public static ConfigurationItem ConstructTaskResult(List<Property> properties, string parentPath, ConfigurationItemPath path, string[] methodIds, string displayName = "invokeResult")
	{
		return ConstructTaskResult(properties, parentPath, path, methodIds, null, displayName);
	}

	private static ConfigurationItem ConstructTaskResult(List<Property> properties, string parentPath, ConfigurationItemPath path, string[] methodIds, ConfigurationItem[] childItems, string displayName = "invokeResult")
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "Task";
		configurationItem.UIHint = "DirectionHorizontal";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = displayName;
		configurationItem.ParentPath = parentPath;
		configurationItem.Path = path.Text;
		configurationItem.MethodIds = methodIds;
		configurationItem.Properties = properties.ToArray();
		if (childItems != null)
		{
			configurationItem.Children = childItems;
			bool childrenFilled = childItems.Length != 0;
			configurationItem.ChildrenFilled = childrenFilled;
		}
		return configurationItem;
	}

	private static Property GetProgress(int percent)
	{
		return new Property("Progress", "")
		{
			ValueType = "Progress",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyProgress,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyProgress",
			UIImportance = UIImportance.Default,
			SortKey = 1,
			Value = percent.ToString(CultureInfo.InvariantCulture)
		};
	}

	private static Property GetErrorCode(int code)
	{
		return new Property("ErrorCode", "")
		{
			ValueType = "Int",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyErrorCode,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyErrorCode",
			UIImportance = UIImportance.Disabled,
			SortKey = 1,
			Value = code.ToString(CultureInfo.InvariantCulture)
		};
	}

	public static Property GetErrorText(string text)
	{
		return new Property("ErrorText", text)
		{
			ValueType = "String",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyErrorText,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyErrorText",
			UIImportance = UIImportance.Disabled,
			SortKey = 1
		};
	}

	public static Property GetTaskType(Task.TaskType taskType)
	{
		return new Property("TaskType", taskType.ToString())
		{
			ValueType = "String",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyTaskType,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyTaskType",
			UIImportance = UIImportance.Disabled,
			SortKey = 1
		};
	}

	public static Property GetTaskTypeFilterProperty(string value)
	{
		Property stringProperty = GeneralPropertyDefinitions.GetStringProperty("TaskType", value, ConfigurationApiResources.PropertyTaskType, "PropertyTaskType", isSettable: true);
		stringProperty.ToolTipTranslationId = "PropertyTaskTypeToolTip";
		return stringProperty;
	}

	public static Property GetTargetPath(IGeminiRequestContext requestContext, string targetId, Func<IGeminiRequestContext, ServerInformation> retrieveServerInformation)
	{
		ConfigurationItemPath configurationItemPath = null;
		if (string.Equals(targetId, "909749AE-88C4-4780-A381-383D691E98FA", StringComparison.OrdinalIgnoreCase))
		{
			ServerInformation serverInformation = retrieveServerInformation(requestContext);
			configurationItemPath = new ConfigurationItemPath(isFolder: false, string.Empty, "System", serverInformation.Id, "System[" + serverInformation.Id + "]");
		}
		else
		{
			configurationItemPath = new ConfigurationItemPathBuilder("RecordingServer", targetId).CreatePath();
		}
		Property property = new Property("TargetPath", configurationItemPath.Text);
		property.ValueType = "Path";
		property.IsSettable = false;
		property.DisplayName = ConfigurationApiResources.PropertyTargetPath;
		property.ItemReference = true;
		property.ServerValidation = false;
		property.TranslationId = "PropertyTargetPath";
		property.UIImportance = UIImportance.Default;
		property.SortKey = 1;
		property.ValueTypeInfos = new ValueTypeInfo[1]
		{
			new ValueTypeInfo
			{
				Name = "PathItemType",
				Value = configurationItemPath.ItemType
			}
		};
		return property;
	}

	public static Property GetPath(ConfigurationItemPath path)
	{
		return GetPath(path, "Path");
	}

	public static Property GetPath(ConfigurationItemPath path, string propertyName)
	{
		Property property = new Property(propertyName, path.Text);
		property.ValueType = "Path";
		property.IsSettable = false;
		property.DisplayName = ConfigurationApiResources.PropertyPath;
		property.ItemReference = true;
		property.ServerValidation = false;
		property.TranslationId = "PropertyPath";
		property.UIImportance = UIImportance.Default;
		property.SortKey = 1;
		property.ValueTypeInfos = new ValueTypeInfo[1]
		{
			new ValueTypeInfo
			{
				Name = "PathItemType",
				Value = path.ItemType
			}
		};
		return property;
	}

	private static Property GetAlreadyCleaned(bool alreadyCleaned)
	{
		return new Property("AlreadyCleaned", alreadyCleaned.ToString(CultureInfo.InvariantCulture))
		{
			ValueType = "Tick",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyTaskAlreadyCleaned,
			ItemReference = true,
			ServerValidation = false,
			TranslationId = "PropertyTaskAlreadyCleaned",
			ToolTipTranslationId = "PropertyTaskAlreadyCleanedTip",
			UIImportance = UIImportance.Summary,
			SortKey = 100
		};
	}

	public static Property TaskStartTimeProperty(DateTime startTime)
	{
		return new Property("TaskStartTime", PropertyTypesHelper.GetValue(startTime))
		{
			ValueType = "DateTime",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyTaskStartTime,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyTaskStartTime",
			ToolTipTranslationId = "PropertyTaskStartTimeToolTip"
		};
	}

	public static Property TaskEndTimeProperty(DateTime endTime)
	{
		return new Property("TaskEndTime", PropertyTypesHelper.GetValue(endTime))
		{
			ValueType = "DateTime",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyTaskEndTime,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyTaskEndTime",
			ToolTipTranslationId = "PropertyTaskEndTimeToolTip"
		};
	}
}
