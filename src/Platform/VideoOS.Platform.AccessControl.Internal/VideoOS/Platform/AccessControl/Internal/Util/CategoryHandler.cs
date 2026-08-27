using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Platform.AccessControl.Internal.AccessControlElements;
using VideoOS.Platform.AccessControl.Internal.AccessControlSystem;
using VideoOS.Platform.AccessControl.Internal.Data;
using VideoOS.Platform.AccessControl.Internal.InternalSerializationClasses;
using VideoOS.Platform.AccessControl.TypeCategories;

namespace VideoOS.Platform.AccessControl.Internal.Util;

public static class CategoryHandler
{
	public static BuiltInCategory[] CategoryEventTypesIgnore;

	public static BuiltInCategory[] CategoryEventTypesError;

	public static BuiltInCategory[] CategoryEventTypesWarning;

	public static BuiltInCategory[] CategoryEventTypesAlarm;

	public static BuiltInCategory[] CategoryEventTypesCredentialHolderAccessGranted;

	public static BuiltInCategory[] CategoryEventTypesCredentialHolderAccessDenied;

	public static BuiltInCategory[] CategoryEventTypesAccessRequest;

	public static BuiltInCategory[] CategoryStateTypesError;

	public static BuiltInCategory[] CategoryStateTypesDisabled;

	public static BuiltInCategory[] CategoryStateTypesWarning;

	public static BuiltInCategory[] CategoryStateTypesServerStatusConnectionConnected;

	public static BuiltInCategory[] CategoryStateTypesServerStatusConnectionNotConnected;

	public static BuiltInCategory[] CategoryStateTypesDoorStateOpen;

	public static BuiltInCategory[] CategoryStateTypesDoorStateClosed;

	public static BuiltInCategory[] CategoryStateTypesDoorStateLocked;

	public static BuiltInCategory[] CategoryStateTypesDoorStateUnlocked;

	public static BuiltInCategory[] CategoryCommandTypesAccessRequest;

	public static BuiltInCategory[] CategoryCommandTypesGrantAccess;

	public static BuiltInCategory[] CategoryUnitTypesDoor;

	public static BuiltInCategory[] CategoryUnitTypesAccessPoint;

	public static BuiltInCategory CategoryEventTypeIgnore;

	public static BuiltInCategory CategoryEventTypeError;

	public static BuiltInCategory CategoryEventTypeWarning;

	public static BuiltInCategory CategoryEventTypeAlarm;

	public static BuiltInCategory CategoryEventTypeCredentialHolderAccessGranted;

	public static BuiltInCategory CategoryEventTypeCredentialHolderAccessDenied;

	public static BuiltInCategory CategoryEventTypeAccessRequest;

	public static BuiltInCategory CategoryStateTypeError;

	public static BuiltInCategory CategoryStateTypeDisabled;

	public static BuiltInCategory CategoryStateTypeWarning;

	public static BuiltInCategory CategoryStateTypeServerStatusConnectionConnected;

	public static BuiltInCategory CategoryStateTypeServerStatusConnectionNotConnected;

	public static BuiltInCategory CategoryStateTypeDoorStateOpen;

	public static BuiltInCategory CategoryStateTypeDoorStateClosed;

	public static BuiltInCategory CategoryStateTypeDoorStateLocked;

	public static BuiltInCategory CategoryStateTypeDoorStateUnlocked;

	public static BuiltInCategory CategoryCommandTypeAccessRequest;

	public static BuiltInCategory CategoryCommandTypeGrantAccess;

	public static BuiltInCategory CategoryUnitTypeDoor;

	public static BuiltInCategory CategoryUnitTypeAccessPoint;

	private static List<BuiltInCategory> _builtInCategories;

	private static Dictionary<Guid, IEnumerable<PluginCategory>> _accessControlPluginCategoryDictionary;

	private static List<UserDefinedCategory> _userDefinedCategories;

	public static event EventHandler UserDefinedCategoriesChanged;

