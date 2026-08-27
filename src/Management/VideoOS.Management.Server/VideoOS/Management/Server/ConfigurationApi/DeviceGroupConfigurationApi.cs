using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VideoOS.ConfigurationAPI;
using VideoOS.ConfigurationAPI.ConfigurationFaultException;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

[EntityName("ConfigAPICameraGroup")]
[EntityName("ConfigAPICameraGroupFolder")]
[EntityName("ConfigAPIMicrophoneGroup")]
[EntityName("ConfigAPIMicrophoneGroupFolder")]
[EntityName("ConfigAPISpeakerGroup")]
[EntityName("ConfigAPISpeakerGroupFolder")]
[EntityName("ConfigAPIInputEventGroup")]
[EntityName("ConfigAPIInputEventGroupFolder")]
[EntityName("ConfigAPIOutputGroup")]
[EntityName("ConfigAPIOutputGroupFolder")]
[EntityName("ConfigAPIMetadataGroup")]
[EntityName("ConfigAPIMetadataGroupFolder")]
public class DeviceGroupConfigurationApi : ConfigurationApiBase, IConfigurationApiSupport
{
	private DeviceGroupCache _deviceGroupCache = new DeviceGroupCache();

	public const string GroupNameKey = "GroupName";

	public const string GroupDescriptionKey = "GroupDescription";

