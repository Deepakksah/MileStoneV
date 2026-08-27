using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VideoOS.Common.Integration.DeviceDiscovery;
using VideoOS.ConfigurationAPI;
using VideoOS.ConfigurationAPI.ConfigurationFaultException;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

[EntityName("ConfigAPIRecordingServer")]
[EntityName("ConfigAPIRecordingServerFolder")]
public class RecordingServerConfigurationApi : ConfigurationApiBase, IConfigurationApiSupport
{
	public const string HardwareAddressKey = "HardwareAddress";

	public const string HardwareDriverKey = "HardwareDriverPath";

	public const string HardwareDriverIdsKey = "HardwareDriverIds";

	public const string UserNameKey = "UserName";

	public const string PrimaryUserNameKey = "PrimaryUserName";

	public const string SecondaryUserNameKey = "SecondaryUserName";

	public const string TertiaryUserNameKey = "TertiaryUserName";

	public const string PasswordKey = "Password";

	public const string PrimaryPasswordKey = "PrimaryPassword";

	public const string SecondaryPasswordKey = "SecondaryPassword";

	public const string TertiaryPasswordKey = "TertiaryPassword";

	public const string UseDefaultCredentialsKey = "UseDefaultCredentials";

	public const string UseSecurePortKey = "UseSecurePort";

	public const string CustomDataKey = "CustomData";

	public const string DeleteLockedDataPropertyKey = "DeleteLockedData";

	public const string StorageInfoPropertyKey = "StorageInfo";

	public const string OtherDeviceRecordingsPropertyKey = "OtherDeviceRecordings";

	public const string UserAcceptPropertyKey = "UserAccept";

	public const string CurrentDirectoryKey = "CurrentDirectory";

	public const string NewDirectoryFullNameKey = "NewDirectoryFullName";

	public const string DrivesKey = "Drives";

	public const string DirectoriesKey = "Directories";

