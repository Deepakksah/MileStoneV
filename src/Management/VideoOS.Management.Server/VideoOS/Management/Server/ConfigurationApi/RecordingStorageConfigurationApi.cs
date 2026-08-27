using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VideoOS.ConfigurationAPI;
using VideoOS.ConfigurationAPI.ConfigurationFaultException;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

[EntityName("ConfigAPIStorage")]
[EntityName("ConfigAPIStorageFolder")]
public class RecordingStorageConfigurationApi : ConfigurationApiBase, IConfigurationApiSupport
{
	public const string DiskPath = "DiskPath";

	public const string Password = "Password";

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
		string itemType = path.ItemType;
		if (string.Equals(itemType, "StorageFolder", StringComparison.OrdinalIgnoreCase))
		{
			string id = path.Id;
			RecordingStorage[] array = RecordingStorage.QueryRecordingStorages(requestContext, id);
			List<ConfigurationItem> list = new List<ConfigurationItem>();
			RecordingStorage[] array2 = array;
			foreach (RecordingStorage recordingStorage in array2)
			{
				list.Add(ConvertToStorageConfigurationItem(path, recordingStorage));
			}
			return list.ToArray();
		}
		if (string.Equals(itemType, "Storage", StringComparison.OrdinalIgnoreCase))
		{
			string id2 = path.Id;
			if (RecordingStorage.QueryRecordingStorage(requestContext, id2) == null)
			{
				throw new PathNotFoundException();
			}
			ConfigurationItem configurationItem = GroupConfigurationItemFactory.CreateFolderItem("ArchiveStorageFolder", path);
			return new ConfigurationItem[1] { configurationItem };
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
		if (string.Equals(path.ItemType, "StorageFolder", StringComparison.OrdinalIgnoreCase))
		{
			if (RecordingServer.QueryRecordingServer(requestContext, path.Id) == null)
			{
				throw new PathNotFoundException(path.Text);
			}
			ConfigurationItemPath parentPath = new ConfigurationItemPathBuilder
			{
				ItemType = "RecordingServer",
				Id = path.Id
			}.CreatePath();
			return GroupConfigurationItemFactory.CreateFolderItem("StorageFolder", parentPath);
		}
		if (string.Equals(path.ItemType, "Storage", StringComparison.Ordinal))
		{
			RecordingStorage recordingStorage = RecordingStorage.QueryRecordingStorage(requestContext, path.Id);
			if (recordingStorage == null)
			{
				throw new PathNotFoundException(path.Text);
			}
			return ConvertToStorageConfigurationItem(ConfigurationItemPath.CreateFolderPath("RecordingServer", recordingStorage.RecordingServerId, "StorageFolder"), recordingStorage);
		}
		throw new NotSupportedException();
	}

	public ConfigurationItem InvokeMethod(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath, string method)
	{
		ValidateArguments(item, path, method);
		bool flag = string.Equals(item.ItemType, "InvokeInfo", StringComparison.Ordinal);
		switch (method)
		{
		case "AddStorage":
			if (!flag)
			{
				return CreateInvokeInfoAddStorage(item, method);
			}
			return AddStorage(requestContext, item, path);
		case "RemoveStorage":
			if (!flag)
			{
				return CreateInvokeInfoRemoveStorage(item, GetChildItems(requestContext, path), method);
			}
			return RemoveStorage(requestContext, item, path);
		case "GetDevicesUsingStorage":
			if (!flag)
			{
				return CreateInvokeInfoGetDevicesUsingStorage(item, method);
			}
			return GetDevicesUsingStorage(requestContext, item, path);
		case "ReadStorageInformation":
			if (!flag)
			{
				return CreateInvokeInfoReadStorageInformation(item, method);
			}
			return ReadStorageInformation(requestContext, item, path);
		case "SetStorageAsDefault":
			if (!flag)
			{
				return CreateInvokeInfoSetStorageAsDefault(item, method);
			}
			return SetStorageAsDefault(requestContext, item, path);
		case "DisableEncryption":
			if (!flag)
			{
				return CreateInvokeInfoDisableEncryption(item, method);
			}
			return DisableEncryption(requestContext, item, path);
		case "EnableEncryption":
			if (!flag)
			{
				return CreateInvokeInfoEnableEncryption(item, method);
			}
			return EnableEncryption(requestContext, item, path);
		case "MoveRecordingStorage":
			if (!flag)
			{
				return CreateInvokeInfoMoveRecordingStorage(item, method);
			}
			return MoveRecordingStorage(requestContext, item, path);
		case "ChangeEncryptionMethod":
			if (!flag)
			{
				return CreateInvokeInfoChangeEncryptionMethod(item, method);
			}
			return ChangeEncryptionMethod(requestContext, item, path);
		case "ChangePasswordStorageEncryption":
			if (!flag)
			{
				return CreateInvokeInfoChangeEncryptionPassword(item, method);
			}
			return ChangeEncryptionPassword(requestContext, item, path);
		case "ValidateStorageEncryptionPassword":
			if (!flag)
			{
				return CreateInvokeInfoValidateEncryptionPasswordMethod(item, method);
			}
			return ValidateEncryptionPasswordMethod(requestContext, item, path);
		default:
			throw new ArgumentException("Unknown methodId:" + method);
		}
	}