	static CategoryHandler()
	{
		CategoryEventTypesIgnore = BuiltInCategory.BuiltInCategories(ACBuiltInEventTypeCategories.Ignore, typeof(AccessControlEventType));
		CategoryEventTypesError = BuiltInCategory.BuiltInCategories(ACBuiltInEventTypeCategories.Error, typeof(AccessControlEventType));
		CategoryEventTypesWarning = BuiltInCategory.BuiltInCategories(ACBuiltInEventTypeCategories.Warning, typeof(AccessControlEventType));
		CategoryEventTypesAlarm = BuiltInCategory.BuiltInCategories(ACBuiltInEventTypeCategories.Alarm, typeof(AccessControlEventType));
		CategoryEventTypesCredentialHolderAccessGranted = BuiltInCategory.BuiltInCategories(ACBuiltInEventTypeCategories.CredentialHolderAccessGranted, typeof(AccessControlEventType));
		CategoryEventTypesCredentialHolderAccessDenied = BuiltInCategory.BuiltInCategories(ACBuiltInEventTypeCategories.CredentialHolderAccessDenied, typeof(AccessControlEventType));
		CategoryEventTypesAccessRequest = BuiltInCategory.BuiltInCategories(ACBuiltInEventTypeCategories.AccessRequest, typeof(AccessControlEventType));
		CategoryStateTypesError = BuiltInCategory.BuiltInCategories(ACBuiltInStateTypeCategories.Error, typeof(AccessControlStateType));
		CategoryStateTypesDisabled = BuiltInCategory.BuiltInCategories(ACBuiltInStateTypeCategories.Disabled, typeof(AccessControlStateType));
		CategoryStateTypesWarning = BuiltInCategory.BuiltInCategories(ACBuiltInStateTypeCategories.Warning, typeof(AccessControlStateType));
		CategoryStateTypesServerStatusConnectionConnected = BuiltInCategory.BuiltInCategories(ACBuiltInStateTypeCategories.ServerStatusConnectionConnected, typeof(AccessControlStateType));
		CategoryStateTypesServerStatusConnectionNotConnected = BuiltInCategory.BuiltInCategories(ACBuiltInStateTypeCategories.ServerStatusConnectionNotConnected, typeof(AccessControlStateType));
		CategoryStateTypesDoorStateOpen = BuiltInCategory.BuiltInCategories(ACBuiltInStateTypeCategories.DoorStateOpen, typeof(AccessControlStateType));
		CategoryStateTypesDoorStateClosed = BuiltInCategory.BuiltInCategories(ACBuiltInStateTypeCategories.DoorStateClosed, typeof(AccessControlStateType));
		CategoryStateTypesDoorStateLocked = BuiltInCategory.BuiltInCategories(ACBuiltInStateTypeCategories.DoorStateLocked, typeof(AccessControlStateType));
		CategoryStateTypesDoorStateUnlocked = BuiltInCategory.BuiltInCategories(ACBuiltInStateTypeCategories.DoorStateUnlocked, typeof(AccessControlStateType));
		CategoryCommandTypesAccessRequest = BuiltInCategory.BuiltInCategories(ACBuiltInCommandTypeCategories.AccessRequest, typeof(AccessControlCommandType));
		CategoryCommandTypesGrantAccess = BuiltInCategory.BuiltInCategories(ACBuiltInCommandTypeCategories.GrantAccess, typeof(AccessControlCommandType));
		CategoryUnitTypesDoor = BuiltInCategory.BuiltInCategories(ACBuiltInUnitTypeCategories.Door, typeof(AccessControlUnitType));
		CategoryUnitTypesAccessPoint = BuiltInCategory.BuiltInCategories(ACBuiltInUnitTypeCategories.AccessPoint, typeof(AccessControlUnitType));
		CategoryEventTypeIgnore = CategoryEventTypesIgnore.First();
		CategoryEventTypeError = CategoryEventTypesError.First();
		CategoryEventTypeWarning = CategoryEventTypesWarning.First();
		CategoryEventTypeAlarm = CategoryEventTypesAlarm.First();
		CategoryEventTypeCredentialHolderAccessGranted = CategoryEventTypesCredentialHolderAccessGranted.First();
		CategoryEventTypeCredentialHolderAccessDenied = CategoryEventTypesCredentialHolderAccessDenied.First();
		CategoryEventTypeAccessRequest = CategoryEventTypesAccessRequest.First();
		CategoryStateTypeError = CategoryStateTypesError.First();
		CategoryStateTypeDisabled = CategoryStateTypesDisabled.First();
		CategoryStateTypeWarning = CategoryStateTypesWarning.First();
		CategoryStateTypeServerStatusConnectionConnected = CategoryStateTypesServerStatusConnectionConnected.First();
		CategoryStateTypeServerStatusConnectionNotConnected = CategoryStateTypesServerStatusConnectionNotConnected.First();
		CategoryStateTypeDoorStateOpen = CategoryStateTypesDoorStateOpen.First();
		CategoryStateTypeDoorStateClosed = CategoryStateTypesDoorStateClosed.First();
		CategoryStateTypeDoorStateLocked = CategoryStateTypesDoorStateLocked.First();
		CategoryStateTypeDoorStateUnlocked = CategoryStateTypesDoorStateUnlocked.First();
		CategoryCommandTypeAccessRequest = CategoryCommandTypesAccessRequest.First();
		CategoryCommandTypeGrantAccess = CategoryCommandTypesGrantAccess.First();
		CategoryUnitTypeDoor = CategoryUnitTypesDoor.First();
		CategoryUnitTypeAccessPoint = CategoryUnitTypesAccessPoint.First();
		CategoryHandler.UserDefinedCategoriesChanged = delegate
		{
		};
		_builtInCategories = new List<BuiltInCategory>();
		_accessControlPluginCategoryDictionary = new Dictionary<Guid, IEnumerable<PluginCategory>>();
		_userDefinedCategories = new List<UserDefinedCategory>();
		_builtInCategories.AddRange(CategoryEventTypesIgnore);
		_builtInCategories.AddRange(CategoryEventTypesError);
		_builtInCategories.AddRange(CategoryEventTypesWarning);
		_builtInCategories.AddRange(CategoryEventTypesAlarm);
		_builtInCategories.AddRange(CategoryEventTypesCredentialHolderAccessGranted);
		_builtInCategories.AddRange(CategoryEventTypesCredentialHolderAccessDenied);
		_builtInCategories.AddRange(CategoryEventTypesAccessRequest);
		_builtInCategories.AddRange(CategoryStateTypesError);
		_builtInCategories.AddRange(CategoryStateTypesDisabled);
		_builtInCategories.AddRange(CategoryStateTypesWarning);
		_builtInCategories.AddRange(CategoryStateTypesServerStatusConnectionConnected);
		_builtInCategories.AddRange(CategoryStateTypesServerStatusConnectionNotConnected);
		_builtInCategories.AddRange(CategoryStateTypesDoorStateOpen);
		_builtInCategories.AddRange(CategoryStateTypesDoorStateClosed);
		_builtInCategories.AddRange(CategoryStateTypesDoorStateLocked);
		_builtInCategories.AddRange(CategoryStateTypesDoorStateUnlocked);
		_builtInCategories.AddRange(CategoryCommandTypesAccessRequest);
		_builtInCategories.AddRange(CategoryCommandTypesGrantAccess);
		_builtInCategories.AddRange(CategoryUnitTypesDoor);
		_builtInCategories.AddRange(CategoryUnitTypesAccessPoint);
	}