	public override bool SupportGetChildItemsWithItemTypes => true;

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
		if (!path.IsFolder)
		{
			ValidateResult validateResult = ValidateItemInternal(requestContext, item, path, out var name, out var description);
			if (!validateResult.ValidatedOk)
			{
				return validateResult;
			}
			try
			{
				DeviceGroup deviceGroup = DeviceGroup.UpdateDeviceGroup(requestContext, path.Id, name, description);
				ConfigurationItem resultItem = ConvertToConfigurationItem(parentPath, deviceGroup);
				validateResult = new ValidateResult();
				validateResult.ValidatedOk = true;
				validateResult.ResultItem = resultItem;
				return validateResult;
			}
			catch (DeviceGroupUpdateGroupNotFoundException)
			{
				throw new PathNotFoundException(path.Text);
			}
			catch (DeviceGroupUpdateNameAlreadyExistException)
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
		if (!path.IsFolder)
		{
			DeviceType deviceType = ConvertItemTypeToDeviceType(path.ItemType);
			try
			{
				if (DeviceGroup.QueryDeviceGroupById(requestContext, path.Id, deviceType).DeviceType != deviceType)
				{
					throw new PathNotFoundException(path.Text);
				}
			}
			catch (DeviceGroupNotFoundException)
			{
				throw new PathNotFoundException(path.Text);
			}
			string name;
			string description;
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
				DeviceType deviceType = ConvertItemTypeToDeviceType(path.ItemType);
				try
				{
					if (DeviceGroup.QueryDeviceGroupById(requestContext, path.Id, deviceType).DeviceType != deviceType)
					{
						throw new PathNotFoundException(path.Text);
					}
				}
				catch (DeviceGroupNotFoundException)
				{
					throw new PathNotFoundException(path.Text);
				}
				DeviceGroup.UpdateDeviceGroupValidation(requestContext, path.Id, name, description);
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
			DeviceTypeRelation relation = DeviceTypeRelation.Relations.FirstOrDefault((DeviceTypeRelation r) => string.Equals(r.GroupFolderType, path.ItemType, StringComparison.Ordinal));
			if (relation == null)
			{
				throw new PathInvalidException("DeviceGroupConfigurationApi: Unexpected item type. " + path.Text);
			}
			ItemFilter itemFilter = itemFilters.FirstOrDefault((ItemFilter f) => f.ItemType == relation.GroupType);
			List<ItemFilter> list = new List<ItemFilter>();
			if (itemFilter != null)
			{
				list.Add(itemFilter);
			}
			else if (childItemFilters != null)
			{
				list.AddRange(childItemFilters);
			}
			return QueryItems(requestContext, path, itemFilter, maxResult, startIndex, list);
		}
		DeviceTypeRelation relation2 = DeviceTypeRelation.Relations.FirstOrDefault((DeviceTypeRelation r) => string.Equals(r.GroupType, path.ItemType, StringComparison.Ordinal));
		if (relation2 == null)
		{
			throw new PathInvalidException("DeviceGroupConfigurationApi: Unexpected item type. " + path.Text);
		}
		if (childItemFilters == null)
		{
			childItemFilters = new ItemFilter[0];
		}
		ItemFilter itemFilter2 = itemFilters.FirstOrDefault((ItemFilter f) => f.ItemType == relation2.ItemType);
		childItemFilters.FirstOrDefault((ItemFilter f) => f.ItemType == relation2.ItemType);
		if (!childItemFilters.Any())
		{
			return base.GetChildItems(requestContext, path, itemTypes, itemFilters, maxResult, startIndex, childItemTypes, childItemFilters.ToArray());
		}
		ConstructDeviceGroupFolders(ConvertItemTypeToDeviceType(path.ItemType), path, out var deviceGroupFolder, out var _);
		bool isAdministrator = ConfigurationApiBase.HasGeneralReadRights(requestContext);
		PrepareCache(requestContext, ConfigurationItemPath.CreatePath(deviceGroupFolder.Path), itemFilter2, childItemFilters.ToList(), isAdministrator, out var _, out var includeMembers);
		ConfigurationItem configurationItem = new ConfigurationItem
		{
			Path = path.Text
		};
		HashSet<string> hashSet = new HashSet<string>();
		foreach (string text in childItemTypes)
		{
			if (IsDeviceGroup(text))
			{
				hashSet.Add(text);
			}
		}
		FillChildrenToGroupItem(requestContext, configurationItem, isAdministrator, hashSet, includeMembers, childItemFilters.ToList());
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
			DeviceType deviceType = ConvertItemTypeToDeviceType(path.ItemType);
			string text = ConfigurationApiBase.RemoveFolderPartFromItemType(path.ItemType);
			bool flag = string.Equals(path.ParentItemType, "System", StringComparison.Ordinal);
			IEnumerable<DeviceGroup> enumerable;
			if (ConfigurationApiBase.HasGeneralReadRights(requestContext))
			{
				string[] ids;
				if (flag)
				{
					ids = DeviceGroup.QueryDeviceGroupIds(requestContext, "909749AE-88C4-4780-A381-383D691E98FA", deviceType);
				}
				else
				{
					CheckGroupExistsAndHasAccess(requestContext, path, deviceType);
					ids = DeviceGroup.QueryDeviceGroupIds(requestContext, path.Id, deviceType);
				}
				enumerable = DeviceGroup.QueryDeviceGroups(requestContext, ids);
			}
			else
			{
				IEnumerable<Guid> enumerable2;
				if (flag)
				{
					_deviceGroupCache.Prepare(requestContext, isAdministrator: false, new HashSet<string> { text }, includeMembers: false);
					enumerable2 = _deviceGroupCache.QuerySubGroups(text, Guid.Empty);
					_deviceGroupCache.LoadGroupsForList(requestContext, text, enumerable2);
				}
				else
				{
					CheckGroupExistsAndHasAccess(requestContext, path, deviceType);
					enumerable2 = _deviceGroupCache.QuerySubGroups(text, new Guid(path.Id));
					_deviceGroupCache.LoadGroupsForList(requestContext, text, enumerable2);
				}
				enumerable = _deviceGroupCache.LoadDeviceGroupsFromCache(text, enumerable2);
			}
			List<ConfigurationItem> list = new List<ConfigurationItem>();
			foreach (DeviceGroup item2 in enumerable)
			{
				ConfigurationItem item = ConvertToConfigurationItem(path, item2);
				list.Add(item);
			}
			return list.ToArray();
		}
		DeviceType type = ConvertItemTypeToDeviceType(path.ItemType);
		CheckGroupExistsAndHasAccess(requestContext, path, type);
		ConfigurationItem deviceGroupFolder;
		ConfigurationItem deviceFolder;
		return ConstructDeviceGroupFolders(type, path, out deviceGroupFolder, out deviceFolder);
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
				DeviceType deviceType = ConvertItemTypeToDeviceType(path.ItemType);
				DeviceGroup deviceGroup = DeviceGroup.QueryDeviceGroupById(requestContext, id, deviceType);
				if (!DeviceTypeRelation.Relations.Any((DeviceTypeRelation r) => r.DeviceType == deviceGroup.DeviceType && string.Equals(r.GroupType, path.ParentItemType, StringComparison.Ordinal)))
				{
					throw new PathNotFoundException(path.Text);
				}
			}
			catch (DeviceGroupNotFoundException)
			{
				throw new PathNotFoundException(path.Text);
			}
			ConfigurationItemPath parentPath = ConfigurationItemPath.CreateItemPath(path.ParentItemType, id);
			return GroupConfigurationItemFactory.CreateFolderItem(path.ItemType, parentPath);
		}
		DeviceType deviceType2 = ConvertItemTypeToDeviceType(path.ItemType);
		string id2 = path.Id;
		DeviceGroup deviceGroup2 = null;
		try
		{
			deviceGroup2 = DeviceGroup.QueryDeviceGroupById(requestContext, id2, deviceType2);
		}
		catch (DeviceGroupNotFoundException)
		{
			throw new PathNotFoundException(path.Text);
		}
		string parentGroupId = deviceGroup2.ParentGroupId;
		ConfigurationItemPath configurationItemPath = null;
		string folderType = ConvertDeviceTypeToGroupFolderItemType(deviceType2);
		configurationItemPath = ((!string.IsNullOrEmpty(parentGroupId)) ? ConfigurationItemPath.CreateFolderPath(ConvertDeviceTypeToItemType(deviceGroup2.DeviceType), parentGroupId, folderType) : ConfigurationItemPath.CreateSystemFolderPath(folderType));
		return ConvertToConfigurationItem(configurationItemPath, deviceGroup2);
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
		if (!item.MethodIds.Contains(methodId))
		{
			throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Invalid MethodId {0} on ItemType: {1}", methodId, item.ItemType), "methodId");
		}
		if (!path.Id.Equals(parentPath.Id))
		{
			throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "ParentPath or Path invalid"), "parentPath");
		}
		if (!(methodId == "AddDeviceGroup"))
		{
			if (methodId == "RemoveDeviceGroup")
			{
				if (string.Equals(item.ItemType, "InvokeInfo", StringComparison.Ordinal))
				{
					return RemoveDeviceGroup(requestContext, item, path);
				}
				return CreateItemRemoveGroup(item, GetChildItems(requestContext, path), methodId);
			}
			throw new ArgumentException("Unknown methodId:" + methodId);
		}
		if (string.Equals(item.ItemType, "InvokeInfo", StringComparison.Ordinal))
		{
			return AddDeviceGroup(requestContext, item, path);
		}
		return CreateItemAddGroup(item, methodId);
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
		bool isAdministrator = ConfigurationApiBase.HasGeneralReadRights(requestContext);
		PrepareCache(requestContext, folderPath, itemFilter, childItemFilter, isAdministrator, out var childGroupItemTypes, out var includeMembers);
		ConfigurationItem configurationItem = new ConfigurationItem
		{
			Path = folderPath.Text
		};
		FillFolderChildrenRecursive(requestContext, configurationItem, folderPath, isAdministrator, childGroupItemTypes, includeMembers, childItemFilter);
		return configurationItem.Children;
	}

	private void PrepareCache(IGeminiRequestContext requestContext, ConfigurationItemPath folderPath, ItemFilter itemFilter, List<ItemFilter> childItemFilter, bool isAdministrator, out HashSet<string> childGroupItemTypes, out bool includeMembers)
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
				if (IsDeviceGroup(item.ItemType))
				{
					hashSet.Add(item.ItemType);
					childGroupItemTypes.Add(item.ItemType);
				}
				if (IsDevice(item.ItemType))
				{
					includeMembers = true;
				}
			}
		}
		_deviceGroupCache.Prepare(requestContext, isAdministrator, hashSet, includeMembers);
		_deviceGroupCache.LoadGroupsForHierarchy(requestContext, ConfigurationApiBase.RemoveFolderPartFromItemType(folderPath.ItemType));
	}

	private bool FillFolderChildrenRecursive(IGeminiRequestContext requestContext, ConfigurationItem groupFolder, ConfigurationItemPath folderPath, bool isAdministrator, HashSet<string> groupItemTypes, bool includeMembers, List<ItemFilter> childItemFilter)
	{
		string text = ConfigurationApiBase.RemoveFolderPartFromItemType(folderPath.ItemType);
		if (!IsDeviceGroup(text))
		{
			return false;
		}
		Guid parentGroupId = Guid.Empty;
		if (!string.IsNullOrEmpty(folderPath.Id))
		{
			parentGroupId = new Guid(folderPath.Id);
		}
		groupFolder.Children = BuildSubGroups(requestContext, _deviceGroupCache.QuerySubGroups(text, parentGroupId), text, groupFolder);
		groupFolder.ChildrenFilled = true;
		List<ConfigurationItem> list = new List<ConfigurationItem>();
		ConfigurationItem[] children = groupFolder.Children;
		foreach (ConfigurationItem configurationItem in children)
		{
			if (FillChildrenToGroupItem(requestContext, configurationItem, isAdministrator, groupItemTypes, includeMembers, childItemFilter))
			{
				list.Add(configurationItem);
			}
		}
		groupFolder.Children = list.ToArray();
		return list.Count != 0;
	}

	private bool FillChildrenToGroupItem(IGeminiRequestContext requestContext, ConfigurationItem groupItem, bool isAdministrator, HashSet<string> groupItemTypes, bool includeMembers, List<ItemFilter> childItemFilter)
	{
		ConfigurationItemPath configurationItemPath = ConfigurationItemPath.CreatePath(groupItem.Path);
		string itemType = configurationItemPath.ItemType;
		DeviceType type = ConvertItemTypeToDeviceType(itemType);
		groupItem.Children = ConstructDeviceGroupFolders(type, configurationItemPath, out var deviceGroupFolder, out var deviceFolder);
		groupItem.ChildrenFilled = true;
		Guid groupId = new Guid(ConfigurationItemPath.CreatePath(groupItem.Path).Id);
		bool flag = MatchFilterOnDisplayName(groupItem, childItemFilter);
		bool flag2 = groupItemTypes.Contains(itemType);
		if (includeMembers)
		{
			string itemType2 = GetItemTypeFromGroup(itemType);
			ConfigurationItem[] array = BuildDeviceItems(requestContext, _deviceGroupCache.QueryDevicesInGroup(itemType, groupId), itemType2, flag2);
			ItemFilter itemFilter = childItemFilter?.FirstOrDefault((ItemFilter f) => f.ItemType == itemType2);
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
				deviceFolder.Children = list.ToArray();
			}
			else
			{
				deviceFolder.Children = array.ToArray();
			}
			deviceFolder.ChildrenFilled = true;
		}
		if (flag2)
		{
			if (!(FillFolderChildrenRecursive(requestContext, deviceGroupFolder, configurationItemPath, isAdministrator, groupItemTypes, includeMembers, childItemFilter) || flag))
			{
				return deviceGroupFolder.Children.Length != 0;
			}
			return true;
		}
		return true;
	}

	private static string GetItemTypeFromGroup(string groupItemType)
	{
		if (groupItemType.EndsWith("Group", StringComparison.InvariantCulture))
		{
			return groupItemType.Substring(0, groupItemType.Length - "Group".Length);
		}
		return groupItemType;
	}

	private ConfigurationItem[] BuildDeviceItems(IGeminiRequestContext requestContext, IEnumerable<Guid> deviceIds, string itemType, bool fillHierarchy)
	{
		List<ConfigurationItem> list = new List<ConfigurationItem>();
		if (fillHierarchy)
		{
			_deviceGroupCache.LoadDevicesForGroup(requestContext, itemType, deviceIds);
		}
		else
		{
			_deviceGroupCache.LoadDevicesForHierarchy(requestContext, itemType);
		}
		foreach (Device item2 in _deviceGroupCache.LoadDevicesFromCache(itemType, deviceIds))
		{
			ConfigurationItem item = DeviceConfigurationApi.ConvertToConfigurationItem(ConfigurationItemPath.CreateFolderPath("Hardware", item2.HardwareId, itemType + "Folder"), item2);
			list.Add(item);
		}
		return list.ToArray();
	}

	private ConfigurationItem[] BuildSubGroups(IGeminiRequestContext requestContext, IEnumerable<Guid> groupIds, string groupItemType, ConfigurationItem parentFolder)
	{
		List<ConfigurationItem> list = new List<ConfigurationItem>();
		IEnumerable<DeviceGroup> enumerable = _deviceGroupCache.LoadDeviceGroupsFromCache(groupItemType, groupIds);
		ConfigurationItemPath parentPath = ConfigurationItemPath.CreatePath(parentFolder.Path);
		foreach (DeviceGroup item2 in enumerable)
		{
			ConfigurationItem item = ConvertToConfigurationItem(parentPath, item2);
			list.Add(item);
		}
		return list.ToArray();
	}

	private static bool MatchFilterOnDisplayName(ConfigurationItem item, List<ItemFilter> itemFilters)
	{
		if (itemFilters == null)
		{
			return true;
		}
		ItemFilter itemFilter = itemFilters.FirstOrDefault((ItemFilter f) => f.ItemType.Equals(item.ItemType, StringComparison.OrdinalIgnoreCase));
		if (itemFilter == null)
		{
			return true;
		}
		PropertyFilter propertyFilter = itemFilter.PropertyFilters?.FirstOrDefault((PropertyFilter pf) => pf.Key.Equals("Name", StringComparison.OrdinalIgnoreCase));
		if (propertyFilter == null)
		{
			return true;
		}
		return propertyFilter.Operator switch
		{
			Operator.Equals => item.DisplayName.Equals(propertyFilter.Value, StringComparison.OrdinalIgnoreCase), 
			Operator.Contains => item.DisplayName.Contains(propertyFilter.Value), 
			Operator.BeginsWith => item.DisplayName.StartsWith(propertyFilter.Value, StringComparison.OrdinalIgnoreCase), 
			Operator.GreaterThan => item.DisplayName.EndsWith(propertyFilter.Value, StringComparison.OrdinalIgnoreCase), 
			_ => false, 
		};
	}

	private static bool IsDeviceGroup(string itemType)
	{
		switch (itemType)
		{
		default:
			return itemType == "MetadataGroup";
		case "CameraGroup":
		case "MicrophoneGroup":
		case "SpeakerGroup":
		case "InputEventGroup":
		case "OutputGroup":
			return true;
		}
	}

	private static bool IsDevice(string itemType)
	{
		switch (itemType)
		{
		default:
			return itemType == "Metadata";
		case "Camera":
		case "Microphone":
		case "Speaker":
		case "InputEvent":
		case "Output":
			return true;
		}
	}

	private void CheckGroupExistsAndHasAccess(IGeminiRequestContext requestContext, ConfigurationItemPath path, DeviceType type)
	{
		try
		{
			if (ConfigurationApiBase.HasGeneralReadRights(requestContext))
			{
				if (DeviceGroup.QueryDeviceGroupById(requestContext, path.Id, type).DeviceType != type)
				{
					throw new PathNotFoundException(path.Text);
				}
			}
			else
			{
				string text = ConfigurationApiBase.RemoveFolderPartFromItemType(path.ItemType);
				_deviceGroupCache.Prepare(requestContext, isAdministrator: false, new HashSet<string> { text }, includeMembers: false);
				_deviceGroupCache.QuerySubGroups(text, new Guid(path.Id));
			}
		}
		catch (DeviceGroupNotFoundException)
		{
			throw new PathNotFoundException(path.Text);
		}
	}

	private static ConfigurationItem[] ConstructDeviceGroupFolders(DeviceType type, ConfigurationItemPath parentPath, out ConfigurationItem deviceGroupFolder, out ConfigurationItem deviceFolder)
	{
		string itemType = ConvertDeviceTypeToGroupFolderItemType(type);
		deviceGroupFolder = GroupConfigurationItemFactory.CreateFolderItem(itemType, parentPath);
		string itemType2 = ConvertDeviceTypeToFolderItemType(type);
		deviceFolder = GroupConfigurationItemFactory.CreateFolderItem(itemType2, parentPath);
		return new ConfigurationItem[2] { deviceGroupFolder, deviceFolder };
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
				if (string.Equals(path.ItemType, "CameraGroupFolder", StringComparison.Ordinal) && !string.Equals(path.ParentItemType, "CameraGroup", StringComparison.Ordinal))
				{
					throw new PathInvalidException("Invalid parent. Path: " + path.Text);
				}
				if (string.Equals(path.ItemType, "MicrophoneGroupFolder", StringComparison.Ordinal) && !string.Equals(path.ParentItemType, "MicrophoneGroup", StringComparison.Ordinal))
				{
					throw new PathInvalidException("Invalid parent. Path: " + path.Text);
				}
				if (string.Equals(path.ItemType, "SpeakerGroupFolder", StringComparison.Ordinal) && !string.Equals(path.ParentItemType, "SpeakerGroup", StringComparison.Ordinal))
				{
					throw new PathInvalidException("Invalid parent. Path: " + path.Text);
				}
				if (string.Equals(path.ItemType, "InputEventGroupFolder", StringComparison.Ordinal) && !string.Equals(path.ParentItemType, "InputEventGroup", StringComparison.Ordinal))
				{
					throw new PathInvalidException("Invalid parent. Path: " + path.Text);
				}
				if (string.Equals(path.ItemType, "OutputGroupFolder", StringComparison.Ordinal) && !string.Equals(path.ParentItemType, "OutputGroup", StringComparison.Ordinal))
				{
					throw new PathInvalidException("Invalid parent. Path: " + path.Text);
				}
				if (string.Equals(path.ItemType, "MetadataGroupFolder", StringComparison.Ordinal) && !string.Equals(path.ParentItemType, "MetadataGroup", StringComparison.Ordinal))
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

	private static ConfigurationItem AddDeviceGroup(IGeminiRequestContext requestContext, ConfigurationItem configurationItem, ConfigurationItemPath parentPath)
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
		DeviceTypeRelation deviceTypeRelation = DeviceTypeRelation.Relations.FirstOrDefault((DeviceTypeRelation r) => string.Equals(parentPath.ItemType, r.GroupFolderType, StringComparison.Ordinal));
		if (deviceTypeRelation == null)
		{
			throw new PathInvalidException("Invalid parent folder path: " + parentPath.Text);
		}
		if (!string.Equals(parentPath.ParentItemType, "System", StringComparison.Ordinal) && !string.Equals(parentPath.ParentItemType, deviceTypeRelation.GroupType, StringComparison.Ordinal))
		{
			throw new PathInvalidException("Invalid parent folder path: " + parentPath.Text);
		}
		string parentId = null;
		if (!string.Equals(parentPath.ParentItemType, "System", StringComparison.Ordinal))
		{
			parentId = parentPath.Id;
		}
		string id = Guid.NewGuid().ToString();
		DeviceType deviceType = deviceTypeRelation.DeviceType;
		((IEnumerable<Property>)configurationItem.Properties).TryGetPropertyValue("GroupName", out string value);
		((IEnumerable<Property>)configurationItem.Properties).TryGetPropertyValue("GroupDescription", out string value2);
		DeviceGroup deviceGroup;
		try
		{
			deviceGroup = DeviceGroup.CreateDeviceGroup(requestContext, id, parentId, deviceType, value, value2);
		}
		catch (ArgumentException)
		{
			throw;
		}
		catch (DeviceGroupCreationParentNotFoundException)
		{
			throw new PathNotFoundException(parentPath.Text);
		}
		catch (DeviceGroupCreationNameAlreadyExistException ex3)
		{
			throw new PropertyInvalidException(ex3.Message, "name");
		}
		catch (DeviceGroupCreationReadOnlyParentException ex4)
		{
			throw new ArgumentException(ex4.Message, "parentPath");
		}
		ConfigurationItem configurationItem2 = ConvertToConfigurationItem(parentPath, deviceGroup);
		return MethodTypesHelper.CreateInvokeResult(configurationItem, configurationItem2.Path, "Result", "Success", true);
	}

	private static ConfigurationItem RemoveDeviceGroup(IGeminiRequestContext requestContext, ConfigurationItem configurationItem, ConfigurationItemPath parentPath)
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
		DeviceTypeRelation deviceTypeRelation = DeviceTypeRelation.Relations.FirstOrDefault((DeviceTypeRelation r) => string.Equals(parentPath.ItemType, r.GroupFolderType, StringComparison.Ordinal));
		if (deviceTypeRelation == null)
		{
			throw new PathInvalidException("Invalid parent folder path: " + parentPath.Text);
		}
		if (string.Equals(parentPath.ParentItemType, deviceTypeRelation.GroupType, StringComparison.Ordinal))
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
			throw new PropertyInvalidException("Invalid device path. " + value, "ItemSelection", inner);
		}
		if (configurationItemPath.IsFolder || !string.Equals(deviceTypeRelation.GroupType, configurationItemPath.ItemType, StringComparison.Ordinal) || !configurationItemPath.Id.IsGuid())
		{
			throw new PropertyInvalidException("Invalid device path. " + value, "ItemSelection");
		}
		string id = configurationItemPath.Id;
		try
		{
			if (result)
			{
				DeleteMembersAndGroups(requestContext, id, deviceTypeRelation.DeviceType);
			}
			else
			{
				DeviceGroup.DeleteDeviceGroup(requestContext, id);
			}
		}
		catch (DeviceGroupDeleteGroupNotFoundException)
		{
			throw new PathNotFoundException(parentPath.Text);
		}
		catch (DeviceGroupNotFoundException)
		{
			throw new PathNotFoundException(parentPath.Text);
		}
		catch (DeviceGroupDeleteGroupReadOnlyException ex3)
		{
			throw new PropertyInvalidException(ex3.Message, "ItemSelection");
		}
		catch (DeviceGroupDeleteGroupNotEmptyException ex4)
		{
			throw new PropertyInvalidException(ex4.Message, "ItemSelection");
		}
		return MethodTypesHelper.CreateInvokeResultWithoutItemPropertiesAndChildren(configurationItem);
	}

	private static void DeleteMembersAndGroups(IGeminiRequestContext requestContext, string groupId, DeviceType type)
	{
		string[] ids = DeviceGroup.QueryDeviceGroupIds(requestContext, groupId, type);
		DeviceGroup[] array = DeviceGroup.QueryDeviceGroups(requestContext, ids);
		foreach (DeviceGroup deviceGroup in array)
		{
			DeleteMembersAndGroups(requestContext, deviceGroup.Id, type);
		}
		DeviceTypeSearch deviceTypeSearch = DeviceConfigurationApi.ConvertItemTypeToDeviceTypeSearch(type.ToString());
		string[] array2 = Device.QueryDeviceIdsByDeviceGroup(requestContext, groupId, deviceTypeSearch);
		if (array2.Length != 0)
		{
			DeviceGroup.RemoveDeviceGroupMembers(requestContext, groupId, array2);
		}
		DeviceGroup.DeleteDeviceGroup(requestContext, groupId);
	}

	public static ConfigurationItem AddDeviceGroupMember(IGeminiRequestContext requestContext, ConfigurationItem configurationItem, ConfigurationItemPath parentPath)
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
		DeviceTypeRelation deviceTypeRelation = DeviceTypeRelation.Relations.FirstOrDefault((DeviceTypeRelation r) => string.Equals(parentPath.ItemType, r.FolderType, StringComparison.Ordinal));
		if (deviceTypeRelation == null)
		{
			throw new PathInvalidException("Invalid parent folder path: " + parentPath.Text);
		}
		if (!string.Equals(parentPath.ParentItemType, deviceTypeRelation.GroupType, StringComparison.Ordinal))
		{
			throw new PathInvalidException("Invalid parent folder path: " + parentPath.Text);
		}
		if (!parentPath.Id.IsGuid())
		{
			throw new PathInvalidException("Invalid parent folder path: " + parentPath.Text);
		}
		string id = parentPath.Id;
		string value = null;
		if (!((IEnumerable<Property>)configurationItem.Properties).TryGetPropertyValue("ItemSelection", out value))
		{
			throw new PropertyInvalidException("No selection.", "ItemSelection");
		}
		if (value == null)
		{
			throw ExceptionFactory.CreateArgumentNullException("ItemSelection");
		}
		ConfigurationItemPath configurationItemPath;
		try
		{
			configurationItemPath = ConfigurationItemPath.CreatePath(value);
		}
		catch (ArgumentException inner)
		{
			throw new PropertyInvalidException("Invalid device path. " + value, "ItemSelection", inner);
		}
		if (configurationItemPath.IsFolder || !string.Equals(deviceTypeRelation.ItemType, configurationItemPath.ItemType, StringComparison.Ordinal) || !configurationItemPath.Id.IsGuid())
		{
			throw new PropertyInvalidException("Invalid device path. " + value, "ItemSelection");
		}
		string id2 = configurationItemPath.Id;
		try
		{
			DeviceGroup.AddDeviceGroupMembers(requestContext, id, new string[1] { id2 });
		}
		catch (DeviceGroupAddMembersGroupNotFoundException)
		{
			throw new PathNotFoundException(parentPath.Text);
		}
		catch (DeviceGroupAddMembersToReadOnlyGroupException ex2)
		{
			throw new ArgumentException(ex2.Message, "parentPath");
		}
		catch (DeviceGroupAddMembersDeviceNotFoundException)
		{
			throw new PathNotFoundException(value);
		}
		catch (DeviceGroupAddMembersTypeMismatchException ex4)
		{
			throw new PropertyInvalidException(ex4.Message, "ItemSelection");
		}
		catch (DeviceGroupAddMembersAlreadyExistException ex5)
		{
			throw new PropertyInvalidException(ex5.Message, "ItemSelection");
		}
		return MethodTypesHelper.CreateInvokeResult(configurationItem, null, "Result", "Success", true);
	}

	public static ConfigurationItem RemoveDeviceGroupMember(IGeminiRequestContext requestContext, ConfigurationItem configurationItem, ConfigurationItemPath parentPath)
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
		DeviceTypeRelation deviceTypeRelation = DeviceTypeRelation.Relations.FirstOrDefault((DeviceTypeRelation r) => string.Equals(parentPath.ItemType, r.FolderType, StringComparison.Ordinal));
		if (deviceTypeRelation == null)
		{
			throw new PathInvalidException("Invalid parent folder path: " + parentPath.Text);
		}
		if (!string.Equals(parentPath.ParentItemType, deviceTypeRelation.GroupType, StringComparison.Ordinal))
		{
			throw new PathInvalidException("Invalid parent folder path: " + parentPath.Text);
		}
		if (!parentPath.Id.IsGuid())
		{
			throw new PathInvalidException("Invalid parent folder path: " + parentPath.Text);
		}
		string id = parentPath.Id;
		string value = null;
		if (!((IEnumerable<Property>)configurationItem.Properties).TryGetPropertyValue("ItemSelection", out value))
		{
			throw new PropertyInvalidException("No selection.", "ItemSelection");
		}
		if (value == null)
		{
			throw ExceptionFactory.CreateArgumentNullException("ItemSelection");
		}
		ConfigurationItemPath configurationItemPath;
		try
		{
			configurationItemPath = ConfigurationItemPath.CreatePath(value);
		}
		catch (ArgumentException inner)
		{
			throw new PropertyInvalidException("Invalid device path. " + value, "ItemSelection", inner);
		}
		if (configurationItemPath.IsFolder || !string.Equals(deviceTypeRelation.ItemType, configurationItemPath.ItemType, StringComparison.Ordinal) || !configurationItemPath.Id.IsGuid())
		{
			throw new PropertyInvalidException("Invalid device path. " + value, "ItemSelection");
		}
		string id2 = configurationItemPath.Id;
		try
		{
			DeviceGroup.RemoveDeviceGroupMembers(requestContext, id, new string[1] { id2 });
		}
		catch (DeviceGroupRemoveMemberGroupNotFoundException)
		{
			throw new PathNotFoundException(parentPath.Text);
		}
		catch (DeviceGroupRemoveMemberReadOnlyGroupException ex2)
		{
			throw new PropertyInvalidException(ex2.Message, "ParentPath");
		}
		catch (DeviceGroupRemoveMemberDeviceNotFoundException ex3)
		{
			throw new PropertyInvalidException(ex3.Message, "ItemSelection");
		}
		return MethodTypesHelper.CreateInvokeResultWithoutItemPropertiesAndChildren(configurationItem);
	}

	public static ConfigurationItem ConvertToConfigurationItem(ConfigurationItemPath parentPath, DeviceGroup deviceGroup)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = ConvertDeviceTypeToItemType(deviceGroup.DeviceType);
		configurationItem.UIHint = "DirectionHorizontal";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = deviceGroup.Name;
		configurationItem.ParentPath = parentPath.Text;
		configurationItem.Path = ConfigurationItemPath.CreateItemPath(configurationItem.ItemType, deviceGroup.Id).Text;
		List<Property> list = new List<Property>();
		list.Add(GeneralPropertyDefinitions.GetIdPropertyGuid(deviceGroup.Id));
		list.Add(GeneralPropertyDefinitions.GetNameProperty(deviceGroup.Name));
		list.Add(GeneralPropertyDefinitions.GetDescriptionProperty(deviceGroup.Description));
		list.Add(GeneralPropertyDefinitions.GetLastModifiedProperty(deviceGroup.LastModified));
		list.Add(DeviceGroupPropertyDefinition.BuiltInProperty(deviceGroup.BuiltIn));
		configurationItem.MethodIds = new string[0];
		configurationItem.Properties = list.ToArray();
		return configurationItem;
	}

	public static DeviceType ConvertItemTypeToDeviceType(string itemType)
	{
		switch (itemType)
		{
		case "CameraGroupFolder":
		case "CameraGroup":
			return DeviceType.Camera;
		case "MicrophoneGroupFolder":
		case "MicrophoneGroup":
			return DeviceType.Microphone;
		case "SpeakerGroupFolder":
		case "SpeakerGroup":
			return DeviceType.Speaker;
		case "InputEventGroupFolder":
		case "InputEventGroup":
			return DeviceType.Input;
		case "OutputGroupFolder":
		case "OutputGroup":
			return DeviceType.Output;
		case "MetadataGroupFolder":
		case "MetadataGroup":
			return DeviceType.Metadata;
		default:
			return DeviceType.Unknown;
		}
	}

	public static string ConvertDeviceTypeToGroupFolderItemType(DeviceType deviceType)
	{
		string text = null;
		return deviceType switch
		{
			DeviceType.Camera => "CameraGroupFolder", 
			DeviceType.Input => "InputEventGroupFolder", 
			DeviceType.Microphone => "MicrophoneGroupFolder", 
			DeviceType.Output => "OutputGroupFolder", 
			DeviceType.Speaker => "SpeakerGroupFolder", 
			DeviceType.Metadata => "MetadataGroupFolder", 
			_ => null, 
		};
	}

	private static string ConvertDeviceTypeToFolderItemType(DeviceType deviceType)
	{
		string text = null;
		return deviceType switch
		{
			DeviceType.Camera => "CameraFolder", 
			DeviceType.Input => "InputEventFolder", 
			DeviceType.Microphone => "MicrophoneFolder", 
			DeviceType.Output => "OutputFolder", 
			DeviceType.Speaker => "SpeakerFolder", 
			DeviceType.Metadata => "MetadataFolder", 
			_ => null, 
		};
	}

	public static string ConvertDeviceTypeToItemType(DeviceType deviceType)
	{
		string text = null;
		return deviceType switch
		{
			DeviceType.Camera => "CameraGroup", 
			DeviceType.Input => "InputEventGroup", 
			DeviceType.Microphone => "MicrophoneGroup", 
			DeviceType.Output => "OutputGroup", 
			DeviceType.Speaker => "SpeakerGroup", 
			DeviceType.Metadata => "MetadataGroup", 
			_ => null, 
		};
	}

	private static ConfigurationItem CreateItemRemoveGroup(ConfigurationItem item, ConfigurationItem[] members, string methodId)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionVertical";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.WizardRemoveGroup;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		List<Property> list = new List<Property>();
		list.Add(DeviceGroupPropertyDefinition.RemoveMembersProperty(removeMembers: false));
		configurationItem.MethodIds = new string[1] { methodId };
		list.Add(GetMembersAsEnumsProperty(members));
		configurationItem.Properties = list.ToArray();
		return configurationItem;
	}

	private static ConfigurationItem CreateItemAddGroup(ConfigurationItem item, string methodId)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionVertical";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.WizardAddGroup;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		List<Property> list = new List<Property>();
		configurationItem.MethodIds = new string[1] { methodId };
		list.Add(GetSelectGroupName());
		list.Add(GetSelectGroupDescription());
		configurationItem.Properties = list.ToArray();
		return configurationItem;
	}

	public static ConfigurationItem CreateWizardItemAddMember(ConfigurationItem item, string methodId)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionVertical";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.WizardAddDeviceGroupMember;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		List<Property> list = new List<Property>();
		configurationItem.MethodIds = new string[1] { methodId };
		string text = GeneralPropertyDefinitions.ConvertFromItemTypeToChildItemType(item.ItemType);
		string displayName = string.Format(CultureInfo.InvariantCulture, ConfigurationApiResources.PropertySelectNewMember, ConfigurationApiResources.ResourceManager.GetString("ItemType" + text));
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
		configurationItem.DisplayName = ConfigurationApiResources.WizardRemoveDeviceGroupMember;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		List<Property> list = new List<Property>();
		configurationItem.MethodIds = new string[1] { methodId };
		list.Add(GetMembersAsEnumsProperty(members));
		configurationItem.Properties = list.ToArray();
		return configurationItem;
	}

	private static Property GetSelectGroupName()
	{
		return new Property("GroupName", "")
		{
			ValueType = "String",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyGroupName,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyGroupName",
			UIImportance = UIImportance.Default,
			SortKey = 1
		};
	}

	private static Property GetSelectGroupDescription()
	{
		return new Property("GroupDescription", "")
		{
			ValueType = "String",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyGroupDescription,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyGroupDescription",
			UIImportance = UIImportance.Default,
			SortKey = 1
		};
	}

	private static Property GetMembersAsEnumsProperty(ConfigurationItem[] members)
	{
		Property property = new Property("ItemSelection", "")
		{
			ValueType = "Enum",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertySelectMemberToRemove,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "",
			UIImportance = UIImportance.Default,
			SortKey = 1
		};
		List<ValueTypeInfo> list = new List<ValueTypeInfo>();
		foreach (ConfigurationItem configurationItem in members)
		{
			list.Add(new ValueTypeInfo
			{
				Name = configurationItem.DisplayName,
				TranslationId = "",
				Value = configurationItem.Path
			});
		}
		property.ValueTypeInfos = list.ToArray();
		return property;
	}
}
