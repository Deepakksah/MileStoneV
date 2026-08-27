using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.ConfigurationAPI;
using VideoOS.ConfigurationAPI.ConfigurationFaultException;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

[EntityName("ConfigAPIArchiveStorageFolder")]
public class ArchiveStorageFolderConfigurationApi : ArchiveStorageConfigurationApiObjectBase
{
	private readonly IArchiveStorageFolderConfigurationApiBackend Backend;

	private const int DefaultRetainMinutes = 1440;

	private const long DefaultMaxSize = 1024L;

	private const double DefaultTargetFramerate = 5.0;

	private const bool DefaultFramerateReductionEnabled = false;

	public ArchiveStorageFolderConfigurationApi()
	{
		Backend = new DefaultBackend();
	}

	public ArchiveStorageFolderConfigurationApi(IArchiveStorageFolderConfigurationApiBackend backend)
	{
		Backend = backend;
	}

	public override ConfigurationItem[] GetChildItems(IGeminiRequestContext requestContext, ConfigurationItemPath path)
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
		return (from a in Backend.QueryArchiveStorages(requestContext, path.Id)
			select ArchiveStorageConfigurationApiObjectBase.ConvertToArchiveStorageConfigurationItem(path, a)).ToArray();
	}

	public override ConfigurationItem GetItem(IGeminiRequestContext requestContext, ConfigurationItemPath path)
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
		if (Backend.QueryRecordingStorage(requestContext, path.Id) == null)
		{
			throw new PathNotFoundException(path.Text);
		}
		ConfigurationItemPath parentPath = new ConfigurationItemPathBuilder
		{
			ItemType = "Storage",
			Id = path.Id
		}.CreatePath();
		return GroupConfigurationItemFactory.CreateFolderItem("ArchiveStorageFolder", parentPath);
	}

	public override ValidateResult SetItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		throw new NotSupportedException();
	}

	public override ValidateResult ValidateItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		throw new NotSupportedException();
	}

	protected override ConfigurationItem GetMethodInputParameters(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, string methodId)
	{
		ThrowIfPathInvalid(path);
		if (!(methodId == "AddArchiveStorage"))
		{
			if (methodId == "RemoveArchiveStorage")
			{
				return CreateInvokeInfoRemoveArchiveStorage();
			}
			throw new ArgumentException("Unknown methodId:" + methodId, "methodId");
		}
		return CreateInvokeInfoAddArchiveStorage();
		ConfigurationItem CreateInvokeInfoAddArchiveStorage()
		{
			string xml = new Schedule().ToXml();
			return new ConfigurationItem
			{
				ItemType = "InvokeInfo",
				UIHint = "DirectionVertical",
				ItemCategory = ItemCategories.Item,
				DisplayName = ConfigurationApiResources.WizardAddArchiveStorage,
				ParentPath = item.ParentPath,
				Path = item.Path,
				MethodIds = new string[1] { "AddArchiveStorage" },
				Properties = new Property[7]
				{
					GeneralPropertyDefinitions.GetNameProperty(""),
					GeneralPropertyDefinitions.GetDescriptionProperty(""),
					ArchiveStoragePropertyDefinition.DiskPathProperty("", isSettable: true),
					ArchiveStoragePropertyDefinition.TargetFramerateProperty(5.0),
					ArchiveStoragePropertyDefinition.FramerateReductionEnabledProperty(isEnabled: false),
					ArchiveStoragePropertyDefinition.RetainMinutesProperty(1440),
					ArchiveStoragePropertyDefinition.MaxSizeProperty(1024L)
				},
				Children = new ConfigurationItem[1] { ArchiveScheduleConfigurationApi.ConvertToScheduleConfigurationItem(item.Path, item.Path, xml) }
			};
		}
		ConfigurationItem CreateInvokeInfoRemoveArchiveStorage()
		{
			ConfigurationItem[] childItems = GetChildItems(requestContext, path);
			ConfigurationItem configurationItem = MethodTypesHelper.CreateInvokeMemberSelectionInfo("RemoveArchiveStorage", ConfigurationApiResources.WizardRemoveArchiveStorage, "PropertyRemoveArchiveStorage", ConfigurationApiResources.PropertyRemoveArchiveStorage, item, childItems);
			List<Property> list = configurationItem.Properties.ToList();
			list.Add(ArchiveStoragePropertyDefinition.DeleteIfOfflineProperty(deleteIfOffline: false));
			configurationItem.Properties = list.ToArray();
			return configurationItem;
		}
	}

	protected override ConfigurationItem ExecuteMethod(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, string methodId)
	{
		ThrowIfPathInvalid(path);
		if (!(methodId == "AddArchiveStorage"))
		{
			if (methodId == "RemoveArchiveStorage")
			{
				return RemoveArchiveStorage(requestContext, item, path);
			}
			throw new ArgumentException("Unknown methodId:" + methodId, "methodId");
		}
		return AddArchiveStorage(requestContext, item, path);
	}

	private ConfigurationItem AddArchiveStorage(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath invokeInfoItemPath)
	{
		ArchiveStorageConfigurationApiObjectBase.ParamNullCheck(requestContext, "requestContext");
		ArchiveStorageConfigurationApiObjectBase.ParamNullCheck(item, "item");
		ArchiveStorageConfigurationApiObjectBase.ParamNullCheck(invokeInfoItemPath, "invokeInfoItemPath");
		ThrowIfWrongParentItemType(invokeInfoItemPath);
		string value = null;
		string value2 = null;
		string value3 = null;
		int? value4 = null;
		double? value5 = null;
		bool? value6 = null;
		long? value7 = null;
		if (item.Properties != null)
		{
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Name", out value);
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Description", out value2);
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue("DiskPath", out value3);
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue("RetainMinutes", out value4);
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue("TargetFramerate", out value5);
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue("FramerateReductionEnabled", out value6);
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue("MaxSize", out value7);
		}
		string scheduleXml = GetScheduleXml(item);
		RecordingStorage recordingStorage = Backend.QueryRecordingStorage(requestContext, invokeInfoItemPath.Id);
		if (recordingStorage == null)
		{
			throw new PathNotFoundException(invokeInfoItemPath.Text);
		}
		try
		{
			ArchiveStorageDataContract archiveStorage = new ArchiveStorageDataContract
			{
				RecordingServerId = recordingStorage.RecordingServerId,
				Name = value,
				Description = value2,
				RetainMinutes = (value4 ?? 1440),
				TargetFramerate = (value5 ?? 5.0),
				FramerateReductionEnabled = (value6 == true),
				MaxSize = (value7 ?? 1024),
				Path = value3,
				RecordingStorageId = recordingStorage.Id,
				ArchiveSchedule = scheduleXml,
				Mounted = true
			};
			archiveStorage = Backend.CreateArchiveStorage(requestContext, archiveStorage);
			ConfigurationItem configurationItem = ArchiveStorageConfigurationApiObjectBase.ConvertToArchiveStorageConfigurationItem(invokeInfoItemPath, archiveStorage);
			return MethodTypesHelper.CreateInvokeResult(configurationItem, configurationItem.Path, "Result", "Success", true);
		}
		catch (ArchiveStorageCreationException ex)
		{
			throw new PropertyInvalidException(ex.Message, "Name");
		}
		catch (StorageRetentionTimeException ex2)
		{
			throw new PropertyInvalidException(ex2.Message, "RetainMinutes");
		}
	}

	private ConfigurationItem RemoveArchiveStorage(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		ArchiveStorageConfigurationApiObjectBase.ParamNullCheck(requestContext, "requestContext");
		ArchiveStorageConfigurationApiObjectBase.ParamNullCheck(item, "item");
		ArchiveStorageConfigurationApiObjectBase.ParamNullCheck(path, "path");
		ThrowIfWrongParentItemType(path);
		ConfigurationItemPath invokeMemberItemSelectionPath = MethodTypesHelper.GetInvokeMemberItemSelectionPath(item);
		Property property = item.Properties.FirstOrDefault((Property p) => p.Key == "DeleteIfOffline");
		bool result = false;
		if (property != null && !string.IsNullOrEmpty(property.Value) && !bool.TryParse(property.Value, out result))
		{
			throw new PropertyInvalidException("Property must be a boolean value.", "DeleteIfOffline");
		}
		try
		{
			Backend.DeleteArchiveStorage(requestContext, invokeMemberItemSelectionPath.Id, result);
		}
		catch (RemoveNonExistingArchiveStorageException)
		{
			throw new PathNotFoundException(invokeMemberItemSelectionPath.Text);
		}
		return MethodTypesHelper.CreateInvokeResultWithoutItemPropertiesAndChildren(item);
	}

	private static string GetScheduleXml(ConfigurationItem item)
	{
		string newScheduleXml = null;
		string text = new Schedule().ToXml();
		if (item.Children != null)
		{
			ConfigurationItem configurationItem = item.Children.Where((ConfigurationItem child) => child.ItemType == "ArchiveSchedule").FirstOrDefault();
			if (configurationItem != null)
			{
				ValidateResult validateResult = ArchiveScheduleConfigurationApi.ValidateItemInternal(configurationItem, text, out newScheduleXml);
				if (!validateResult.ValidatedOk)
				{
					if (validateResult.ErrorResults.Any())
					{
						throw new PropertyInvalidException(validateResult.ErrorResults[0].ErrorText, validateResult.ErrorResults[0].ErrorProperty);
					}
					throw new PropertyInvalidException("Invalid schedule");
				}
			}
		}
		return newScheduleXml ?? text;
	}

	protected override void ThrowIfPathInvalid(ConfigurationItemPath path)
	{
		ThrowIfFolderPathInvalid(path);
	}

	protected override void ThrowIfParentPathInvalid(ConfigurationItemPath path)
	{
		if (path.ItemType == "Storage" && path.Id.IsGuid())
		{
			return;
		}
		throw new PathInvalidException("Invalid archive storage folder parent path: " + path?.Text);
	}

	public static void ThrowIfFolderPathInvalid(ConfigurationItemPath path)
	{
		if (path != null && path.IsFolder && string.Equals(path.ParentItemType, "Storage", StringComparison.Ordinal) && path.Id.IsGuid())
		{
			return;
		}
		throw new PathInvalidException("Invalid archive storage folder path: " + path?.Text);
	}

	public static void ThrowIfWrongParentItemType(ConfigurationItemPath path)
	{
		if (!string.Equals(path.ParentItemType, "Storage", StringComparison.Ordinal))
		{
			throw new PathInvalidException("Parent item type must be Storage. Path: " + path.Text);
		}
	}
}