	public static IEnumerable<BuiltInCategory> GetBuiltInCategories()
	{
		return _builtInCategories;
	}

	public static void SetAccessControlPluginCategories(Guid accessControlPluginId, IEnumerable<PluginCategory> plugInCategories)
	{
		lock (_accessControlPluginCategoryDictionary)
		{
			_accessControlPluginCategoryDictionary[accessControlPluginId] = plugInCategories;
		}
	}

	public static IEnumerable<PluginCategory> GetAccessControlPluginCategories(Guid accessControlPluginId)
	{
		lock (_accessControlPluginCategoryDictionary)
		{
			if (_accessControlPluginCategoryDictionary.ContainsKey(accessControlPluginId))
			{
				return _accessControlPluginCategoryDictionary[accessControlPluginId];
			}
			return new List<PluginCategory>();
		}
	}

	public static void Clear()
	{
		_accessControlPluginCategoryDictionary.Clear();
		_userDefinedCategories.Clear();
	}

	public static void ClearAccessControlPluginCategories(Guid accessControlPluginId)
	{
		lock (_accessControlPluginCategoryDictionary)
		{
			if (_accessControlPluginCategoryDictionary.ContainsKey(accessControlPluginId))
			{
				_accessControlPluginCategoryDictionary.Remove(accessControlPluginId);
			}
		}
	}

	public static void SetUserDefinedCategories(string serializedString)
	{
		List<UserDefinedCategory> list = new List<UserDefinedCategory>();
		try
		{
			foreach (UserDefinedCategoriesSerializeData.UserDefinedCategory userDefinedCategory in UserDefinedCategoriesSerializeData.Deserialize(serializedString).UserDefinedCategories)
			{
				list.Add(new UserDefinedCategory(userDefinedCategory.Id, userDefinedCategory.Name, AccessControlTypeToStringConverter.TypeFromString(userDefinedCategory.AccessControlTypeString)));
			}
		}
		catch (Exception)
		{
		}
		SetUserDefinedCategories(list);
	}

