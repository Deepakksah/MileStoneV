using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Platform.AccessControl.Internal.AccessControlElements;
using VideoOS.Platform.AccessControl.Internal.InternalSerializationClasses;

namespace VideoOS.Platform.AccessControl.Internal.AccessControlSystem.Configuration;

public class AccessControlCategoryMappingConfiguration
{
	private AccessControlSystemManager _accessControlSystemManager;

	private Dictionary<Guid, List<Category>> _accessControlTypeIdToCustomCategoriesDictionary = new Dictionary<Guid, List<Category>>();

	private Dictionary<Guid, List<Category>> _accessControlTypeIdToDefaultCategoriesDictionary = new Dictionary<Guid, List<Category>>();

	public AccessControlCategoryMappingConfiguration(AccessControlSystemManager accessControlSystemManager)
	{
		_accessControlSystemManager = accessControlSystemManager;
		BuildDefaultCategoriesDictionary();
	}

	public AccessControlCategoryMappingConfiguration(AccessControlSystemManager accessControlSystemManager, string serializedString)
		: this(accessControlSystemManager)
	{
		_accessControlTypeIdToCustomCategoriesDictionary.Clear();
		CategoryMappingConfigurationSerializeData categoryMappingConfigurationSerializeData = CategoryMappingConfigurationSerializeData.Deserialize(serializedString);
		if (categoryMappingConfigurationSerializeData == null)
		{
			return;
		}
		foreach (CategoryMappingConfigurationSerializeData.CategoryMapping categoryMapping in categoryMappingConfigurationSerializeData.CategoryMappings)
		{
			if (!(_accessControlSystemManager.GetAccessControlElement(categoryMapping.AccessControlTypeId) is AccessControlType accessControlType))
			{
				continue;
			}
			List<Category> list = new List<Category>();
			foreach (Category possibleCategory in accessControlType.PossibleCategories)
			{
				if (categoryMapping.CategoryIds.Contains(possibleCategory.Id))
				{
					list.Add(possibleCategory);
				}
			}
			if (list != null)
			{
				_accessControlTypeIdToCustomCategoriesDictionary.Add(categoryMapping.AccessControlTypeId, list);
			}
		}
	}

	public CategoryMappingConfigurationSerializeData SerializeData()
	{
		lock (this)
		{
			CategoryMappingConfigurationSerializeData categoryMappingConfigurationSerializeData = new CategoryMappingConfigurationSerializeData();
			foreach (KeyValuePair<Guid, List<Category>> item in _accessControlTypeIdToCustomCategoriesDictionary)
			{
				List<Guid> categoryIds = item.Value.Select((Category category) => category.Id).ToList();
				categoryMappingConfigurationSerializeData.CategoryMappings.Add(new CategoryMappingConfigurationSerializeData.CategoryMapping
				{
					AccessControlTypeId = item.Key,
					CategoryIds = categoryIds
				});
			}
			return categoryMappingConfigurationSerializeData;
		}
	}

	public bool HasCustomCategories(AccessControlType accessControlType)
	{
		lock (this)
		{
			return _accessControlTypeIdToCustomCategoriesDictionary.ContainsKey(accessControlType.Id);
		}
	}

	public void ClearCustomCategories(AccessControlType accessControlType)
	{
		lock (this)
		{
			if (_accessControlTypeIdToCustomCategoriesDictionary.ContainsKey(accessControlType.Id))
			{
				_accessControlTypeIdToCustomCategoriesDictionary.Remove(accessControlType.Id);
			}
		}
	}

	public IEnumerable<Category> GetResolvedCategories(AccessControlType accessControlType)
	{
		lock (this)
		{
			if (HasCustomCategories(accessControlType))
			{
				return GetCustomCategories(accessControlType);
			}
			return GetDefaultCategories(accessControlType);
		}
	}

	public IEnumerable<Category> GetDefaultCategories(AccessControlType accessControlType)
	{
		lock (this)
		{
			return new List<Category>(_accessControlTypeIdToDefaultCategoriesDictionary[accessControlType.Id]);
		}
	}

	public IEnumerable<Category> GetCustomCategories(AccessControlType accessControlType)
	{
		lock (this)
		{
			if (HasCustomCategories(accessControlType))
			{
				return new List<Category>(_accessControlTypeIdToCustomCategoriesDictionary[accessControlType.Id]);
			}
			return null;
		}
	}

	public void SetCustomCategories(AccessControlType accessControlType, IEnumerable<Category> categories)
	{
		lock (this)
		{
			if (categories == null)
			{
				ClearCustomCategories(accessControlType);
				return;
			}
			if (!HasCustomCategories(accessControlType))
			{
				_accessControlTypeIdToCustomCategoriesDictionary.Add(accessControlType.Id, null);
			}
			_accessControlTypeIdToCustomCategoriesDictionary[accessControlType.Id] = categories.ToList();
		}
	}

	public Dictionary<Guid, List<Category>> GetAccessControlTypeIdToCustomCategories()
	{
		return _accessControlTypeIdToCustomCategoriesDictionary;
	}

	private void BuildDefaultCategoriesDictionary()
	{
		_accessControlTypeIdToDefaultCategoriesDictionary.Clear();
		lock (this)
		{
			foreach (AccessControlType item in _accessControlSystemManager.GetAccessControlElements(typeof(AccessControlType)).Cast<AccessControlType>())
			{
				_accessControlTypeIdToDefaultCategoriesDictionary.Add(item.Id, _accessControlSystemManager.AccessControlSystemConfiguration.GetTypeCategories(item).ToList());
			}
		}
	}
}
