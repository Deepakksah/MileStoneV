using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VideoOS.ConfigurationAPI;
using VideoOS.ConfigurationAPI.ConfigurationFaultException;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

[EntityName("ConfigAPIFailoverGroup")]
[EntityName("ConfigAPIFailoverGroupFolder")]
public class FailoverGroupConfigurationApi : ConfigurationApiBase, IConfigurationApiSupport
{
	public ValidateResult SetItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		ConfigurationApiBase.ThrowIfAnyParameterNull(requestContext, item, path, parentPath);
		ThrowIfPathInvalid(path);
		ThrowIfPathInvalid(parentPath);
		ValidateResult validateResult = ValidateItemInternal(requestContext, item, path, out var name, out var description);
		if (!validateResult.ValidatedOk)
		{
			return validateResult;
		}
		ServerGroup serverGroup = ServerGroup.QueryServerGroupById(requestContext, path.Id);
		if (serverGroup == null)
		{
			throw new PathNotFoundException(path.Text);
		}
		if (name != null)
		{
			serverGroup.Name = name;
		}
		if (description != null)
		{
			serverGroup.Description = description;
		}
		ServerGroup.UpdateServerGroup(requestContext, serverGroup);
		ConfigurationItem resultItem = ConvertToFailoverGroupConfigurationItem(parentPath, serverGroup);
		validateResult = new ValidateResult();
		validateResult.ValidatedOk = true;
		validateResult.ResultItem = resultItem;
		return validateResult;
	}

	public ValidateResult ValidateItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		ConfigurationApiBase.ThrowIfAnyParameterNull(requestContext, item, path, parentPath);
		ThrowIfPathInvalid(path);
		ThrowIfPathInvalid(parentPath);
		if (path.IsFolder)
		{
			throw new NotSupportedException();
		}
		try
		{
			if (ServerGroup.QueryServerGroupById(requestContext, path.Id) == null)
			{
				throw new PathNotFoundException(path.Text);
			}
			string name;
			string description;
			return ValidateItemInternal(requestContext, item, path, out name, out description);
		}
		catch (ServerGroupNotFoundException)
		{
			throw new PathNotFoundException(path.Text);
		}
	}

	private static ValidateResult ValidateItemInternal(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, out string name, out string description)
	{
		name = null;
		description = null;
		if (path.IsFolder)
		{
			throw new NotSupportedException();
		}
		try
		{
			if (item.Properties != null)
			{
				((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Name", out name);
				((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Description", out description);
			}
			ThrowIfDuplicateName(requestContext, name, path.Id);
			ValidateResult validateResult = new ValidateResult();
			validateResult.ValidatedOk = true;
			validateResult.ResultItem = item;
			return validateResult;
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
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, path);
		ThrowIfPathInvalid(path);
		if (path.IsFolder)
		{
			string text = (string.Equals(path.ParentItemType, "System", StringComparison.Ordinal) ? "6A85FFC3-C134-4338-A072-024817CB5820" : path.Id);
			List<ConfigurationItem> list = new List<ConfigurationItem>();
			ServerGroup[] array = ServerGroup.QueryServerGroupsByParentGroupId(requestContext, ServerGroup.EnumStructureType.FailoverGroup, text);
			foreach (ServerGroup failoverGroup in array)
			{
				ConfigurationItem item = ConvertToFailoverGroupConfigurationItem(path, failoverGroup);
				list.Add(item);
			}
			FailoverServer[] array2 = FailoverServer.QueryFailoverServers(requestContext, text);
			foreach (FailoverServer failoverRecorder in array2)
			{
				list.Add(FailoverRecorderConfigurationApi.ConvertToFailoverRecorderConfigurationItem(path, failoverRecorder));
			}
			return list.ToArray();
		}
		bool num = string.Equals(path.ParentItemType, "System", StringComparison.Ordinal);
		ConfigurationItem configurationItem = GroupConfigurationItemFactory.CreateFolderItem("FailoverGroupFolder", path);
		ConfigurationItem configurationItem2 = GroupConfigurationItemFactory.CreateFolderItem("FailoverRecorderFolder", path);
		if (!num)
		{
			return new ConfigurationItem[1] { configurationItem2 };
		}
		return new ConfigurationItem[2] { configurationItem, configurationItem2 };
	}

	public ConfigurationItem GetItem(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, path);
		ThrowIfPathInvalid(path);
		if (path.IsFolder)
		{
			return GroupConfigurationItemFactory.CreateFolderItem("FailoverGroupFolder", ConfigurationItemPath.CreateSystemPath());
		}
		try
		{
			ServerGroup failoverGroup = ServerGroup.QueryServerGroupById(requestContext, path.Id);
			return ConvertToFailoverGroupConfigurationItem(new ConfigurationItemPathBuilder(ConfigurationItemPath.CreateSystemPath(), "FailoverGroupFolder").CreatePath(), failoverGroup);
		}
		catch (ServerGroupNotFoundException)
		{
			throw new PathNotFoundException(path.Text);
		}
	}

	public ConfigurationItem InvokeMethod(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath, string method)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, item, path);
		bool flag = string.Equals(item.ItemType, "InvokeInfo", StringComparison.Ordinal);
		switch (method)
		{
		case "AddFailoverGroup":
			if (!flag)
			{
				return CreateInvokeInfoAddFailoverGroup(item, method);
			}
			return AddFailoverGroup(requestContext, item, path);
		case "RemoveFailoverGroup":
			if (!flag)
			{
				return CreateInvokeInfoRemoveFailoverGroup(item, GetChildItems(requestContext, path), method);
			}
			return RemoveFailoverGroup(requestContext, item, path);
		case "MoveFailoverGroup":
			if (!flag)
			{
				return CreateInvokeInfoMoveFailoverGroup(item, method);
			}
			return MoveFailoverGroup(requestContext, item, path);
		default:
			throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Unknown methodId: {0}", method), method);
		}
	}

	private static ConfigurationItem CreateInvokeInfoAddFailoverGroup(ConfigurationItem item, string methodId)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionVertical";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.WizardAddFailoverGroup;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		configurationItem.MethodIds = new string[1] { methodId };
		configurationItem.Properties = new Property[2]
		{
			GeneralPropertyDefinitions.GetNameProperty("", isSettable: true),
			GeneralPropertyDefinitions.GetDescriptionProperty("")
		};
		return configurationItem;
	}

	private static ConfigurationItem AddFailoverGroup(IGeminiRequestContext requestContext, ConfigurationItem configurationItem, ConfigurationItemPath parentPath)
	{
		if (!string.Equals(parentPath.ParentItemType, "System", StringComparison.Ordinal))
		{
			throw new PathInvalidException("Invalid parent folder path: " + parentPath.Text);
		}
		string parentId = "6A85FFC3-C134-4338-A072-024817CB5820";
		string text = Guid.NewGuid().ToString();
		((IEnumerable<Property>)configurationItem.Properties).TryGetPropertyValue("Name", out string value);
		((IEnumerable<Property>)configurationItem.Properties).TryGetPropertyValue("Description", out string value2);
		ThrowIfDuplicateName(requestContext, value, text);
		ServerGroup serverGroup = new ServerGroup
		{
			Id = text,
			ParentId = parentId,
			Name = value,
			Description = value2,
			StructureType = ServerGroup.EnumStructureType.FailoverGroup,
			CustomProperties = new CustomProperty[0]
		};
		try
		{
			serverGroup = ServerGroup.CreateServerGroup(requestContext, serverGroup, checkPermissions: true);
		}
		catch (ArgumentException)
		{
			throw;
		}
		catch (ServerGroupNotFoundException)
		{
			throw new PathNotFoundException(parentPath.Text);
		}
		catch (FeatureNotLicensedException)
		{
			throw new LicenseException();
		}
		ConfigurationItem configurationItem2 = ConvertToFailoverGroupConfigurationItem(parentPath, serverGroup);
		return MethodTypesHelper.CreateInvokeResult(configurationItem2, configurationItem2.Path, "Result", "Success", true);
	}

	private static ConfigurationItem CreateInvokeInfoRemoveFailoverGroup(ConfigurationItem item, ConfigurationItem[] members, string methodId)
	{
		return MethodTypesHelper.CreateInvokeMemberSelectionInfo(methodId, ConfigurationApiResources.WizardRemoveFailoverGroup, "PropertyRemoveFailoverGroup", ConfigurationApiResources.PropertyRemoveFailoverGroup, item, members);
	}

	private static ConfigurationItem RemoveFailoverGroup(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		if (!string.Equals(path.ParentItemType, "System", StringComparison.Ordinal))
		{
			throw new PathInvalidException("Invalid parent. Path: " + path.Text);
		}
		ConfigurationItemPath invokeMemberItemSelectionPath = MethodTypesHelper.GetInvokeMemberItemSelectionPath(item);
		try
		{
			ServerGroup.QueryServerGroupById(requestContext, invokeMemberItemSelectionPath.Id);
			if (FailoverServer.QueryFailoverServers(requestContext, invokeMemberItemSelectionPath.Id).Any())
			{
				throw new ArgumentException("Cannot delete group with members");
			}
			ServerGroup.DeleteServerGroup(requestContext, invokeMemberItemSelectionPath.Id);
		}
		catch (ServerGroupNotFoundException)
		{
			throw new PathNotFoundException("Group not found");
		}
		catch (FeatureNotLicensedException)
		{
			throw new LicenseException();
		}
		return MethodTypesHelper.CreateInvokeResultWithoutItemPropertiesAndChildren(item);
	}

	private static ConfigurationItem CreateInvokeInfoMoveFailoverGroup(ConfigurationItem item, string methodId)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionVertical";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.WizardMoveFailoverGroup;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		configurationItem.MethodIds = new string[1] { methodId };
		configurationItem.Properties = new Property[3]
		{
			FailoverGroupPropertyDefinitions.SelectFailoverServerProperty(),
			FailoverGroupPropertyDefinitions.SelectFailoverGroupProperty(),
			FailoverRecorderPropertyDefinitions.GroupPositionProperty(-1, isSettable: true)
		};
		return configurationItem;
	}

	private static ConfigurationItem MoveFailoverGroup(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		if (!string.Equals(path.ParentItemType, "System", StringComparison.Ordinal))
		{
			throw new PathInvalidException("Invalid parent. Path: " + path.Text);
		}
		((IEnumerable<Property>)item.Properties).TryGetPropertyValue("FailoverServer", out string value);
		((IEnumerable<Property>)item.Properties).TryGetPropertyValue("FailoverGroup", out string value2);
		((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Position", out string value3);
		if (!int.TryParse(value3, out var result))
		{
			throw new PropertyInvalidException("Invalid integer", "Position");
		}
		ConfigurationItemPath configurationItemPath = ConfigurationItemPath.CreatePath(value);
		ConfigurationItemPath configurationItemPath2 = (string.IsNullOrEmpty(value2) ? null : ConfigurationItemPath.CreatePath(value2));
		try
		{
			string newParentId;
			if (configurationItemPath2 == null)
			{
				newParentId = "6A85FFC3-C134-4338-A072-024817CB5820";
			}
			else
			{
				newParentId = configurationItemPath2.Id;
				ServerGroup.QueryServerGroupById(requestContext, configurationItemPath2.Id);
				if (FailoverServer.QueryFailoverServer(requestContext, configurationItemPath.Id) == null)
				{
					throw new PathNotFoundException("Failover server not found");
				}
			}
			FailoverServer.Move(requestContext, configurationItemPath.Id, newParentId, result);
		}
		catch (ServerGroupNotFoundException)
		{
			throw new PathNotFoundException("Group not found");
		}
		return MethodTypesHelper.CreateInvokeResult(item, null, "Result", "Success", true);
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

	private static void ThrowIfDuplicateName(IGeminiRequestContext requestContext, string name, string objectId)
	{
		Validation.CheckName(name, "Name", allowNull: false);
		ServerGroup[] array = ServerGroup.QueryServerGroupsByParentGroupId(requestContext, ServerGroup.EnumStructureType.FailoverGroup, "6A85FFC3-C134-4338-A072-024817CB5820");
		foreach (ServerGroup serverGroup in array)
		{
			if (!string.Equals(serverGroup.Id, objectId, StringComparison.OrdinalIgnoreCase) && serverGroup.Name == name)
			{
				throw new ArgumentException("Duplicate name");
			}
		}
	}

	private static ConfigurationItem ConvertToFailoverGroupConfigurationItem(ConfigurationItemPath parentPath, ServerGroup failoverGroup)
	{
		ConfigurationItem obj = new ConfigurationItem
		{
			ItemType = "FailoverGroup",
			UIHint = "DirectionHorizontal",
			ItemCategory = ItemCategories.Item,
			DisplayName = failoverGroup.Name,
			ParentPath = parentPath.Text
		};
		obj.Path = ConfigurationItemPath.CreateItemPath(obj.ItemType, failoverGroup.Id).Text;
		obj.Properties = new List<Property>
		{
			GeneralPropertyDefinitions.GetIdPropertyGuid(failoverGroup.Id),
			GeneralPropertyDefinitions.GetNameProperty(failoverGroup.Name, isSettable: true),
			GeneralPropertyDefinitions.GetDescriptionProperty(failoverGroup.Description, isSettable: true),
			GeneralPropertyDefinitions.GetCreatedDateProperty(failoverGroup.CreatedDate),
			GeneralPropertyDefinitions.GetLastModifiedProperty(failoverGroup.LastModified)
		}.ToArray();
		List<string> list = new List<string>();
		obj.MethodIds = list.ToArray();
		return obj;
	}
}
