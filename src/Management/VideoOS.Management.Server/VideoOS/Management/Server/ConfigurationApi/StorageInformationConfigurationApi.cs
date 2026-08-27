using System;
using System.Collections.Generic;
using VideoOS.ConfigurationAPI;
using VideoOS.ConfigurationAPI.ConfigurationFaultException;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server.ConfigurationApi;

[EntityName("ConfigAPIStorageInformation")]
[EntityName("ConfigAPIArchiveStorageInformation")]
public class StorageInformationConfigurationApi : ConfigurationApiBase, IConfigurationApiSupport
{
	public ConfigurationItem[] GetChildItems(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		return new ConfigurationItem[0];
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
		StorageDiskInformation storageDiskInformation = null;
		ConfigurationItemPathBuilder configurationItemPathBuilder = null;
		string itemType = path.ItemType;
		if (!(itemType == "StorageInformation"))
		{
			if (!(itemType == "ArchiveStorageInformation"))
			{
				throw new PathInvalidException("Invalid item type");
			}
			storageDiskInformation = StorageDiskInformation.GetArchiveStorageInformation(requestContext, path.Id);
			configurationItemPathBuilder = new ConfigurationItemPathBuilder(ConfigurationItemPath.CreateItemPath("ArchiveStorage", path.Id));
		}
		else
		{
			storageDiskInformation = StorageDiskInformation.GetRecordingStorageInformation(requestContext, path.Id);
			configurationItemPathBuilder = new ConfigurationItemPathBuilder(ConfigurationItemPath.CreateItemPath("Storage", path.Id));
		}
		if (storageDiskInformation == null)
		{
			throw new PathNotFoundException(path.Text);
		}
		return ConvertToConfigurationItem(configurationItemPathBuilder.CreatePath(), storageDiskInformation, path.ItemType);
	}

	public ConfigurationItem InvokeMethod(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath, string method)
	{
		throw new NotSupportedException();
	}

	public ValidateResult SetItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		throw new NotSupportedException();
	}

	public ValidateResult ValidateItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		return new ValidateResult
		{
			ValidatedOk = true,
			ResultItem = item
		};
	}

	private static ConfigurationItem ConvertToConfigurationItem(ConfigurationItemPath parentPath, StorageDiskInformation storageInformation, string itemType)
	{
		ConfigurationItem obj = new ConfigurationItem
		{
			ItemType = itemType,
			ItemCategory = ItemCategories.Item,
			UIHint = "",
			ParentPath = parentPath.Text
		};
		obj.Path = ConfigurationItemPath.CreateItemPath(obj.ItemType, parentPath.Id).Text;
		List<Property> list = new List<Property>
		{
			StorageInformationPropertyDefinition.AnyTablesWithDataProperty(storageInformation.AnyTablesWithData),
			StorageInformationPropertyDefinition.LastUpdatedProperty(storageInformation.LastUpdated.LocalDateTime)
		};
		if (storageInformation.IsAvailable)
		{
			list.Add(StorageInformationPropertyDefinition.LockedUsedSpaceProperty((int)(storageInformation.LockedUsedSpace / 1048576)));
			list.Add(StorageInformationPropertyDefinition.UsedSpaceProperty((int)(storageInformation.UsedSpace / 1048576)));
		}
		else
		{
			list.Add(StorageInformationPropertyDefinition.LockedUsedSpaceProperty(-1));
			list.Add(StorageInformationPropertyDefinition.UsedSpaceProperty(-1));
		}
		list.Add(StorageInformationPropertyDefinition.IsAvailableProperty(storageInformation.IsAvailable));
		list.Add(StorageInformationPropertyDefinition.IsMountedProperty(storageInformation.IsMounted));
		obj.Properties = list.ToArray();
		return obj;
	}

	private static void ThrowIfPathInvalid(ConfigurationItemPath path)
	{
		if (path.IsFolder)
		{
			throw new PathInvalidException("Invalid path. Path: " + path.Text);
		}
		if (!path.Id.IsGuid())
		{
			throw new PathInvalidException("Invalid id. Path: " + path.Text);
		}
	}
}