	private static void ValidateArguments(ConfigurationItem item, ConfigurationItemPath path, string method)
	{
		if (item == null)
		{
			throw new ArgumentNullException("item");
		}
		if (method == null)
		{
			throw new ArgumentNullException("method");
		}
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		if (!item.MethodIds.Contains(method))
		{
			throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Invalid MethodId {0} on ItemType: {1}", method, item.ItemType), "method");
		}
	}

	private static ConfigurationItem CreateInvokeInfoChangeEncryptionMethod(ConfigurationItem item, string methodId)
	{
		Property property = RecordingStoragePropertyDefinition.EncryptionMethodProperty(EncryptionMethod.Strong);
		property.IsSettable = true;
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionVertical";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.MethodChangeEncryptionMethod;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		configurationItem.MethodIds = new string[1] { methodId };
		configurationItem.Properties = new Property[2]
		{
			property,
			RecordingStoragePropertyDefinition.PasswordProperty("")
		};
		return configurationItem;
	}

	private static ConfigurationItem CreateInvokeInfoChangeEncryptionPassword(ConfigurationItem item, string methodId)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionVertical";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.MethodChangeEncryptionPassword;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		configurationItem.MethodIds = new string[1] { methodId };
		configurationItem.Properties = new Property[2]
		{
			RecordingStoragePropertyDefinition.CurrentPasswordProperty(""),
			RecordingStoragePropertyDefinition.NewPasswordProperty("")
		};
		return configurationItem;
	}

	private static ConfigurationItem CreateInvokeInfoValidateEncryptionPasswordMethod(ConfigurationItem item, string methodId)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionVertical";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.MethodValidateStorageEncryptionPassword;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		configurationItem.MethodIds = new string[1] { methodId };
		configurationItem.Properties = new Property[1] { RecordingStoragePropertyDefinition.PasswordProperty("") };
		return configurationItem;
	}

	private static ConfigurationItem ChangeEncryptionMethod(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		ValidatePath(path);
		if (item.Properties == null || !((IEnumerable<Property>)item.Properties).TryGetPropertyValue("EncryptionMethod", out EncryptionMethod? value))
		{
			throw new PropertyInvalidException("No encryption method provided.");
		}
		if (item.Properties == null || !((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Password", out string value2))
		{
			throw new PropertyInvalidException("No password was provided for disabling encryption.", "Password");
		}
		return TaskHelperFunctions.CreateTaskInvokeResultConfigurationItem(Task.ChangeRecordingStorageEncryptionMethod(requestContext, path.Id, value.Value, value2), item);
	}

	private static ConfigurationItem ChangeEncryptionPassword(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		ValidatePath(path);
		if (item.Properties == null || !((IEnumerable<Property>)item.Properties).TryGetPropertyValue("oldPassword", out string value))
		{
			throw new PropertyInvalidException("No password was provided for changing the password.", "oldPassword");
		}
		if (item.Properties == null || !((IEnumerable<Property>)item.Properties).TryGetPropertyValue("newPassword", out string value2))
		{
			throw new PropertyInvalidException("No new password was provided for changing the password.", "newPassword");
		}
		Task task = null;
		try
		{
			task = Task.ChangeRecordingStorageEncryptionPassword(requestContext, path.Id, value, value2);
		}
		catch (RecordingStorageUpdateException)
		{
			throw new PathNotFoundException(path.Text);
		}
		return TaskHelperFunctions.CreateTaskInvokeResultConfigurationItem(task, item);
	}

	private static ConfigurationItem ValidateEncryptionPasswordMethod(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		ValidatePath(path);
		if (item.Properties == null || !((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Password", out string value))
		{
			throw new PropertyInvalidException("No password was provided for validate password.", "Password");
		}
		bool flag = false;
		try
		{
			flag = RecordingStorage.ValidateEncryptionPassword(requestContext, path.Id, value);
		}
		catch (RecordingStorageConfigureEncryptionException)
		{
			throw new PathNotFoundException(path.Text);
		}
		ConfigurationItem configurationItem = null;
		if (flag)
		{
			return MethodTypesHelper.CreateInvokeResult(item, item.Path, "Result", "Success", true);
		}
		return MethodTypesHelper.CreateInvokeResult(item, item.Path, "Result", "Error", true);
	}

	private static ConfigurationItem CreateInvokeInfoDisableEncryption(ConfigurationItem item, string methodId)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionVertical";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.WizardDisableEncryption;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		configurationItem.MethodIds = new string[1] { methodId };
		configurationItem.Properties = new Property[1] { RecordingStoragePropertyDefinition.PasswordProperty("") };
		return configurationItem;
	}

	private static ConfigurationItem DisableEncryption(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		ValidatePath(path);
		if (item.Properties == null || !((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Password", out string value))
		{
			throw new PropertyInvalidException("No password was provided for disabling encryption.", "Password");
		}
		Task[] tasks = Task.DisableRecordingStorageEncryption(requestContext, path.Id, value);
		return ConvertTasksToInvokeResult(item, tasks);
	}

	private static ConfigurationItem CreateInvokeInfoEnableEncryption(ConfigurationItem item, string methodId)
	{
		Property property = RecordingStoragePropertyDefinition.EncryptionMethodProperty(EncryptionMethod.Light);
		property.IsSettable = true;
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionVertical";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.WizardEnableEncryption;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		configurationItem.MethodIds = new string[1] { methodId };
		configurationItem.Properties = new Property[2]
		{
			RecordingStoragePropertyDefinition.PasswordProperty(""),
			property
		};
		return configurationItem;
	}

	private static ConfigurationItem EnableEncryption(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		ValidatePath(path);
		if (item.Properties == null)
		{
			throw new ArgumentException("Item properties collection is null");
		}
		if (!((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Password", out string value))
		{
			throw new PropertyInvalidException("No password was provided for enabling encryption.", "Password");
		}
		if (!((IEnumerable<Property>)item.Properties).TryGetPropertyValue("EncryptionMethod", out EncryptionMethod? value2))
		{
			throw new PropertyInvalidException("No encryption method was provided for enabling encryption.", "EncryptionMethod");
		}
		Task[] tasks = Task.EnableRecordingStorageEncryption(requestContext, path.Id, value2.Value, value);
		return ConvertTasksToInvokeResult(item, tasks);
	}

	private static ConfigurationItem ConvertTasksToInvokeResult(ConfigurationItem item, Task[] tasks)
	{
		List<Property> list = new List<Property>();
		foreach (Task task in tasks)
		{
			string name = string.Format(CultureInfo.InvariantCulture, "{0}TaskPath", task.Type.ToString());
			string text = ConfigurationItemPath.CreateItemPath("Task", task.Id).Text;
			list.Add(new Property(name, text));
		}
		item.Properties = list.ToArray();
		return MethodTypesHelper.CreateInvokeResult(item, null, "Result", "Success", true);
	}

	public ValidateResult SetItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		ConfigurationApiBase.ThrowIfAnyParameterNull(requestContext, item, path, parentPath);
		ThrowIfPathInvalid(path);
		ThrowIfPathInvalid(parentPath);
		if (string.Equals(path.ItemType, "Storage", StringComparison.Ordinal))
		{
			ValidateResult validateResult = ValidateItemInternal(requestContext, item, path, out var name, out var description, out var retainMinutes, out var signing, out var maxSize, out var encryption);
			if (!validateResult.ValidatedOk)
			{
				return validateResult;
			}
			try
			{
				RecordingStorage recordingStorage = new RecordingStorage
				{
					Id = path.Id,
					Name = name,
					Description = description,
					Signing = signing.Value,
					RetainMinutes = retainMinutes.Value,
					MaxSize = maxSize.Value,
					Encryption = encryption.Value
				};
				recordingStorage = RecordingStorage.UpdateRecordingStorage(requestContext, recordingStorage);
				ConfigurationItem resultItem = ConvertToStorageConfigurationItem(item.ParentPath, recordingStorage);
				validateResult = new ValidateResult();
				validateResult.ValidatedOk = true;
				validateResult.ResultItem = resultItem;
				return validateResult;
			}
			catch (RecordingStorageUpdateException)
			{
				throw new PathNotFoundException(path.Text);
			}
			catch (RecordingStorageRenameException ex2)
			{
				throw new PropertyInvalidException(ex2.Message, "Name");
			}
		}
		throw new NotSupportedException();
	}

	public ValidateResult ValidateItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		ConfigurationApiBase.ThrowIfAnyParameterNull(requestContext, item, path, parentPath);
		ThrowIfPathInvalid(path);
		ThrowIfPathInvalid(parentPath);
		if (string.Equals(path.ItemType, "Storage", StringComparison.Ordinal))
		{
			if (RecordingStorage.QueryRecordingStorage(requestContext, path.Id) == null)
			{
				throw new PathNotFoundException(path.Text);
			}
			string name;
			string description;
			int? retainMinutes;
			bool? signing;
			long? maxSize;
			EncryptionMethod? encryption;
			return ValidateItemInternal(requestContext, item, path, out name, out description, out retainMinutes, out signing, out maxSize, out encryption);
		}
		throw new NotSupportedException();
	}

	private static ValidateResult ValidateItemInternal(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, out string name, out string description, out int? retainMinutes, out bool? signing, out long? maxSize, out EncryptionMethod? encryption)
	{
		if (string.Equals(path.ItemType, "Storage", StringComparison.Ordinal))
		{
			name = null;
			description = null;
			retainMinutes = null;
			signing = null;
			maxSize = null;
			encryption = null;
			try
			{
				if (item.Properties != null)
				{
					((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Name", out name);
					((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Description", out description);
					((IEnumerable<Property>)item.Properties).TryGetPropertyValue("RetainMinutes", out retainMinutes);
					((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Signing", out signing);
					((IEnumerable<Property>)item.Properties).TryGetPropertyValue("MaxSize", out maxSize);
					((IEnumerable<Property>)item.Properties).TryGetPropertyValue("EncryptionMethod", out encryption);
				}
			}
			catch (ArgumentException exception)
			{
				ValidateResult validateResult = ErrorHelper.CreateValidateResult(exception, item.Properties);
				if (validateResult != null)
				{
					return validateResult;
				}
				throw;
			}
			if (retainMinutes.HasValue)
			{
				try
				{
					Validation.CheckRetainMinutes(retainMinutes.Value, "RetainMinutes");
				}
				catch (ArgumentOutOfRangeException)
				{
					ValidateResult validateResult2 = new ValidateResult();
					validateResult2.ErrorResults = new ErrorResult[1]
					{
						new ErrorResult
						{
							ErrorProperty = "RetainMinutes",
							ErrorText = string.Format(CultureInfo.InvariantCulture, ConfigurationApiResources.ErrorOutOfRange, ConfigurationApiResources.PropertyRecordingStorageRetainMinutes),
							ErrorTextId = "ErrorOutOfRange"
						}
					};
					validateResult2.ValidatedOk = false;
					validateResult2.ResultItem = null;
					return validateResult2;
				}
			}
			if (maxSize.HasValue)
			{
				try
				{
					Validation.CheckMaxSize(maxSize.Value, "MaxSize");
				}
				catch (ArgumentOutOfRangeException)
				{
					ValidateResult validateResult2 = new ValidateResult();
					validateResult2.ErrorResults = new ErrorResult[1]
					{
						new ErrorResult
						{
							ErrorProperty = "MaxSize",
							ErrorText = string.Format(CultureInfo.InvariantCulture, ConfigurationApiResources.ErrorOutOfRange, ConfigurationApiResources.PropertyRecordingStorageMaxSize),
							ErrorTextId = "ErrorOutOfRange"
						}
					};
					validateResult2.ValidatedOk = false;
					validateResult2.ResultItem = null;
					return validateResult2;
				}
			}
			try
			{
				RecordingStorage recordingStorage = RecordingStorage.QueryRecordingStorage(requestContext, path.Id);
				if (recordingStorage == null)
				{
					throw new PathNotFoundException(path.Text);
				}
				name = name ?? recordingStorage.Name;
				description = description ?? recordingStorage.Description;
				retainMinutes = retainMinutes ?? recordingStorage.RetainMinutes;
				maxSize = maxSize ?? recordingStorage.MaxSize;
				signing = signing ?? recordingStorage.Signing;
				encryption = encryption ?? recordingStorage.Encryption;
				RecordingStorage.UpdateRecordingStorageValidation(requestContext, path.Id, name, retainMinutes.Value, maxSize.Value, signing.Value, encryption.Value);
				return new ValidateResult
				{
					ValidatedOk = true,
					ResultItem = item
				};
			}
			catch (ArgumentNullException exception2)
			{
				ValidateResult validateResult3 = ErrorHelper.CreateValidateResult(exception2, item.Properties);
				if (validateResult3 != null)
				{
					return validateResult3;
				}
				throw;
			}
			catch (ArgumentException exception3)
			{
				ValidateResult validateResult4 = ErrorHelper.CreateValidateResult(exception3, item.Properties);
				if (validateResult4 != null)
				{
					return validateResult4;
				}
				throw;
			}
		}
		throw new NotSupportedException();
	}

	private static void ValidatePath(ConfigurationItemPath path)
	{
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		ThrowIfPathInvalid(path);
		if (!string.Equals(path.ItemType, "Storage", StringComparison.Ordinal))
		{
			throw new PathInvalidException("Invalid path. Path: " + path.Text);
		}
		if (path.IsFolder)
		{
			throw new PathInvalidException("Invalid path. Path: " + path.Text);
		}
	}

	private static void ThrowIfPathInvalid(ConfigurationItemPath path)
	{
		if (path.IsFolder)
		{
			if (!string.Equals(path.ParentItemType, "RecordingServer", StringComparison.Ordinal))
			{
				throw new PathInvalidException("Invalid parent. Path: " + path.Text);
			}
			if (!path.Id.IsGuid())
			{
				throw new PathInvalidException("Invalid id. Path: " + path.Text);
			}
		}
		else if (!path.Id.IsGuid())
		{
			throw new PathInvalidException("Invalid id. Path: " + path.Text);
		}
	}

	public static ConfigurationItem ConvertToStorageConfigurationItem(ConfigurationItemPath parentPath, RecordingStorage recordingStorage)
	{
		return ConvertToStorageConfigurationItem(parentPath.Text, recordingStorage);
	}

	private static ConfigurationItem ConvertToStorageConfigurationItem(string parentPath, RecordingStorage recordingStorage)
	{
		ConfigurationItem obj = new ConfigurationItem
		{
			ItemType = "Storage",
			UIHint = "DirectionHorizontal",
			ItemCategory = ItemCategories.Item,
			DisplayName = recordingStorage.Name,
			ParentPath = parentPath
		};
		obj.Path = ConfigurationItemPath.CreateItemPath(obj.ItemType, recordingStorage.Id).Text;
		obj.Properties = new List<Property>
		{
			GeneralPropertyDefinitions.GetIdPropertyGuid(recordingStorage.Id),
			GeneralPropertyDefinitions.GetNameProperty(recordingStorage.Name),
			GeneralPropertyDefinitions.GetDescriptionProperty(recordingStorage.Description),
			RecordingStoragePropertyDefinition.DiskPathProperty(recordingStorage.Path),
			GeneralPropertyDefinitions.GetCreatedDateProperty(recordingStorage.CreatedDate),
			GeneralPropertyDefinitions.GetLastModifiedProperty(recordingStorage.LastModified),
			RecordingStoragePropertyDefinition.RetainMinutesProperty(recordingStorage.RetainMinutes),
			RecordingStoragePropertyDefinition.MaxSizeProperty(recordingStorage.MaxSize),
			RecordingStoragePropertyDefinition.IsDefaultProperty(recordingStorage.IsDefault),
			RecordingStoragePropertyDefinition.SigningProperty(recordingStorage.Signing),
			RecordingStoragePropertyDefinition.EncryptionMethodProperty(recordingStorage.Encryption)
		}.ToArray();
		List<string> list = new List<string> { "GetDevicesUsingStorage", "ReadStorageInformation", "SetStorageAsDefault", "EnableEncryption", "MoveRecordingStorage" };
		if (!recordingStorage.Encryption.Equals(EncryptionMethod.None))
		{
			list.Add("ChangeEncryptionMethod");
			list.Add("ValidateStorageEncryptionPassword");
			list.Add("DisableEncryption");
			list.Add("ChangePasswordStorageEncryption");
		}
		obj.MethodIds = list.ToArray();
		return obj;
	}

	private static ConfigurationItem CreateInvokeInfoReadStorageInformation(ConfigurationItem item, string methodId)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionVertical";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.MethodReadStorageInformation;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		configurationItem.MethodIds = new string[1] { methodId };
		configurationItem.Properties = new Property[0];
		return configurationItem;
	}

	private static ConfigurationItem CreateInvokeInfoAddStorage(ConfigurationItem item, string methodId)
	{
		Property property = RecordingStoragePropertyDefinition.DiskPathProperty("");
		property.IsSettable = true;
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionVertical";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.WizardAddStorage;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		configurationItem.MethodIds = new string[1] { methodId };
		configurationItem.Properties = new Property[6]
		{
			GeneralPropertyDefinitions.GetNameProperty(""),
			GeneralPropertyDefinitions.GetDescriptionProperty(""),
			property,
			RecordingStoragePropertyDefinition.SigningProperty(signing: false),
			RecordingStoragePropertyDefinition.RetainMinutesProperty(1440),
			RecordingStoragePropertyDefinition.MaxSizeProperty(1024L)
		};
		return configurationItem;
	}

	private static ConfigurationItem CreateInvokeInfoRemoveStorage(ConfigurationItem item, ConfigurationItem[] members, string methodId)
	{
		return MethodTypesHelper.CreateInvokeMemberSelectionInfo(methodId, ConfigurationApiResources.WizardRemoveStorage, "PropertyRemoveStorage", ConfigurationApiResources.PropertyRemoveStorage, item, members);
	}

	private static ConfigurationItem CreateInvokeInfoGetDevicesUsingStorage(ConfigurationItem item, string methodId)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionHorizontal";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.MethodGetDevicesUsingStorage;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		configurationItem.Properties = new List<Property>().ToArray();
		configurationItem.MethodIds = new string[1] { methodId };
		return configurationItem;
	}

	private static ConfigurationItem CreateInvokeInfoSetStorageAsDefault(ConfigurationItem item, string methodId)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionHorizontal";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.MethodSetStorageAsDefault;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		configurationItem.Properties = new List<Property>().ToArray();
		configurationItem.MethodIds = new string[1] { methodId };
		return configurationItem;
	}

	private static ConfigurationItem CreateInvokeInfoMoveRecordingStorage(ConfigurationItem item, string methodId)
	{
		string value = null;
		if (item.Properties != null)
		{
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Path", out value);
		}
		Property property = RecordingStoragePropertyDefinition.DiskPathProperty(value);
		property.IsSettable = true;
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionVertical";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.WizardMoveRecordingStorage;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		configurationItem.MethodIds = new string[1] { methodId };
		configurationItem.Properties = new Property[1] { property };
		return configurationItem;
	}

	private static ConfigurationItem MoveRecordingStorage(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
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
		if (!string.Equals(path.ItemType, "Storage", StringComparison.Ordinal))
		{
			throw new PathInvalidException("Invalid parent. Path: " + path.Text);
		}
		if (item.Properties == null || !((IEnumerable<Property>)item.Properties).TryGetPropertyValue("DiskPath", out string value))
		{
			throw new PropertyInvalidException("Property not set", "DiskPath");
		}
		try
		{
			Validation.CheckPath(value, "DiskPath");
		}
		catch (ArgumentNullException)
		{
			throw new PropertyInvalidException("Property not valid", "DiskPath");
		}
		return TaskHelperFunctions.CreateTaskInvokeResultConfigurationItem(Task.MoveRecordingStorage(requestContext, path.Id, value), item);
	}

	private static ConfigurationItem AddStorage(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath invokeInfoItemPath)
	{
		if (requestContext == null)
		{
			throw new ArgumentNullException("requestContext");
		}
		if (item == null)
		{
			throw new ArgumentNullException("item");
		}
		if (invokeInfoItemPath == null)
		{
			throw new ArgumentNullException("invokeInfoItemPath");
		}
		if (!string.Equals(invokeInfoItemPath.ParentItemType, "RecordingServer", StringComparison.Ordinal))
		{
			throw new PathInvalidException("Invalid parent. Path: " + invokeInfoItemPath.Text);
		}
		string value = null;
		string value2 = null;
		string value3 = null;
		int? value4 = null;
		bool? value5 = null;
		long? value6 = null;
		EncryptionMethod? value7 = null;
		if (item.Properties != null)
		{
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Name", out value);
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Description", out value2);
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue("DiskPath", out value3);
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue("RetainMinutes", out value4);
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Signing", out value5);
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue("MaxSize", out value6);
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue("EncryptionMethod", out value7);
		}
		string id = invokeInfoItemPath.Id;
		if (RecordingServer.QueryRecordingServer(requestContext, id) == null)
		{
			throw new PathNotFoundException(invokeInfoItemPath.Text);
		}
		try
		{
			RecordingStorage recordingStorage = new RecordingStorage
			{
				RecordingServerId = invokeInfoItemPath.Id,
				Name = value,
				Description = value2,
				RetainMinutes = (value4 ?? 1440),
				Signing = (value5 == true),
				MaxSize = (value6 ?? 1024),
				Path = value3
			};
			recordingStorage = RecordingStorage.CreateRecordingStorage(requestContext, recordingStorage);
			ConfigurationItem configurationItem = ConvertToStorageConfigurationItem(invokeInfoItemPath, recordingStorage);
			return MethodTypesHelper.CreateInvokeResult(configurationItem, configurationItem.Path, "Result", "Success", true);
		}
		catch (RecordingStorageCreationException ex)
		{
			throw new PropertyInvalidException(ex.Message, "Name");
		}
	}

	private static ConfigurationItem RemoveStorage(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
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
		if (!string.Equals(path.ParentItemType, "RecordingServer", StringComparison.Ordinal))
		{
			throw new PathInvalidException("Invalid parent. Path: " + path.Text);
		}
		ConfigurationItemPath invokeMemberItemSelectionPath = MethodTypesHelper.GetInvokeMemberItemSelectionPath(item);
		try
		{
			Task.DeleteRecordingStorage(requestContext, invokeMemberItemSelectionPath.Id, deleteIfOffline: false);
		}
		catch (RemoveNonExistingRecordingStorageException)
		{
			throw new PathNotFoundException(invokeMemberItemSelectionPath.Text);
		}
		catch (RemoveDefaultRecordingStorageException ex2)
		{
			throw new PropertyInvalidException(ex2.Message, "Name");
		}
		return MethodTypesHelper.CreateInvokeResultWithoutItemPropertiesAndChildren(item);
	}

	private static ConfigurationItem GetDevicesUsingStorage(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
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
		if (!string.Equals(path.ItemType, "Storage", StringComparison.Ordinal))
		{
			throw new PathInvalidException("Invalid parent. Path: " + path.Text);
		}
		string id = path.Id;
		string[] array = Device.QueryDeviceIdsByRecordingStorage(requestContext, id, DeviceTypeSearch.All);
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

	private static ConfigurationItem ReadStorageInformation(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
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
		if (!string.Equals(path.ItemType, "Storage", StringComparison.Ordinal))
		{
			throw new PathInvalidException("Invalid parent. Path: " + path.Text);
		}
		StorageInformationConfigurationApi storageInformationConfigurationApi = new StorageInformationConfigurationApi();
		ConfigurationItemPath path2 = ConfigurationItemPath.CreateItemPath("StorageInformation", path.Id);
		ConfigurationItem item2 = storageInformationConfigurationApi.GetItem(requestContext, path2);
		return MethodTypesHelper.CreateInvokeResult(item2, item2.Path, "Result", "Success", true);
	}

	private static ConfigurationItem SetStorageAsDefault(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
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
		if (!string.Equals(path.ItemType, "Storage", StringComparison.Ordinal))
		{
			throw new PathInvalidException("Invalid parent path: " + path.Text);
		}
		try
		{
			RecordingStorage.SetRecordingStorageAsDefault(requestContext, path.Id);
		}
		catch (SetNonExistingRecordingStorageAsDefaultException)
		{
			throw new PathNotFoundException(path.Text);
		}
		return MethodTypesHelper.CreateInvokeResult(item, item.Path, "Result", "Success", true);
	}
}