	public ValidateResult SetItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		if (requestContext == null)
		{
			throw new ArgumentNullException("requestContext");
		}
		if (item == null)
		{
			throw new ArgumentNullException("item");
		}
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		if (parentPath == null)
		{
			throw new ArgumentNullException("parentPath");
		}
		ThrowIfPathInvalid(path);
		if (string.Equals(path.ItemType, "RecordingServer", StringComparison.Ordinal))
		{
			if (parentPath.ItemType != "RecordingServerFolder")
			{
				throw new PathInvalidException("Invalid parent path. Path: " + parentPath.Text);
			}
			ValidateResult validateResult = ValidateItemInternal(requestContext, item, path, out var name, out var description, out var synchronizationTime, out var publicAccessEnabled, out var publicWebserverHostName, out var publicWebserverPort, out var shutdownOnStorageFailure, out var multicastServerAddress);
			if (!validateResult.ValidatedOk)
			{
				return validateResult;
			}
			RecordingServer recordingServer;
			try
			{
				if (publicAccessEnabled.HasValue || publicWebserverHostName != null || publicWebserverPort.HasValue || shutdownOnStorageFailure.HasValue || multicastServerAddress != null)
				{
					RecordingServer.UpdateRecordingServerServiceConfigurable(requestContext, path.Id, publicAccessEnabled, publicWebserverHostName, publicWebserverPort, shutdownOnStorageFailure, multicastServerAddress);
				}
				recordingServer = RecordingServer.UpdateRecordingServer(requestContext, path.Id, name, description, synchronizationTime, null);
			}
			catch (RecordingServerUpdateException)
			{
				throw new PathNotFoundException(path.Text);
			}
			ConfigurationItem resultItem = ConvertToConfigurationItem(recordingServer);
			validateResult = new ValidateResult();
			validateResult.ValidatedOk = true;
			validateResult.ResultItem = resultItem;
			return validateResult;
		}
		throw new NotSupportedException();
	}

	public ValidateResult ValidateItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		if (requestContext == null)
		{
			throw new ArgumentNullException("requestContext");
		}
		if (item == null)
		{
			throw new ArgumentNullException("item");
		}
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		if (parentPath == null)
		{
			throw new ArgumentNullException("parentPath");
		}
		ThrowIfPathInvalid(path);
		if (string.Equals(path.ItemType, "RecordingServer", StringComparison.Ordinal))
		{
			if (parentPath.ItemType != "RecordingServerFolder")
			{
				throw new PathInvalidException("Invalid parent path. Path: " + parentPath.Text);
			}
			if (!RecordingServer.RecordingServerExists(requestContext, path.Id))
			{
				throw new PathNotFoundException(path.Text);
			}
			string name;
			string description;
			int? synchronizationTime;
			bool? publicAccessEnabled;
			string publicWebserverHostName;
			int? publicWebserverPort;
			bool? shutdownOnStorageFailure;
			string multicastServerAddress;
			return ValidateItemInternal(requestContext, item, path, out name, out description, out synchronizationTime, out publicAccessEnabled, out publicWebserverHostName, out publicWebserverPort, out shutdownOnStorageFailure, out multicastServerAddress);
		}
		throw new NotSupportedException();
	}

	private static ValidateResult ValidateItemInternal(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, out string name, out string description, out int? synchronizationTime, out bool? publicAccessEnabled, out string publicWebserverHostName, out int? publicWebserverPort, out bool? shutdownOnStorageFailure, out string multicastServerAddress)
	{
		name = null;
		description = null;
		synchronizationTime = null;
		publicAccessEnabled = null;
		publicWebserverHostName = null;
		publicWebserverPort = null;
		shutdownOnStorageFailure = null;
		multicastServerAddress = null;
		string value = null;
		try
		{
			if (string.Equals(path.ItemType, "RecordingServer", StringComparison.Ordinal))
			{
				((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Name", out name);
				((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Description", out description);
				((IEnumerable<Property>)item.Properties).TryGetPropertyValue("SynchronizationTime", out synchronizationTime);
				((IEnumerable<Property>)item.Properties).TryGetPropertyValue("PublicAccessEnabled", out value);
				((IEnumerable<Property>)item.Properties).TryGetPropertyValue("PublicWebserverHostName", out publicWebserverHostName);
				((IEnumerable<Property>)item.Properties).TryGetPropertyValue("PublicWebserverPort", out publicWebserverPort);
				((IEnumerable<Property>)item.Properties).TryGetPropertyValue("ShutdownOnStorageFailure", out shutdownOnStorageFailure);
				((IEnumerable<Property>)item.Properties).TryGetPropertyValue("MulticastServerAddress", out multicastServerAddress);
				publicAccessEnabled = Validation.CheckBooleanStringValue(value, "publicAccessEnabled", allowNull: true);
				RecordingServer.UpdateRecordingServerValidation(requestContext, path.Id, name, description, synchronizationTime);
				RecordingServer.UpdateRecordingServerServiceConfigurableValidation(requestContext, path.Id, publicAccessEnabled, publicWebserverHostName, publicWebserverPort, shutdownOnStorageFailure, multicastServerAddress);
				ValidateResult validateResult = new ValidateResult();
				validateResult.ValidatedOk = true;
				validateResult.ResultItem = item;
				return validateResult;
			}
			throw new NotSupportedException();
		}
		catch (ArgumentNullException exception)
		{
			ValidateResult validateResult = ErrorHelper.CreateValidateResult(exception, item.Properties);
			if (validateResult != null)
			{
				return validateResult;
			}
			throw;
		}
		catch (ArgumentException exception2)
		{
			ValidateResult validateResult = ErrorHelper.CreateValidateResult(exception2, item.Properties);
			if (validateResult != null)
			{
				return validateResult;
			}
			throw;
		}
	}

	public ConfigurationItem[] GetChildItems(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		if (requestContext == null)
		{
			throw new ArgumentNullException("requestContext");
		}
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		ThrowIfPathInvalid(path);
		if (string.Equals(path.ItemType, "RecordingServer", StringComparison.OrdinalIgnoreCase))
		{
			if (!RecordingServer.RecordingServerExists(requestContext, path.Id))
			{
				throw new PathNotFoundException(path.Text);
			}
			ConfigurationItem configurationItem = GroupConfigurationItemFactory.CreateFolderItem("HardwareFolder", path);
			ConfigurationItem configurationItem2 = GroupConfigurationItemFactory.CreateFolderItem("HardwareDriverFolder", path);
			ConfigurationItem configurationItem3 = GroupConfigurationItemFactory.CreateFolderItem("StorageFolder", path);
			ConfigurationItem configurationItem4 = GroupConfigurationItemFactory.CreateFolderItem("RecordingServerMulticastFolder", path);
			ConfigurationItem configurationItem5 = GroupConfigurationItemFactory.CreateFolderItem("RecordingServerFailoverFolder", path);
			return new ConfigurationItem[5] { configurationItem, configurationItem2, configurationItem3, configurationItem4, configurationItem5 };
		}
		if (string.Equals(path.ItemType, "RecordingServerFolder", StringComparison.OrdinalIgnoreCase))
		{
			string[] recordingServerIds = RecordingServer.QueryRecordingServerIds(requestContext);
			RecordingServer[] array = RecordingServer.QueryRecordingServersByIds(requestContext, recordingServerIds);
			List<ConfigurationItem> list = new List<ConfigurationItem>();
			RecordingServer[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				ConfigurationItem item = ConvertToConfigurationItem(array2[i]);
				list.Add(item);
			}
			return list.ToArray();
		}
		throw new NotSupportedException();
	}

	public ConfigurationItem GetItem(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		if (requestContext == null)
		{
			throw new ArgumentNullException("requestContext");
		}
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		ThrowIfPathInvalid(path);
		if (string.Equals(path.ItemType, "RecordingServer", StringComparison.Ordinal))
		{
			return ConvertToConfigurationItem(RecordingServer.QueryRecordingServer(requestContext, path.Id) ?? throw new PathNotFoundException(path.Text));
		}
		if (string.Equals(path.ItemType, "RecordingServerFolder", StringComparison.Ordinal))
		{
			ConfigurationItemPath parentPath = ConfigurationItemPath.CreateSystemPath();
			return GroupConfigurationItemFactory.CreateFolderItem("RecordingServerFolder", parentPath);
		}
		throw new NotSupportedException();
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
		bool flag = string.Equals(item.ItemType, "InvokeInfo", StringComparison.Ordinal);
		switch (methodId)
		{
		case "AddHardware":
			if (!flag)
			{
				return CreateInvokeInfoItemAddHardware(item, methodId);
			}
			return AddHardware(requestContext, parentPath, item, path);
		case "RemoveRecordingServer":
			if (!flag)
			{
				return CreateInvokeInfoItemRemoveRecordingServer(item, GetChildItems(requestContext, path), methodId);
			}
			return RemoveRecordingServer(requestContext, item, path);
		case "GetOtherDevicesWithMediaOnRecorder":
			if (!flag)
			{
				return CreateInvokeInfoItemGetOtherDevicesWithMediaOnRecorder(item, methodId);
			}
			return GetOtherDevicesWithMediaOnRecorder(requestContext, item, path);
		case "HardwareScan":
			if (!flag)
			{
				return CreateInvokeInfoItemHardwareScan(item, methodId);
			}
			return HardwareScan(requestContext, item, path);
		case "HardwareScanExpress":
			if (!flag)
			{
				return CreateInvokeInfoItemHardwareScanExpress(item, methodId);
			}
			return HardwareScanExpress(requestContext, item, path);
		case "LoadTasks":
			if (!flag)
			{
				return CreateInvokeInfoItemLoadTasks(item, methodId);
			}
			return LoadTasks(requestContext, item, path);
		case "ReadDrivesOnRecorder":
			if (!flag)
			{
				return CreateInvokeInfoItemReadDrivesOnRecorder(item, methodId);
			}
			return ReadDrivesOnRecorder(requestContext, item, path);
		case "ReadDirectoriesOnRecorder":
			if (!flag)
			{
				return CreateInvokeInfoItemReadDirectoriesOnRecorder(item, methodId);
			}
			return ReadDirectoriesOnRecorder(requestContext, item, path);
		case "CreateDirectoryOnRecorder":
			if (!flag)
			{
				return CreateInvokeInfoItemCreateDirectoryOnRecorder(item, methodId);
			}
			return CreateDirectoryOnRecorder(requestContext, item, path);
		default:
			throw new ArgumentException("Unknown methodId:" + methodId, "methodId");
		}
	}

	private static ConfigurationItem CreateInvokeInfoItemHardwareScanExpress(ConfigurationItem item, string methodId)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionHorizontal";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.WizardScanHardwareExpress;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		configurationItem.MethodIds = new string[1] { methodId };
		List<Property> list = new List<Property>();
		list.Add(UserNameProperty("PrimaryUserName", ""));
		list.Add(PasswordProperty("PrimaryPassword", ""));
		list.Add(UserNameProperty("SecondaryUserName", ""));
		list.Add(PasswordProperty("SecondaryPassword", ""));
		list.Add(UserNameProperty("TertiaryUserName", ""));
		list.Add(PasswordProperty("TertiaryPassword", ""));
		list.Add(UseDefaultCredentialsProperty(useDefaultCredentials: false));
		list.Add(UseSecurePortProperty(useSecurePort: false));
		configurationItem.Properties = list.ToArray();
		return configurationItem;
	}

	private static ConfigurationItem HardwareScanExpress(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		ValidateArguments(requestContext, path, item);
		if (!string.Equals(path.ItemType, "RecordingServer", StringComparison.Ordinal))
		{
			throw new PathInvalidException("Invalid type. Path: " + path.Text);
		}
		((IEnumerable<Property>)item.Properties).TryGetPropertyValue("UseDefaultCredentials", out string value);
		bool useDefaultCredentials = ValidateUseDefaultCredentials(value);
		((IEnumerable<Property>)item.Properties).TryGetPropertyValue("UseSecurePort", out string value2);
		bool useSecurePort = ValidateUseSecurePort(value2);
		string recordingServerId = ValidateRecorderId(requestContext, path);
		Credentials credentials = GetCredentials(item.Properties);
		try
		{
			item = TaskHelperFunctions.CreateTaskInvokeResultConfigurationItem(Task.HardwareScanExpress(requestContext, recordingServerId, credentials, useDefaultCredentials, useSecurePort), item);
			return item;
		}
		catch (ArgumentException ex)
		{
			throw new PropertyInvalidException(ex.Message, ex.ParamName);
		}
	}

	private static Credentials GetCredentials(Property[] itemProperties)
	{
		Credentials credentials = new Credentials();
		Credential credential = GetCredential(itemProperties, "PrimaryUserName", "PrimaryPassword");
		if (credential != null)
		{
			credentials.Items.Add(credential);
		}
		Credential credential2 = GetCredential(itemProperties, "SecondaryUserName", "SecondaryPassword");
		if (credential2 != null)
		{
			credentials.Items.Add(credential2);
		}
		Credential credential3 = GetCredential(itemProperties, "TertiaryUserName", "TertiaryPassword");
		if (credential3 != null)
		{
			credentials.Items.Add(credential3);
		}
		return credentials;
	}

	private static Credential GetCredential(Property[] itemProperties, string userNameKey, string passwordKey)
	{
		((IEnumerable<Property>)itemProperties).TryGetPropertyValue(userNameKey, out string value);
		((IEnumerable<Property>)itemProperties).TryGetPropertyValue(passwordKey, out string value2);
		if (string.IsNullOrEmpty(value))
		{
			return null;
		}
		if (string.IsNullOrEmpty(value2))
		{
			value2 = string.Empty;
		}
		return new Credential(value, value2);
	}

	private static string ValidateRecorderId(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		string id = path.Id;
		if (RecordingServer.QueryRecordingServer(requestContext, id) == null)
		{
			throw new PathNotFoundException(path.Text);
		}
		return id;
	}

	private static bool ValidateUseDefaultCredentials(string useDefaultCredentialsString)
	{
		try
		{
			return bool.Parse(useDefaultCredentialsString);
		}
		catch (FormatException)
		{
			throw new PropertyInvalidException("Invalid property.", "UseDefaultCredentials");
		}
	}

	private static bool ValidateUseSecurePort(string useSecurePortString)
	{
		if (useSecurePortString == null)
		{
			return false;
		}
		try
		{
			return bool.Parse(useSecurePortString);
		}
		catch (FormatException)
		{
			throw new PropertyInvalidException("Invalid property.", "UseSecurePort");
		}
	}

	private static ConfigurationItem CreateInvokeInfoItemLoadTasks(ConfigurationItem item, string methodId)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionHorizontal";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.WizardLoadTasks;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		configurationItem.MethodIds = new string[1] { methodId };
		configurationItem.Properties = new Property[1] { TaskConfigurationApi.GetTaskTypeFilterProperty(string.Empty) };
		return configurationItem;
	}

	private static ConfigurationItem LoadTasks(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		try
		{
			string taskType = string.Empty;
			if (item.Properties != null && ((IEnumerable<Property>)item.Properties).TryGetPropertyValue("TaskType", out string value))
			{
				taskType = value;
			}
			Task[] array = Task.QueryTasksByParentRecordingServer(requestContext, path.Id);
			if (!string.IsNullOrEmpty(taskType))
			{
				array = array.Where((Task t) => string.Equals(t.Type.ToString(), taskType, StringComparison.Ordinal)).ToArray();
			}
			return TaskHelperFunctions.CreateTasksInvokeResultConfigurationItem(requestContext, array, item);
		}
		catch (Exception ex)
		{
			throw ConfigurationApiExceptionUtilities.HandleException(ex);
		}
	}

	private static void ThrowIfPathInvalid(ConfigurationItemPath path)
	{
		if (path.IsFolder)
		{
			if (!string.Equals(path.ParentItemType, "System", StringComparison.Ordinal))
			{
				throw new PathInvalidException("Invalid parent. Path: " + path.Text);
			}
		}
		else if (!path.Id.IsGuid())
		{
			throw new PathInvalidException("Invalid id. Path: " + path.Text);
		}
	}

	public static ConfigurationItem ConvertToConfigurationItem(RecordingServer recordingServer)
	{
		ConfigurationItemPath configurationItemPath = ConfigurationItemPath.CreateFolderPath("System", string.Empty, "RecordingServerFolder");
		ConfigurationItemPath configurationItemPath2 = ConfigurationItemPath.CreateItemPath("RecordingServer", recordingServer.Id);
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "RecordingServer";
		configurationItem.UIHint = "DirectionHorizontal";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = recordingServer.Name;
		configurationItem.ParentPath = configurationItemPath.Text;
		configurationItem.Path = configurationItemPath2.Text;
		configurationItem.EnableProperty = GeneralPropertyDefinitions.GetEnabledProperty(recordingServer.Enabled, uiFollowsEnabled: false);
		List<Property> list = new List<Property>();
		list.Add(GeneralPropertyDefinitions.GetIdPropertyGuid(recordingServer.Id));
		list.Add(GeneralPropertyDefinitions.GetNameProperty(recordingServer.Name));
		list.Add(GeneralPropertyDefinitions.GetDescriptionProperty(recordingServer.Description));
		list.Add(GeneralPropertyDefinitions.GetLastModifiedProperty(recordingServer.LastModified));
		list.Add(RecordingServerPropertyDefinition.SynchronizationTimeProperty(recordingServer.SynchronizationTime));
		list.Add(RecordingServerPropertyDefinition.TimeZoneProperty(recordingServer.TimeZoneName));
		list.Add(RecordingServerPropertyDefinition.HostNameProperty(recordingServer.WebServerHostName));
		list.Add(RecordingServerPropertyDefinition.PortNumberProperty(recordingServer.WebServerPort));
		list.Add(RecordingServerPropertyDefinition.WebServerUriProperty(recordingServer.WebServerUri));
		list.Add(RecordingServerPropertyDefinition.ActiveWebServerUriProperty(recordingServer.ActiveWebServerUriConfiguration));
		list.Add(RecordingServerPropertyDefinition.PublicAccessEnabledProperty(recordingServer.PublicAccessEnabled));
		list.Add(RecordingServerPropertyDefinition.PublicWebServerHostnameProperty(recordingServer.PublicWebServerHostName));
		list.Add(RecordingServerPropertyDefinition.PublicWebServerPortProperty(recordingServer.PublicWebServerPort));
		list.Add(RecordingServerPropertyDefinition.MulticastServerAddressProperty(recordingServer.MulticastServerAddress));
		list.Add(RecordingServerPropertyDefinition.ShutdownOnStorageFailureProperty(recordingServer.ShutdownOnStorageFailure));
		list.Add(GeneralPropertyDefinitions.GetVersionProperty(recordingServer.Version));
		configurationItem.Properties = list.ToArray();
		configurationItem.MethodIds = new string[8] { "AddHardware", "GetOtherDevicesWithMediaOnRecorder", "HardwareScan", "HardwareScanExpress", "LoadTasks", "ReadDrivesOnRecorder", "ReadDirectoriesOnRecorder", "CreateDirectoryOnRecorder" };
		return configurationItem;
	}

	private static ConfigurationItem CreateInvokeInfoItemAddHardware(ConfigurationItem item, string methodId)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionHorizontal";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.WizardAddHardware;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		List<Property> list = new List<Property>();
		configurationItem.MethodIds = new string[1] { methodId };
		list.Add(HardwareAddressProperty(""));
		list.Add(GetSelectHardwareDriver());
		list.Add(UserNameProperty(""));
		list.Add(PasswordProperty(""));
		list.Add(CustomDataProperty(""));
		configurationItem.Properties = list.ToArray();
		return configurationItem;
	}

	private static ConfigurationItem CreateInvokeInfoItemRemoveRecordingServer(ConfigurationItem item, ConfigurationItem[] members, string methodId)
	{
		return MethodTypesHelper.CreateInvokeMemberSelectionInfo(methodId, ConfigurationApiResources.WizardRemoveRecordingServer, "WizardRemoveRecordingServer", ConfigurationApiResources.PropertyRemoveRecordingServer, item, members);
	}

	private static ConfigurationItem CreateInvokeInfoItemGetOtherDevicesWithMediaOnRecorder(ConfigurationItem item, string methodId)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionHorizontal";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.MethodGetOtherDevicesWithMediaOnRecorder;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		configurationItem.Properties = Array.Empty<Property>();
		configurationItem.MethodIds = new string[1] { methodId };
		return configurationItem;
	}

	private static ConfigurationItem CreateInvokeInfoItemReadDrivesOnRecorder(ConfigurationItem item, string methodId)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionHorizontal";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.MethodReadDrivesOnRecorder;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		configurationItem.Properties = Array.Empty<Property>();
		configurationItem.MethodIds = new string[1] { methodId };
		return configurationItem;
	}

	private static ConfigurationItem CreateInvokeInfoItemReadDirectoriesOnRecorder(ConfigurationItem item, string methodId)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionHorizontal";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.MethodReadDirectoriesOnRecorder;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		List<Property> list = new List<Property>();
		list.Add(RecordingServerPropertyDefinition.CurrentDirectoryProperty(""));
		configurationItem.Properties = list.ToArray();
		configurationItem.MethodIds = new string[1] { methodId };
		return configurationItem;
	}

	private static ConfigurationItem CreateInvokeInfoItemCreateDirectoryOnRecorder(ConfigurationItem item, string methodId)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionHorizontal";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.MethodCreateDirectoryOnRecorder;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		List<Property> list = new List<Property>();
		list.Add(RecordingServerPropertyDefinition.NewDirectoryNameProperty(""));
		configurationItem.Properties = list.ToArray();
		configurationItem.MethodIds = new string[1] { methodId };
		return configurationItem;
	}

	private static ConfigurationItem CreateInvokeInfoItemHardwareScan(ConfigurationItem item, string methodId)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionHorizontal";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.WizardScanHardware;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		configurationItem.MethodIds = new string[1] { methodId };
		List<Property> list = new List<Property>();
		list.Add(HardwareAddressProperty(""));
		list.Add(GetSelectHardwareDriverIds());
		list.Add(UserNameProperty(""));
		list.Add(PasswordProperty(""));
		list.Add(UseDefaultCredentialsProperty(useDefaultCredentials: false));
		configurationItem.Properties = list.ToArray();
		return configurationItem;
	}

	private static ConfigurationItem AddHardware(IGeminiRequestContext requestContext, ConfigurationItemPath recordingServerFolderPath, ConfigurationItem invokeInfoItem, ConfigurationItemPath invokeInfoItemPath)
	{
		ValidateArguments(requestContext, invokeInfoItemPath, invokeInfoItem);
		if (recordingServerFolderPath == null)
		{
			throw new ArgumentNullException("recordingServerFolderPath");
		}
		if (!string.Equals(recordingServerFolderPath.ItemType, "RecordingServerFolder", StringComparison.Ordinal))
		{
			throw new PathInvalidException("Invalid parent folder path: " + recordingServerFolderPath.Text);
		}
		((IEnumerable<Property>)invokeInfoItem.Properties).TryGetPropertyValue("HardwareAddress", out string value);
		((IEnumerable<Property>)invokeInfoItem.Properties).TryGetPropertyValue("HardwareDriverPath", out string value2);
		((IEnumerable<Property>)invokeInfoItem.Properties).TryGetPropertyValue("UserName", out string value3);
		((IEnumerable<Property>)invokeInfoItem.Properties).TryGetPropertyValue("Password", out string value4);
		((IEnumerable<Property>)invokeInfoItem.Properties).TryGetPropertyValue("CustomData", out string value5);
		Uri uri;
		try
		{
			uri = new UriBuilder(value).Uri;
		}
		catch (UriFormatException)
		{
			throw new ArgumentException("Invalid hardware address", "invokeInfoItem");
		}
		string id = invokeInfoItemPath.Id;
		if (RecordingServer.QueryRecordingServer(requestContext, id) == null)
		{
			throw new PathNotFoundException(recordingServerFolderPath.Text);
		}
		int driverNumber = BuildHardwareDriverNumber(requestContext, id, value2);
		invokeInfoItem = TaskHelperFunctions.CreateTaskInvokeResultConfigurationItem(Task.AddHardware(requestContext, id, uri.ToString(), value3, value4, driverNumber, string.Empty, value5), invokeInfoItem);
		return invokeInfoItem;
	}

	private static ConfigurationItem RemoveRecordingServer(IGeminiRequestContext requestContext, ConfigurationItem invokeInfoItem, ConfigurationItemPath invokeInfoItemPath)
	{
		ValidateArguments(requestContext, invokeInfoItemPath, invokeInfoItem);
		if (!string.Equals(invokeInfoItemPath.ItemType, "RecordingServerFolder", StringComparison.Ordinal))
		{
			throw new PathInvalidException("Invalid RecordingServerFolder path: " + invokeInfoItemPath.Text);
		}
		ConfigurationItemPath invokeMemberItemSelectionPath = MethodTypesHelper.GetInvokeMemberItemSelectionPath(invokeInfoItem);
		if (invokeMemberItemSelectionPath == null)
		{
			throw new PathNotFoundException("No Recording Server selected");
		}
		if (!Guid.TryParse(invokeMemberItemSelectionPath.Id, out var result))
		{
			throw new PathNotFoundException("No Recording Server selected");
		}
		RecorderData recorderData = RecorderData.QueryActiveRecorder(requestContext, result);
		if (recorderData == null)
		{
			throw new PathNotFoundException("Recording Server not found");
		}
		bool flag = Integration.IsRecorderOnline(invokeMemberItemSelectionPath.Id);
		MarkedDataDataContract[] array = MarkedData.QueryRecordingServerMarkedData(requestContext, invokeMemberItemSelectionPath.Id);
		bool flag2 = array.Any() || MarkedData.RecorderHasLockedDeviceDataDistributions(requestContext, invokeMemberItemSelectionPath.Id);
		if (flag2 && flag)
		{
			throw new ArgumentException("Cannot remove an online Recording Server that contains recordings that has been marked as evidence locked. Please remove all evidence locks first");
		}
		Property property = invokeInfoItem.Properties.FirstOrDefault((Property p) => p.Key == "UserAccept");
		if (property == null)
		{
			bool hasOtherMedia = HasOtherDevicesWithMedia(requestContext, invokeMemberItemSelectionPath.Id);
			bool storageUnknown = StorageInfoUnknown(requestContext, invokeMemberItemSelectionPath.Id);
			return CreateRemoveRecorderInfoResult(invokeInfoItem, flag2, hasOtherMedia, storageUnknown, invokeMemberItemSelectionPath);
		}
		if (!bool.TryParse(property.Value, out var result2))
		{
			throw new PropertyInvalidException("Value must be 'true' or 'false'", "UserAccept");
		}
		if (!result2)
		{
			throw new PropertyInvalidException("You must tick confirm acceptance");
		}
		if (array.Any())
		{
			if (!result2)
			{
				throw new ArgumentException("Cannot remove a Recording Server that contains recordings that has been marked as evidence locked, without acceptance");
			}
			UpdateOrDeleteMarkedData(requestContext, recorderData, array);
		}
		invokeInfoItem = TaskHelperFunctions.CreateTaskInvokeResultConfigurationItem(Task.DeleteRecordingServer(requestContext, invokeMemberItemSelectionPath.Id, result2), invokeInfoItem);
		return invokeInfoItem;
	}

	private static ConfigurationItem CreateRemoveRecorderInfoResult(ConfigurationItem invokeInfoItem, bool hasPossibleMarkedData, bool hasOtherMedia, bool storageUnknown, ConfigurationItemPath selectedRecorderPath)
	{
		string enumName = (storageUnknown ? "Warning" : "Ready");
		string enumName2 = (hasOtherMedia ? "Warning" : "Ready");
		string enumName3 = (hasPossibleMarkedData ? "MarkedData" : "NoMarkedData");
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionVertical";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = invokeInfoItem.DisplayName;
		configurationItem.ParentPath = invokeInfoItem.ParentPath;
		configurationItem.Path = invokeInfoItem.Path;
		configurationItem.MethodIds = invokeInfoItem.MethodIds;
		configurationItem.Properties = new Property[5]
		{
			RecordingServerPropertyDefinition.PreselectedRecorderProperty(selectedRecorderPath.Text),
			RecordingServerPropertyDefinition.DeleteEvidenceLockedDataProperty(enumName3),
			RecordingServerPropertyDefinition.StorageNotificationProperty(enumName),
			RecordingServerPropertyDefinition.OtherDeviceRecordingsProperty(enumName2),
			RecordingServerPropertyDefinition.UserAcceptProperty()
		};
		return configurationItem;
	}

	private static bool StorageInfoUnknown(IGeminiRequestContext requestContext, string recordingServerId)
	{
		RecordingStorage[] array = RecordingStorage.QueryRecordingStorages(requestContext, recordingServerId);
		foreach (RecordingStorage recordingStorage in array)
		{
			StorageDiskInformation recordingStorageInformation = StorageDiskInformation.GetRecordingStorageInformation(requestContext, recordingStorage.Id);
			if (recordingStorageInformation == null || recordingStorageInformation.UsedSpace == -1)
			{
				return true;
			}
		}
		return false;
	}

	private static bool HasOtherDevicesWithMedia(IGeminiRequestContext requestContext, string recordingServerId)
	{
		return Device.QueryOtherDevicesWithMediaOnRecorder(requestContext, recordingServerId)?.Any() ?? false;
	}

	private static void UpdateOrDeleteMarkedData(IGeminiRequestContext requestContext, RecorderData recordingServer, MarkedDataDataContract[] markedDataList)
	{
		string[] array = Device.QueryDeviceIdsGeneric(requestContext, recordingServer.Id.ToString(), null, DeviceTypeSearch.RecordingDevice);
		MarkedDataDataContract[] array2;
		foreach (string d in array)
		{
			array2 = markedDataList;
			foreach (MarkedDataDataContract markedDataDataContract in array2)
			{
				if (markedDataDataContract.DeviceIds != null)
				{
					markedDataDataContract.DeviceIds = markedDataDataContract.DeviceIds.Where((string dv) => !dv.Equals(d, StringComparison.OrdinalIgnoreCase)).ToArray();
				}
			}
		}
		array2 = markedDataList;
		foreach (MarkedDataDataContract markedDataDataContract2 in array2)
		{
			if (markedDataDataContract2.DeviceIds == null || markedDataDataContract2.DeviceIds.Length == 0)
			{
				MarkedData.DeleteMarkedData(requestContext, markedDataDataContract2.Id, (MarkedDataDefinitions.MarkedDataTypes)markedDataDataContract2.MarkedDataType);
			}
			else
			{
				MarkedData.UpdateMarkedData(requestContext, markedDataDataContract2.Id, markedDataDataContract2.MarkedDataType, markedDataDataContract2.DeviceIds, markedDataDataContract2.Reference, markedDataDataContract2.Header, markedDataDataContract2.Description, markedDataDataContract2.StartTime, markedDataDataContract2.TagTime, markedDataDataContract2.EndTime, markedDataDataContract2.UseRetention, markedDataDataContract2.RetentionType, markedDataDataContract2.RetentionUnits, markedDataDataContract2.RetentionExpire);
			}
		}
	}

	private static void ValidateArguments(IGeminiRequestContext requestContext, ConfigurationItemPath path, ConfigurationItem item)
	{
		if (requestContext == null)
		{
			throw new ArgumentNullException("requestContext");
		}
		if (item == null)
		{
			throw new ArgumentNullException("item");
		}
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
	}

	private static int BuildHardwareDriverNumber(IGeminiRequestContext requestContext, string recorderId, string hardwareDriverPath)
	{
		if (string.IsNullOrEmpty(hardwareDriverPath))
		{
			throw new ArgumentNullException(string.Format(CultureInfo.CurrentCulture, "Hardware Driver Path empty"));
		}
		try
		{
			ConfigurationItemPath configurationItemPath = ConfigurationItemPath.CreatePath(hardwareDriverPath);
			return HardwareDriver.QueryActiveHardwareDriver(requestContext, recorderId, configurationItemPath.Id).Number;
		}
		catch (HardwareDriverNotFoundException)
		{
			throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "Hardware Driver not found, Path:{0}", hardwareDriverPath));
		}
		catch (ArgumentException)
		{
			throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "Hardware Driver Path invalid:{0}", hardwareDriverPath));
		}
	}

	private static ConfigurationItem GetOtherDevicesWithMediaOnRecorder(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		ValidateArguments(requestContext, path, item);
		if (!string.Equals(path.ItemType, "RecordingServer", StringComparison.Ordinal))
		{
			throw new PathInvalidException("Invalid parent. Path: " + path.Text);
		}
		string id = path.Id;
		string[] array = Device.QueryOtherDevicesWithMediaOnRecorder(requestContext, id);
		List<ConfigurationItem> list = new List<ConfigurationItem>();
		if (array != null && array.Length != 0)
		{
			Device[] array2 = Device.QueryDevicesByIds(requestContext, array);
			foreach (Device device in array2)
			{
				ConfigurationItemPath parentPath = ConfigurationItemPath.CreateItemPath("Hardware", device.HardwareId);
				list.Add(DeviceConfigurationApi.ConvertToConfigurationItem(parentPath, device));
			}
		}
		ConfigurationItem configurationItem = MethodTypesHelper.CreateInvokeResult(item, null, "Result", "Success", true);
		configurationItem.Children = list.ToArray();
		configurationItem.ChildrenFilled = true;
		return configurationItem;
	}

	private static ConfigurationItem ReadDrivesOnRecorder(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		ValidateArguments(requestContext, path, item);
		if (!string.Equals(path.ItemType, "RecordingServer", StringComparison.Ordinal))
		{
			throw new PathInvalidException("Invalid parent. Path: " + path.Text);
		}
		string id = path.Id;
		string[] source = RecordingServer.ReadDrivesOnRecorder(requestContext, id);
		item.Properties = new Property[1] { RecordingServerPropertyDefinition.DrivesProperty(source.OrderBy((string d) => d)) };
		return MethodTypesHelper.CreateInvokeResult(item, null, "Result", "Success", true);
	}

	private static ConfigurationItem ReadDirectoriesOnRecorder(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		ValidateArguments(requestContext, path, item);
		if (!string.Equals(path.ItemType, "RecordingServer", StringComparison.Ordinal))
		{
			throw new PathInvalidException("Invalid parent. Path: " + path.Text);
		}
		string id = path.Id;
		((IEnumerable<Property>)item.Properties).TryGetPropertyValue("CurrentDirectory", out string value);
		try
		{
			string[] source = RecordingServer.ReadDirectoriesOnRecorder(requestContext, id, value);
			item.Properties = new Property[1] { RecordingServerPropertyDefinition.DirectoriesProperty(source.OrderBy((string d) => d)) };
			return MethodTypesHelper.CreateInvokeResult(item, null, "Result", "Success", true);
		}
		catch (DirectoryInvalidException)
		{
			throw new PropertyInvalidException("Invalid directory", "CurrentDirectory");
		}
		catch (RecordingServerOperationFailedException)
		{
			throw new PropertyInvalidException("Invalid directory", "CurrentDirectory");
		}
	}

	private static ConfigurationItem CreateDirectoryOnRecorder(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		ValidateArguments(requestContext, path, item);
		if (!string.Equals(path.ItemType, "RecordingServer", StringComparison.Ordinal))
		{
			throw new PathInvalidException("Invalid parent. Path: " + path.Text);
		}
		string id = path.Id;
		((IEnumerable<Property>)item.Properties).TryGetPropertyValue("NewDirectoryFullName", out string value);
		try
		{
			RecordingServer.CreateDirectoryOnRecorder(requestContext, id, value);
			return MethodTypesHelper.CreateInvokeResult(item, null, "Result", "Success", false);
		}
		catch (DirectoryInvalidException)
		{
			throw new PropertyInvalidException("Invalid directory", "NewDirectoryFullName");
		}
		catch (RecordingServerOperationFailedException)
		{
			throw new PropertyInvalidException("Invalid directory", "NewDirectoryFullName");
		}
	}

	private static ConfigurationItem HardwareScan(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		ValidateArguments(requestContext, path, item);
		if (!string.Equals(path.ItemType, "RecordingServer", StringComparison.Ordinal))
		{
			throw new PathInvalidException("Invalid type. Path: " + path.Text);
		}
		((IEnumerable<Property>)item.Properties).TryGetPropertyValue("HardwareAddress", out string value);
		((IEnumerable<Property>)item.Properties).TryGetPropertyValue("UserName", out string value2);
		((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Password", out string value3);
		((IEnumerable<Property>)item.Properties).TryGetPropertyValue("UseDefaultCredentials", out string value4);
		((IEnumerable<Property>)item.Properties).TryGetPropertyValue("HardwareDriverIds", out string value5);
		Uri uri;
		try
		{
			uri = new Uri(value);
		}
		catch (UriFormatException)
		{
			try
			{
				uri = new UriBuilder(value).Uri;
			}
			catch (ArgumentException)
			{
				throw new PropertyInvalidException("Invalid property", "HardwareAddress");
			}
			catch (UriFormatException)
			{
				throw new PropertyInvalidException("Invalid property", "HardwareAddress");
			}
		}
		bool useDefaultCredentials = ValidateUseDefaultCredentials(value4);
		string recordingServerId = ValidateRecorderId(requestContext, path);
		int[] array = null;
		if (!string.IsNullOrEmpty(value5))
		{
			try
			{
				array = (from f in value5.Trim().Split(';')
					select int.Parse(f, CultureInfo.InvariantCulture)).ToArray();
			}
			catch (OverflowException)
			{
				throw new PropertyInvalidException("Invalid property.", "HardwareDriverIds");
			}
			catch (FormatException)
			{
				throw new PropertyInvalidException("Invalid property.", "HardwareDriverIds");
			}
		}
		AddressDriverPairs addressDriverPairs = new AddressDriverPairs();
		if (array != null && array.Length == 1 && array[0] != -1)
		{
			addressDriverPairs.Items.Add(uri.DnsSafeHost, (ushort)uri.Port, array[0], uri.Scheme == Uri.UriSchemeHttps, (ushort)((uri.Scheme == Uri.UriSchemeHttps) ? ((uint)uri.Port) : 0u));
		}
		try
		{
			item = TaskHelperFunctions.CreateTaskInvokeResultConfigurationItem(Task.HardwareScan(requestContext, recordingServerId, uri.ToString(), value2, value3, useDefaultCredentials, array, addressDriverPairs), item);
			return item;
		}
		catch (ArgumentException ex6)
		{
			throw new PropertyInvalidException(ex6.Message, ex6.ParamName);
		}
	}

	public static Property HardwareAddressProperty(string address)
	{
		return new Property("HardwareAddress", address)
		{
			ValueType = "String",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyHardwareAddress,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyHardwareAddress",
			ToolTipTranslationId = "PropertyHardwareAddressToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 1
		};
	}

	private static Property GetSelectHardwareDriver()
	{
		Property property = new Property("HardwareDriverPath", "");
		property.ValueType = "Path";
		property.IsSettable = true;
		property.DisplayName = ConfigurationApiResources.PropertyHardwareDriver;
		property.ItemReference = true;
		property.ServerValidation = false;
		property.TranslationId = "PropertyHardwareDriver";
		property.ToolTipTranslationId = "PropertyHardwareDriverPathToolTip";
		property.UIImportance = UIImportance.Default;
		property.SortKey = 1;
		property.ValueTypeInfos = new ValueTypeInfo[1]
		{
			new ValueTypeInfo
			{
				Name = "PathItemType",
				Value = "HardwareDriver"
			}
		};
		return property;
	}

	private static Property GetSelectHardwareDriverIds()
	{
		Property property = new Property("HardwareDriverIds", "");
		property.ValueType = "Separator";
		property.IsSettable = true;
		property.DisplayName = ConfigurationApiResources.PropertyHardwareDriverIds;
		property.ItemReference = true;
		property.ServerValidation = false;
		property.TranslationId = "PropertyHardwareDriverIds";
		property.ToolTipTranslationId = "PropertyHardwareDriverIdsToolTip";
		property.UIImportance = UIImportance.Default;
		property.SortKey = 1;
		property.ValueTypeInfos = new ValueTypeInfo[1]
		{
			new ValueTypeInfo
			{
				Name = "Int",
				Value = "HardwareDriver"
			}
		};
		return property;
	}

	public static Property UserNameProperty(string userName)
	{
		return UserNameProperty("UserName", userName);
	}

	public static Property UserNameProperty(string userNameKey, string userName)
	{
		return new Property(userNameKey, userName)
		{
			ValueType = "String",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyUserName,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyUserName",
			ToolTipTranslationId = "PropertyUserNameToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 1
		};
	}

	public static Property CustomDataProperty(string customData)
	{
		return CustomDataProperty("CustomData", customData);
	}

	public static Property CustomDataProperty(string customDataKey, string customData)
	{
		return new Property(customDataKey, customData)
		{
			ValueType = "String",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyCustomData,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyCustomData",
			ToolTipTranslationId = "PropertyCustomDataToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 1
		};
	}

	public static Property PasswordProperty(string password)
	{
		return PasswordProperty("Password", password);
	}

	public static Property PasswordProperty(string passwordKey, string password)
	{
		return new Property(passwordKey, password)
		{
			ValueType = "String",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyPassword,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyPassword",
			ToolTipTranslationId = "PropertyHardwarePasswordTooltip",
			UIImportance = UIImportance.Password,
			SortKey = 1
		};
	}

	private static Property UseDefaultCredentialsProperty(bool useDefaultCredentials)
	{
		return new Property("UseDefaultCredentials", PropertyTypesHelper.GetValue(useDefaultCredentials))
		{
			ValueType = "Tick",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyRecordingServerUseDefaultCredentials,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyRecordingServerUseDefaultCredentials",
			ToolTipTranslationId = "PropertyRecordingServerUseDefaultCredentialsToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 1,
			ValueTypeInfos = PropertyTypesHelper.GetBoolValueTypeInfos()
		};
	}

	private static Property UseSecurePortProperty(bool useSecurePort)
	{
		return new Property("UseSecurePort", PropertyTypesHelper.GetValue(useSecurePort))
		{
			ValueType = "Tick",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyRecordingServerUseSecurePort,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyRecordingServerUseSecurePort",
			ToolTipTranslationId = "PropertyRecordingServerUseSecurePortToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 1,
			ValueTypeInfos = PropertyTypesHelper.GetBoolValueTypeInfos()
		};
	}
}