	public static void SetUserDefinedCategories(IEnumerable<UserDefinedCategory> userDefinedCategories)
	{
		List<UserDefinedCategory> list = new List<UserDefinedCategory>(userDefinedCategories);
		lock (_userDefinedCategories)
		{
			if (_userDefinedCategories.Except(list).Any() || list.Except(_userDefinedCategories).Any())
			{
				_userDefinedCategories = list;
				CategoryHandler.UserDefinedCategoriesChanged(null, new EventArgs());
			}
		}
	}

	public static IEnumerable<UserDefinedCategory> GetUserDefinedCategories()
	{
		lock (_userDefinedCategories)
		{
			return new List<UserDefinedCategory>(_userDefinedCategories);
		}
	}

	public static Category GetCategory(Guid categoryId)
	{
		using (IEnumerator<Category> enumerator = (from category in GetCategories()
			where category.Id == categoryId
			select category).GetEnumerator())
		{
			if (enumerator.MoveNext())
			{
				return enumerator.Current;
			}
		}
		return null;
	}

	public static IEnumerable<Category> GetCategories()
	{
		return GetCategories(Guid.Empty, null);
	}

	public static IEnumerable<Category> GetCategories(Guid accessControlPluginId)
	{
		return GetCategories(accessControlPluginId, null);
	}

	public static IEnumerable<Category> GetCategories(Type accessControlType)
	{
		return GetCategories(Guid.Empty, accessControlType);
	}

	public static IEnumerable<Category> GetCategories(Guid accessControlPluginId, Type accessControlType)
	{
		List<Category> list = new List<Category>();
		foreach (BuiltInCategory builtInCategory in _builtInCategories)
		{
			if (accessControlType == null || accessControlType.IsAssignableFrom(builtInCategory.AccessControlType))
			{
				list.Add(builtInCategory);
			}
		}
		lock (_accessControlPluginCategoryDictionary)
		{
			foreach (KeyValuePair<Guid, IEnumerable<PluginCategory>> item in _accessControlPluginCategoryDictionary)
			{
				if (!(accessControlPluginId == Guid.Empty) && !(accessControlPluginId == item.Key))
				{
					continue;
				}
				foreach (PluginCategory item2 in item.Value)
				{
					if (accessControlType == null || accessControlType.IsAssignableFrom(item2.AccessControlType))
					{
						list.Add(item2);
					}
				}
			}
		}
		lock (_userDefinedCategories)
		{
			foreach (UserDefinedCategory userDefinedCategory in _userDefinedCategories)
			{
				if (accessControlType == null || accessControlType.IsAssignableFrom(userDefinedCategory.AccessControlType))
				{
					list.Add(userDefinedCategory);
				}
			}
			return list;
		}
	}

	public static string SerializeUserDefinedCategories()
	{
		lock (_userDefinedCategories)
		{
			UserDefinedCategoriesSerializeData userDefinedCategoriesSerializeData = new UserDefinedCategoriesSerializeData();
			foreach (UserDefinedCategory userDefinedCategory in _userDefinedCategories)
			{
				userDefinedCategoriesSerializeData.UserDefinedCategories.Add(new UserDefinedCategoriesSerializeData.UserDefinedCategory
				{
					Id = userDefinedCategory.Id,
					Name = userDefinedCategory.Name,
					AccessControlTypeString = AccessControlTypeToStringConverter.TypeToString(userDefinedCategory.AccessControlType)
				});
			}
			return UserDefinedCategoriesSerializeData.Serialize(userDefinedCategoriesSerializeData);
		}
	}

	public static List<Category> ResolveStateCategories(AccessControlState accessControlState, AccessControlSystemManager accessControlSystemManager)
	{
		List<Category> list = new List<Category>();
		Guid[] accessControlStateTypeIds = accessControlState.AccessControlStateTypeIds;
		foreach (Guid id in accessControlStateTypeIds)
		{
			if (!(accessControlSystemManager.GetAccessControlElement(id) is AccessControlStateType accessControlStateType))
			{
				continue;
			}
			foreach (Category category in accessControlStateType.Categories)
			{
				if (!list.Contains(category))
				{
					list.Add(category);
				}
			}
		}
		return list;
	}
}
