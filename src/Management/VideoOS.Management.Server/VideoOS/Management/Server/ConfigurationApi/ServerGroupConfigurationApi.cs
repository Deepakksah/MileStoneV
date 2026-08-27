using System;
using System.Collections.Generic;
using VideoOS.ConfigurationAPI;
using VideoOS.ConfigurationAPI.ConfigurationFaultException;
using VideoOS.Management.Common;
using VideoOS.Utilities.Validation;

namespace VideoOS.Management.Server.ConfigurationApi;

[EntityName("ConfigAPILayoutGroup")]
[EntityName("ConfigAPILayoutGroupFolder")]
public class ServerGroupConfigurationApi : ConfigurationApiBase, IConfigurationApiSupport
{
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
		requestContext.NullCheck("", "ValidateItem", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\ServerGroup\\ServerGroupConfigurationApi.cs", 31);
		item.NullCheck("", "ValidateItem", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\ServerGroup\\ServerGroupConfigurationApi.cs", 32);
		path.NullCheck("", "ValidateItem", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\ServerGroup\\ServerGroupConfigurationApi.cs", 33);
		parentPath.NullCheck("", "ValidateItem", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\ServerGroup\\ServerGroupConfigurationApi.cs", 34);
		ThrowIfPathInvalid(path);
		ThrowIfPathInvalid(parentPath);
		string name;
		string description;
		return ValidateItemInternal(requestContext, item, path, out name, out description);
	}

	private static ValidateResult ValidateItemInternal(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, out string name, out string description)
	{
		name = null;
		description = null;
		try
		{
			if (!path.IsFolder)
			{
				if (item.Properties != null)
				{
					((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Name", out name);
					((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Description", out description);
				}
				ServerGroup.EnumStructureType enumStructureType;
				try
				{
					enumStructureType = ConvertItemTypeToStructureType(item.ItemType);
				}
				catch (PathNotFoundException)
				{
					throw new PathNotFoundException(path.Text);
				}
				try
				{
					if (ServerGroup.QueryServerGroupById(requestContext, path.Id).StructureType != enumStructureType)
					{
						throw new PathNotFoundException(path.Text);
					}
				}
				catch (ServerGroupNotFoundException)
				{
					throw new PathNotFoundException(path.Text);
				}
				ValidateResult validateResult = new ValidateResult();
				validateResult.ValidatedOk = true;
				validateResult.ResultItem = item;
				return validateResult;
			}
			throw new NotSupportedException();
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
	}

	public ConfigurationItem[] GetChildItems(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		requestContext.NullCheck("", "GetChildItems", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\ServerGroup\\ServerGroupConfigurationApi.cs", 100);
		path.NullCheck("", "GetChildItems", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\ServerGroup\\ServerGroupConfigurationApi.cs", 101);
		ThrowIfPathInvalid(path);
		ServerGroup.EnumStructureType enumStructureType;
		if (path.IsFolder)
		{
			try
			{
				enumStructureType = ConvertItemTypeToStructureType(path.ItemType);
			}
			catch (PathNotFoundException)
			{
				throw new PathNotFoundException(path.Text);
			}
			ServerGroup[] array = null;
			if (string.Equals(path.ParentItemType, "System", StringComparison.Ordinal))
			{
				array = ServerGroup.QueryServerGroupsByParentGroupId(requestContext, enumStructureType, "430C2FE2-B233-42DA-B508-1A22EF03383D");
			}
			else
			{
				try
				{
					if (ServerGroup.QueryServerGroupById(requestContext, path.Id).StructureType != enumStructureType)
					{
						throw new PathNotFoundException(path.Text);
					}
				}
				catch (ServerGroupNotFoundException)
				{
					throw new PathNotFoundException(path.Text);
				}
				array = ServerGroup.QueryServerGroupsByParentGroupId(requestContext, enumStructureType, path.Id);
			}
			List<ConfigurationItem> list = new List<ConfigurationItem>();
			ServerGroup[] array2 = array;
			foreach (ServerGroup serverGroup in array2)
			{
				ConfigurationItem item = ConvertToConfigurationItem(path, serverGroup);
				list.Add(item);
			}
			return list.ToArray();
		}
		try
		{
			enumStructureType = ConvertItemTypeToStructureType(path.ItemType);
		}
		catch (PathNotFoundException)
		{
			throw new PathNotFoundException(path.Text);
		}
		try
		{
			if (ServerGroup.QueryServerGroupById(requestContext, path.Id).StructureType != enumStructureType)
			{
				throw new PathNotFoundException(path.Text);
			}
		}
		catch (ServerGroupNotFoundException)
		{
			throw new PathNotFoundException(path.Text);
		}
		ConfigurationItem configurationItem = GroupConfigurationItemFactory.CreateFolderItem(ConvertStructureTypeToFolderItemType(enumStructureType), path);
		return new ConfigurationItem[1] { configurationItem };
	}

	public ConfigurationItem GetItem(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		requestContext.NullCheck("", "GetItem", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\ServerGroup\\ServerGroupConfigurationApi.cs", 188);
		path.NullCheck("", "GetItem", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\ServerGroup\\ServerGroupConfigurationApi.cs", 189);
		ThrowIfPathInvalid(path);
		if (path.IsFolder)
		{
			if (string.Equals(path.ParentItemType, "System", StringComparison.Ordinal))
			{
				return GroupConfigurationItemFactory.CreateFolderItem(path.ItemType, ConfigurationItemPath.CreateSystemPath());
			}
			string id = path.Id;
			try
			{
				ServerGroup.QueryServerGroupById(requestContext, id);
			}
			catch (ServerGroupNotFoundException)
			{
				throw new PathNotFoundException(path.Text);
			}
			ConfigurationItemPath parentPath = ConfigurationItemPath.CreateItemPath(path.ParentItemType, id);
			return GroupConfigurationItemFactory.CreateFolderItem(path.ItemType, parentPath);
		}
		ServerGroup.EnumStructureType structureType = ConvertItemTypeToStructureType(path.ItemType);
		string id2 = path.Id;
		ServerGroup serverGroup = null;
		try
		{
			serverGroup = ServerGroup.QueryServerGroupById(requestContext, id2);
		}
		catch (ServerGroupNotFoundException)
		{
			throw new PathNotFoundException(path.Text);
		}
		return ConvertToConfigurationItem(ConfigurationItemPath.CreateSystemFolderPath(ConvertStructureTypeToGroupFolderItemType(structureType)), serverGroup);
	}

	private static void ThrowIfPathInvalid(ConfigurationItemPath path)
	{
		if (path.IsFolder)
		{
			if (!string.Equals(path.ParentItemType, "System", StringComparison.Ordinal))
			{
				if (!path.Id.IsGuid())
				{
					throw new PathInvalidException("Invalid parent. Path: " + path.Text);
				}
				if (string.Equals(path.ItemType, "LayoutGroupFolder", StringComparison.Ordinal) && !string.Equals(path.ParentItemType, "LayoutGroup", StringComparison.Ordinal))
				{
					throw new PathInvalidException("Invalid parent. Path: " + path.Text);
				}
			}
		}
		else if (!path.Id.IsGuid())
		{
			throw new PathInvalidException("Invalid id. Path: " + path.Text);
		}
	}

	private static ServerGroup.EnumStructureType ConvertItemTypeToStructureType(string itemType)
	{
		if (itemType == "LayoutGroupFolder" || itemType == "LayoutGroup")
		{
			return ServerGroup.EnumStructureType.ViewLayoutGroup;
		}
		throw new PathNotFoundException();
	}

	private static ConfigurationItem ConvertToConfigurationItem(ConfigurationItemPath parentPath, ServerGroup serverGroup)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = ConvertStructureTypeToItemType(serverGroup.StructureType);
		configurationItem.UIHint = "DirectionHorizontal";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = serverGroup.Name;
		configurationItem.ParentPath = parentPath.Text;
		configurationItem.Path = ConfigurationItemPath.CreateItemPath(configurationItem.ItemType, serverGroup.Id).Text;
		List<Property> list = new List<Property>();
		list.Add(GeneralPropertyDefinitions.GetIdPropertyGuid(serverGroup.Id));
		list.Add(GeneralPropertyDefinitions.GetNameProperty(serverGroup.Name, isSettable: false));
		list.Add(GeneralPropertyDefinitions.GetDescriptionProperty(serverGroup.Description, isSettable: false));
		list.Add(GeneralPropertyDefinitions.GetLastModifiedProperty(serverGroup.LastModified));
		configurationItem.MethodIds = new string[0];
		configurationItem.Properties = list.ToArray();
		return configurationItem;
	}

	private static string ConvertStructureTypeToGroupFolderItemType(ServerGroup.EnumStructureType structureType)
	{
		string text = null;
		if (structureType == ServerGroup.EnumStructureType.ViewLayoutGroup)
		{
			return "LayoutGroupFolder";
		}
		return null;
	}

	private static string ConvertStructureTypeToFolderItemType(ServerGroup.EnumStructureType structureType)
	{
		string text = null;
		if (structureType == ServerGroup.EnumStructureType.ViewLayoutGroup)
		{
			return "LayoutFolder";
		}
		return null;
	}

	private static string ConvertStructureTypeToItemType(ServerGroup.EnumStructureType structureType)
	{
		string text = null;
		if (structureType == ServerGroup.EnumStructureType.ViewLayoutGroup)
		{
			return "LayoutGroup";
		}
		return null;
	}
}
