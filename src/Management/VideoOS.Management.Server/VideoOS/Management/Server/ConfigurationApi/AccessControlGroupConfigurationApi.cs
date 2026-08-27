using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VideoOS.ConfigurationAPI;
using VideoOS.ConfigurationAPI.ConfigurationFaultException;
using VideoOS.Management.Common;
using VideoOS.Management.Server.ConfigurationApi.AccessControl;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

[EntityName("ConfigAPIAccessControlGroup")]
[EntityName("ConfigAPIAccessControlGroupFolder")]
public class AccessControlGroupConfigurationApi : SecurityConfigurationApi, IConfigurationApiSupport
{
	private AccessControlGroupCache _accessControlGroupCache = new AccessControlGroupCache();

	public override bool SupportGetChildItemsWithItemTypes => true;

	protected override Dictionary<string, List<string>> ItemTypeActionsToExclude => new Dictionary<string, List<string>> { 
	{
		"AccessControlGroup",
		new List<string> { "ADMIN_SECURITY" }
	} };

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
		ThrowIfPathInvalid(parentPath);
		GroupMemberUtility.HasManagementPermission(requestContext, path.Id);
		if (!path.IsFolder)
		{
			ValidateResult validateResult = ValidateItemInternal(requestContext, item, path, out var name, out var description);
			if (!validateResult.ValidatedOk)
			{
				return validateResult;
			}
			try
			{
				ServerGroup serverGroup = ServerGroup.QueryServerGroupById(requestContext, path.Id);
				if (serverGroup == null || serverGroup.StructureType != ServerGroup.EnumStructureType.AccessControlGroup)
				{
					throw new PathNotFoundException(path.Text);
				}
				if (serverGroup.Name != name || serverGroup.Description != description)
				{
					serverGroup.Name = name;
					serverGroup.Description = description;
					ServerGroup.UpdateServerGroup(requestContext, serverGroup);
				}
				ConfigurationItem resultItem = null;
				validateResult = new ValidateResult();
				validateResult.ValidatedOk = true;
				validateResult.ResultItem = resultItem;
				return validateResult;
			}
			catch (ServerGroupNotFoundException)
			{
				throw new PathNotFoundException(path.Text);
			}
			catch (Exception)
			{
				return ErrorHelper.ErrorResultNameAlreadyExists();
			}
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
		ThrowIfPathInvalid(parentPath);
		GroupMemberUtility.HasManagementPermission(requestContext, path.Id);
		string name;
		string description;
		if (!path.IsFolder)
		{
			return ValidateItemInternal(requestContext, item, path, out name, out description);
		}
		throw new NotSupportedException();
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
				try
				{
					if (ServerGroup.QueryServerGroupById(requestContext, path.Id).StructureType != ServerGroup.EnumStructureType.AccessControlGroup)
					{
						throw new PathNotFoundException(path.Text);
					}
				}
				catch (ServerGroupNotFoundException)
				{
					throw new PathNotFoundException(path.Text);
				}
				ServerGroup.UpdateServerGroupValidation(requestContext, path.Id, name, description);
				if (!GroupMemberUtility.HasPermission(requestContext, path.Id))
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

	public override bool SetSecurityOnRoleOnly()
	{
		return true;
	}

	public override ConfigurationItem[] GetChildItems(IGeminiRequestContext requestContext, ConfigurationItemPath path, string[] itemTypes, ItemFilter[] itemFilters, int maxResult, int startIndex, string[] childItemTypes, ItemFilter[] childItemFilters)
	{
		if (requestContext == null)
		{
			throw new ArgumentNullException("requestContext");
		}
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		if (itemFilters == null)
		{
			throw new ArgumentNullException("itemFilters");
		}
		ThrowIfPathInvalid(path);
		if (path.IsFolder)
		{
			ItemFilter itemFilter = itemFilters.FirstOrDefault((ItemFilter f) => f.ItemType == "AccessControlGroup");
			ItemFilter itemFilter2 = itemFilters.FirstOrDefault((ItemFilter f) => f.ItemType == "AccessControlUnit");
			List<ItemFilter> list = new List<ItemFilter>();
			if (itemFilter2 != null)
			{
				list.Add(itemFilter2);
			}
			if (itemFilter != null)
			{
				list.Add(itemFilter);
			}
			else if (childItemFilters != null)
			{
				list.AddRange(childItemFilters);
			}
			if (path.ParentItemType == "System")
			{
				path = ConfigurationItemPath.CreateItemPath("AccessControlGroup", "6ACA51C5-A716-48A1-A2DC-AEEADA59C771");
			}
			return QueryItems(requestContext, path, itemFilter, maxResult, startIndex, list);
		}
		if (childItemFilters == null)
		{
			childItemFilters = Array.Empty<ItemFilter>();
		}
		ItemFilter itemFilter3 = itemFilters.FirstOrDefault((ItemFilter f) => f.ItemType == "AccessControlGroup");
		childItemFilters.FirstOrDefault((ItemFilter f) => f.ItemType == "AccessControlGroup");
		if (childItemFilters.Length == 0)
		{
			return base.GetChildItems(requestContext, path, itemTypes, itemFilters, maxResult, startIndex, childItemTypes, childItemFilters.ToArray());
		}
		ConstructGroupFolders(path, out var groupFolder, out var _);
		PrepareCache(requestContext, ConfigurationItemPath.CreatePath(groupFolder.Path), itemFilter3, childItemFilters.ToList(), out var _, out var includeMembers);
		ConfigurationItem configurationItem = new ConfigurationItem
		{
			Path = path.Text
		};
		new HashSet<string>().Add("AccessControlGroup");
		FillChildrenToGroupItem(requestContext, configurationItem, includeMembers, childItemFilters.ToList());
		return configurationItem.Children;
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
		if (path.IsFolder)
		{
			string parentGroupId = (string.Equals(path.ParentItemType, "System", StringComparison.Ordinal) ? "6ACA51C5-A716-48A1-A2DC-AEEADA59C771" : path.Id);
			ServerGroup[] array = ServerGroup.QueryServerGroupsByParentGroupId(requestContext, ServerGroup.EnumStructureType.AccessControlGroup, parentGroupId);
			bool flag = !requestContext.GetSecurityManagerService().IsAdministrator(requestContext);
			List<ConfigurationItem> list = new List<ConfigurationItem>();
			foreach (ServerGroup item2 in (IEnumerable<ServerGroup>)array)
			{
				if (item2.Id == "6ACA51C5-A716-48A1-A2DC-AEEADA59C771" || item2.Id == path.Id)
				{
					continue;
				}
				if (flag)
				{
					string fullPath = requestContext.SecurityObjectMapCache.GetFullPath(requestContext, VideoOS.Management.Server.ConfigurationApi.AccessControl.Constants.AccessControlSecurityNamespace, item2.Id);
					if (!requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, VideoOS.Management.Server.ConfigurationApi.AccessControl.Constants.AccessControlSecurityNamespace, "GENERIC_READ", fullPath))
					{
						continue;
					}
				}
				ConfigurationItem item = ConvertToConfigurationItem(path, item2);
				list.Add(item);
			}
			return list.ToArray();
		}
		CheckGroupExistsAndHasAccess(requestContext, path);
		ConfigurationItem groupFolder;
		ConfigurationItem memberFolder;
		return ConstructGroupFolders(path, out groupFolder, out memberFolder);
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
		if (path.IsFolder)
		{
			if (string.Equals(path.ParentItemType, "System", StringComparison.Ordinal))
			{
				return GroupConfigurationItemFactory.CreateFolderItem(path.ItemType, ConfigurationItemPath.CreateSystemPath());
			}
			string id = path.Id;
			try
			{
				if (ServerGroup.QueryServerGroupById(requestContext, id).StructureType != ServerGroup.EnumStructureType.AccessControlGroup)
				{
					throw new PathNotFoundException(path.Text);
				}
			}
			catch (ServerGroupNotFoundException)
			{
				throw new PathNotFoundException(path.Text);
			}
			if (!GroupMemberUtility.HasPermission(requestContext, path.Id))
			{
				throw new PathNotFoundException(path.Text);
			}
			ConfigurationItemPath parentPath = ConfigurationItemPath.CreateItemPath(path.ParentItemType, id);
			return GroupConfigurationItemFactory.CreateFolderItem(path.ItemType, parentPath);
		}
		string id2 = path.Id;
		if (string.IsNullOrWhiteSpace(id2) || !id2.IsGuid() || string.CompareOrdinal(id2, "6ACA51C5-A716-48A1-A2DC-AEEADA59C771") == 0)
		{
			throw new PathInvalidException("Invalid path: " + path.Text);
		}
		bool num = !requestContext.GetSecurityManagerService().IsAdministrator(requestContext);
		if (num && id2.Equals("9EA76B67-E0FF-4A8F-A904-F4DB1DB6A9CF", StringComparison.OrdinalIgnoreCase))
		{
			throw new PathNotFoundException(path.Text);
		}
		if (num)
		{
			string fullPath = requestContext.SecurityObjectMapCache.GetFullPath(requestContext, VideoOS.Management.Server.ConfigurationApi.AccessControl.Constants.AccessControlSecurityNamespace, id2);
			if (!requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, VideoOS.Management.Server.ConfigurationApi.AccessControl.Constants.AccessControlSecurityNamespace, "GENERIC_READ", fullPath))
			{
				throw new PathNotFoundException(path.Text);
			}
		}
		ServerGroup serverGroup = null;
		try
		{
			serverGroup = ServerGroup.QueryServerGroupById(requestContext, id2);
		}
		catch (ServerGroupNotFoundException)
		{
			throw new PathNotFoundException(path.Text);
		}
		string parentId = serverGroup.ParentId;
		ConfigurationItemPath configurationItemPath = null;
		configurationItemPath = ((string.CompareOrdinal(parentId, "6ACA51C5-A716-48A1-A2DC-AEEADA59C771") != 0 && !string.IsNullOrWhiteSpace(parentId)) ? ConfigurationItemPath.CreateFolderPath("AccessControlGroup", parentId, "AccessControlGroupFolder") : ConfigurationItemPath.CreateSystemFolderPath("AccessControlGroupFolder"));
		return ConvertToConfigurationItem(configurationItemPath, serverGroup);
	}

	public ConfigurationItem InvokeMethod(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath, string methodId)
	{
		if (item == null)
		{
			throw new ArgumentNullException("item");
		}
		if (methodId == null)
		{
			throw new ArgumentNullException("methodId");
		}
		GroupMemberUtility.HasManagementPermission(requestContext, path.Id);
		switch (methodId)
		{
		case "AddAccessControlGroup":
			if (string.Equals(item.ItemType, "InvokeInfo", StringComparison.Ordinal))
			{
				return AddGroup(requestContext, item, path);
			}
			return CreateItemAddGroup(item, methodId);
		case "RemoveAccessControlGroup":
		{
			if (string.Equals(item.ItemType, "InvokeInfo", StringComparison.Ordinal))
			{
				return RemoveGroup(requestContext, item, path);
			}
			IEnumerable<ConfigurationItem> source = from i in GetChildItems(requestContext, path).ToList()
				where i.Path.IndexOf("9EA76B67-E0FF-4A8F-A904-F4DB1DB6A9CF", StringComparison.OrdinalIgnoreCase) == -1
				select i;
			return CreateItemRemoveGroup(item, source.ToArray(), methodId);
		}
		case "ChangeSecurityPermissions":
			return InvokeSecurityMethod(requestContext, item, path, methodId);
		default:
			throw new ArgumentException("Unknown methodId:" + methodId);
		}
	}

	public override ConfigurationItem[] QueryItems(IGeminiRequestContext requestContext, ConfigurationItemPath folderPath, ItemFilter itemFilter, int maxResults, int startIndex, List<ItemFilter> childItemFilter)
	{
		if (requestContext == null)
		{
			throw new ArgumentNullException("requestContext");
		}
		if (folderPath == null)
		{
			throw new ArgumentNullException("folderPath");
		}
		PrepareCache(requestContext, folderPath, itemFilter, childItemFilter, out var _, out var includeMembers);
		if (folderPath.ParentItemType == "System")
		{
			folderPath = ConfigurationItemPath.CreateFolderPath("AccessControlGroup", "6ACA51C5-A716-48A1-A2DC-AEEADA59C771", "AccessControlGroupFolder");
		}
		ConfigurationItem configurationItem = new ConfigurationItem
		{
			Path = folderPath.Text
		};
		FillFolderChildrenRecursive(requestContext, configurationItem, includeMembers, childItemFilter);
		return configurationItem.Children;
	}

	private void PrepareCache(IGeminiRequestContext requestContext, ConfigurationItemPath folderPath, ItemFilter itemFilter, List<ItemFilter> childItemFilter, out HashSet<string> childGroupItemTypes, out bool includeMembers)
	{
		HashSet<string> hashSet = new HashSet<string>();
		childGroupItemTypes = new HashSet<string>();
		if (itemFilter != null)
		{
			hashSet.Add(itemFilter.ItemType);
		}
		includeMembers = false;
		if (childItemFilter != null)
		{
			foreach (ItemFilter item in childItemFilter)
			{
				if (item.ItemType == "AccessControlGroup")
				{
					hashSet.Add(item.ItemType);
					childGroupItemTypes.Add(item.ItemType);
				}
				if (item.ItemType == "AccessControlUnit")
				{
					includeMembers = true;
				}
			}
		}
		_accessControlGroupCache.Prepare(requestContext, includeMembers);
		_accessControlGroupCache.LoadGroupsForHierarchy(requestContext);
	}

	private void FillFolderChildrenRecursive(IGeminiRequestContext requestContext, ConfigurationItem groupFolder, bool includeMembers, List<ItemFilter> childItemFilter)
	{
		ConfigurationItemPath configurationItemPath = ConfigurationItemPath.CreatePath(groupFolder.Path);
		Guid parentGroupId = new Guid(configurationItemPath.Id);
		groupFolder.Children = BuildSubGroups(requestContext, _accessControlGroupCache.QuerySubGroups(parentGroupId), groupFolder);
		groupFolder.ChildrenFilled = true;
		ConfigurationItem[] children = groupFolder.Children;
		foreach (ConfigurationItem groupItem in children)
		{
			FillChildrenToGroupItem(requestContext, groupItem, includeMembers, childItemFilter);
		}
	}

	private void FillChildrenToGroupItem(IGeminiRequestContext requestContext, ConfigurationItem groupItem, bool includeMembers, List<ItemFilter> childItemFilter)
	{
		ConfigurationItemPath configurationItemPath = ConfigurationItemPath.CreatePath(groupItem.Path);
		_ = configurationItemPath.ItemType;
		groupItem.Children = ConstructGroupFolders(configurationItemPath, out var groupFolder, out var memberFolder);
		groupItem.ChildrenFilled = true;
		Guid groupId = new Guid(ConfigurationItemPath.CreatePath(groupItem.Path).Id);
		bool flag = childItemFilter.Any((ItemFilter f) => f.ItemType == "AccessControlGroup");
		if (includeMembers)
		{
			ConfigurationItem[] array = BuildAccessControlItems(requestContext, _accessControlGroupCache.QueryMembersInGroup(groupId), flag);
			ItemFilter itemFilter = childItemFilter?.FirstOrDefault((ItemFilter f) => f.ItemType == "AccessControlUnit");
			if (itemFilter != null)
			{
				List<ConfigurationItem> list = new List<ConfigurationItem>();
				ConfigurationItem[] array2 = array;
				foreach (ConfigurationItem item in array2)
				{
					if (ConfigurationApiBase.Match(item, itemFilter))
					{
						list.Add(item);
					}
				}
				memberFolder.Children = list.ToArray();
			}
			else
			{
				memberFolder.Children = array.ToArray();
			}
			memberFolder.ChildrenFilled = true;
		}
		if (flag)
		{
			FillFolderChildrenRecursive(requestContext, groupFolder, includeMembers, childItemFilter);
		}
	}

	private ConfigurationItem[] BuildAccessControlItems(IGeminiRequestContext requestContext, IEnumerable<Guid> memberIds, bool fillHierarchy)
	{
		new List<ConfigurationItem>();
		if (_accessControlGroupCache == null)
		{
			throw new ArgumentException("_accessControlGroupCache is null");
		}
		if (fillHierarchy)
		{
			_accessControlGroupCache.LoadMembersForGroup(requestContext, memberIds);
		}
		else
		{
			_accessControlGroupCache.LoadMembersForHierarchy(requestContext);
		}
		return _accessControlGroupCache.LoadMembersFromCache(memberIds).ToArray();
	}

	private ConfigurationItem[] BuildSubGroups(IGeminiRequestContext requestContext, IEnumerable<Guid> groupIds, ConfigurationItem parentFolder)
	{
		List<ConfigurationItem> list = new List<ConfigurationItem>();
		IEnumerable<ServerGroup> enumerable = _accessControlGroupCache.LoadGroupsFromCache(groupIds);
		ConfigurationItemPath parentPath = ConfigurationItemPath.CreatePath(parentFolder.Path);
		foreach (ServerGroup item2 in enumerable)
		{
			ConfigurationItem item = ConvertToConfigurationItem(parentPath, item2);
			list.Add(item);
		}
		return list.ToArray();
	}

	private static void CheckGroupExistsAndHasAccess(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		try
		{
			if (ServerGroup.QueryServerGroupById(requestContext, path.Id).StructureType != ServerGroup.EnumStructureType.AccessControlGroup)
			{
				throw new PathNotFoundException(path.Text);
			}
		}
		catch (ServerGroupNotFoundException)
		{
			throw new PathNotFoundException(path.Text);
		}
	}

	private static ConfigurationItem[] ConstructGroupFolders(ConfigurationItemPath parentPath, out ConfigurationItem groupFolder, out ConfigurationItem memberFolder)
	{
		groupFolder = GroupConfigurationItemFactory.CreateFolderItem("AccessControlGroupFolder", parentPath);
		memberFolder = GroupConfigurationItemFactory.CreateFolderItem("AccessControlUnitFolder", parentPath);
		if (!parentPath.Id.Equals("9EA76B67-E0FF-4A8F-A904-F4DB1DB6A9CF", StringComparison.OrdinalIgnoreCase))
		{
			memberFolder.MethodIds = new string[2] { "AddAccessControlGroupMember", "RemoveAccessControlGroupMember" };
			return new ConfigurationItem[2] { groupFolder, memberFolder };
		}
		return new ConfigurationItem[1] { memberFolder };
	}

	private static ConfigurationItem AddGroup(IGeminiRequestContext requestContext, ConfigurationItem configurationItem, ConfigurationItemPath parentPath)
	{
		if (requestContext == null)
		{
			throw new ArgumentNullException("requestContext");
		}
		if (configurationItem == null)
		{
			throw new ArgumentNullException("configurationItem");
		}
		if (parentPath == null)
		{
			throw new ArgumentNullException("parentPath");
		}
		if (!string.Equals(parentPath.ParentItemType, "System", StringComparison.Ordinal) && !string.Equals(parentPath.ParentItemType, "AccessControlGroup", StringComparison.Ordinal))
		{
			throw new PathInvalidException("Invalid parent folder path: " + parentPath.Text);
		}
		string text = "6ACA51C5-A716-48A1-A2DC-AEEADA59C771";
		string parentObjectId = "/";
		if (!string.Equals(parentPath.ParentItemType, "System", StringComparison.Ordinal))
		{
			text = parentPath.Id;
			parentObjectId = text;
		}
		string id = Guid.NewGuid().ToString();
		((IEnumerable<Property>)configurationItem.Properties).TryGetPropertyValue("GroupName", out string value);
		((IEnumerable<Property>)configurationItem.Properties).TryGetPropertyValue("GroupDescription", out string value2);
		ServerGroup serverGroup = new ServerGroup
		{
			Id = id,
			ParentId = text,
			Name = value,
			Description = value2,
			StructureType = ServerGroup.EnumStructureType.AccessControlGroup
		};
		try
		{
			serverGroup = ServerGroup.CreateServerGroup(requestContext, serverGroup);
			AccessControlGroupSecurity.RegisterSecurityObject(requestContext, parentObjectId, serverGroup.Id);
		}
		catch (ArgumentException)
		{
			throw;
		}
		catch (ServerGroupNotFoundException)
		{
			throw new PathNotFoundException(parentPath.Text);
		}
		ConfigurationItem configurationItem2 = ConvertToConfigurationItem(parentPath, serverGroup);
		return MethodTypesHelper.CreateInvokeResult(configurationItem, configurationItem2.Path, "Result", "Success", true);
	}

	private ConfigurationItem RemoveGroup(IGeminiRequestContext requestContext, ConfigurationItem configurationItem, ConfigurationItemPath parentPath)
	{
		if (requestContext == null)
		{
			throw new ArgumentNullException("requestContext");
		}
		if (configurationItem == null)
		{
			throw new ArgumentNullException("configurationItem");
		}
		if (parentPath == null)
		{
			throw new ArgumentNullException("parentPath");
		}
		if (string.Equals(parentPath.ParentItemType, "AccessControlGroup", StringComparison.Ordinal))
		{
			if (!parentPath.Id.IsGuid())
			{
				throw new PathInvalidException("Invalid parent folder path: " + parentPath.Text);
			}
		}
		else if (!string.Equals(parentPath.ParentItemType, "System", StringComparison.Ordinal))
		{
			throw new PathInvalidException("Invalid parent folder path: " + parentPath.Text);
		}
		string value = null;
		if (!((IEnumerable<Property>)configurationItem.Properties).TryGetPropertyValue("ItemSelection", out value))
		{
			throw new PropertyInvalidException("No selection.", "ItemSelection");
		}
		if (value == null)
		{
			throw ExceptionFactory.CreateArgumentNullException("ItemSelection");
		}
		bool result = false;
		if (((IEnumerable<Property>)configurationItem.Properties).TryGetPropertyValue("RemoveMembers", out string value2) && !bool.TryParse(value2, out result))
		{
			throw new PropertyInvalidException("Invalid value, expected True/False", "RemoveMembers");
		}
		ConfigurationItemPath configurationItemPath;
		try
		{
			configurationItemPath = ConfigurationItemPath.CreatePath(value);
		}
		catch (ArgumentException inner)
		{
			throw new PropertyInvalidException("Invalid path. " + value, "ItemSelection", inner);
		}
		if (configurationItemPath.IsFolder || !string.Equals("AccessControlGroup", configurationItemPath.ItemType, StringComparison.Ordinal) || !configurationItemPath.Id.IsGuid() || configurationItemPath.Id.Equals("9EA76B67-E0FF-4A8F-A904-F4DB1DB6A9CF", StringComparison.OrdinalIgnoreCase))
		{
			throw new PropertyInvalidException("Invalid group path. " + value, "ItemSelection");
		}
		bool flag = CheckForSubGroups(requestContext, configurationItemPath);
		if (!result && flag)
		{
			throw new PropertyInvalidException(value + " is not empty. Remove all sub-groups and members before deleting.", "ItemSelection");
		}
		string id = configurationItemPath.Id;
		try
		{
			if (result)
			{
				DeleteMembersAndGroups(requestContext, id);
			}
			else
			{
				ServerGroup.DeleteServerGroup(requestContext, id);
				AccessControlGroupSecurity.UnregisterSecurityObject(requestContext, id);
			}
		}
		catch (ServerGroupNotFoundException)
		{
			throw new PathNotFoundException(parentPath.Text);
		}
		return MethodTypesHelper.CreateInvokeResultWithoutItemPropertiesAndChildren(configurationItem);
	}

	private static void DeleteMembersAndGroups(IGeminiRequestContext requestContext, string groupId)
	{
		ServerGroup[] array = ServerGroup.QueryServerGroupsByParentGroupId(requestContext, ServerGroup.EnumStructureType.AccessControlGroup, groupId);
		foreach (ServerGroup serverGroup in array)
		{
			if (!(serverGroup.Id == groupId))
			{
				DeleteMembersAndGroups(requestContext, serverGroup.Id);
			}
		}
		List<Guid> list = GroupMemberUtility.QueryMembersInGroup(requestContext, groupId);
		MIPObjectProxy mIPObjectProxy = new MIPObjectProxy();
		foreach (Guid item in list)
		{
			MIPObject mipObject = mIPObjectProxy.QueryMIPObject(requestContext, item.ToString());
			List<Guid> memberIds = GroupMemberUtility.CollectAllACUnitChildren(requestContext, mIPObjectProxy, mipObject);
			GroupMemberUtility.RemoveMembers(requestContext, mIPObjectProxy, groupId, item.ToString(), memberIds);
			GroupMemberUtility.AddMembers(requestContext, "9EA76B67-E0FF-4A8F-A904-F4DB1DB6A9CF", item.ToString(), memberIds);
		}
		ServerGroup.DeleteServerGroup(requestContext, groupId);
		AccessControlGroupSecurity.UnregisterSecurityObject(requestContext, groupId);
	}

	private bool CheckForSubGroups(IGeminiRequestContext requestContext, ConfigurationItemPath groupPath)
	{
		if (!groupPath.Id.IsGuid())
		{
			throw new PathInvalidException("Invalid group path: " + groupPath.Text);
		}
		ConfigurationItemPath path = ConfigurationItemPath.CreateFolderPath("AccessControlGroup", groupPath.Id, "AccessControlGroupFolder");
		return GetChildItems(requestContext, path).Length != 0;
	}

	public static ConfigurationItem ConvertToConfigurationItem(ConfigurationItemPath parentPath, ServerGroup serverGroup)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "AccessControlGroup";
		configurationItem.UIHint = "DirectionHorizontal";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = serverGroup.Name;
		configurationItem.ParentPath = parentPath.Text;
		configurationItem.Path = ConfigurationItemPath.CreateItemPath(configurationItem.ItemType, serverGroup.Id).Text;
		configurationItem.MethodIds = new string[1] { "ChangeSecurityPermissions" };
		List<Property> list = new List<Property>();
		list.Add(GeneralPropertyDefinitions.GetIdPropertyGuid(serverGroup.Id));
		list.Add(GeneralPropertyDefinitions.GetNameProperty(serverGroup.Name));
		list.Add(GeneralPropertyDefinitions.GetDescriptionProperty(serverGroup.Description));
		list.Add(GeneralPropertyDefinitions.GetLastModifiedProperty(serverGroup.LastModified));
		configurationItem.Properties = list.ToArray();
		configurationItem.ChildrenFilled = false;
		return configurationItem;
	}

	private static ConfigurationItem CreateItemRemoveGroup(ConfigurationItem item, ConfigurationItem[] members, string methodId)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionVertical";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.WizardRemoveAccessControlGroup;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		List<Property> list = new List<Property>();
		list.Add(AccessControlGroupProperties.RemoveMembersProperty(removeMembers: false));
		configurationItem.MethodIds = new string[1] { methodId };
		list.Add(AccessControlGroupProperties.GetMembersAsEnumsProperty(members));
		configurationItem.Properties = list.ToArray();
		return configurationItem;
	}

	private static ConfigurationItem CreateItemAddGroup(ConfigurationItem item, string methodId)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionVertical";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.MethodAddAccessControlGroup;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		List<Property> list = new List<Property>();
		configurationItem.MethodIds = new string[1] { methodId };
		list.Add(AccessControlGroupProperties.GetSelectGroupName());
		list.Add(AccessControlGroupProperties.GetSelectGroupDescription());
		configurationItem.Properties = list.ToArray();
		return configurationItem;
	}

	public static ConfigurationItem CreateWizardItemAddMember(ConfigurationItem item, string methodId)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionVertical";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.WizardAddAccessControlGroupMember;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		List<Property> list = new List<Property>();
		configurationItem.MethodIds = new string[1] { methodId };
		string text = "AccessControlUnit";
		string displayName = string.Format(CultureInfo.InvariantCulture, ConfigurationApiResources.PropertySelectNewMember, ConfigurationApiResources.ResourceManager.GetString("ItemType" + text, CultureInfo.InvariantCulture));
		list.Add(GeneralPropertyDefinitions.GetPathSelectionProperty(item.ItemType, "ItemSelection", displayName));
		configurationItem.Properties = list.ToArray();
		return configurationItem;
	}

	public static ConfigurationItem CreateWizardItemRemoveMember(ConfigurationItem item, ConfigurationItem[] members, string methodId)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionVertical";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.WizardRemoveAccessControlGroupMember;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		List<Property> list = new List<Property>();
		configurationItem.MethodIds = new string[1] { methodId };
		list.Add(AccessControlGroupProperties.GetMembersAsEnumsProperty(members));
		configurationItem.Properties = list.ToArray();
		return configurationItem;
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
				if (string.Equals(path.ItemType, "AccessControlGroupFolder", StringComparison.Ordinal) && !string.Equals(path.ParentItemType, "AccessControlGroup", StringComparison.Ordinal))
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
}
