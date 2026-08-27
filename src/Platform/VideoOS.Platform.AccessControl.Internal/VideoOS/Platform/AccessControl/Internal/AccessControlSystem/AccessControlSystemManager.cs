using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Platform.AccessControl.Elements;
using VideoOS.Platform.AccessControl.Internal.AccessControlElements;
using VideoOS.Platform.AccessControl.Internal.AccessControlSystem.Configuration;
using VideoOS.Platform.AccessControl.Internal.Data.Rule;
using VideoOS.Platform.AccessControl.Internal.InternalSerializationClasses;
using VideoOS.Platform.AccessControl.Internal.Providers.Strategies;
using VideoOS.Platform.AccessControl.Internal.Util;
using VideoOS.Platform.AccessControl.Plugin;
using VideoOS.Platform.Data;

namespace VideoOS.Platform.AccessControl.Internal.AccessControlSystem;

public class AccessControlSystemManager
{
	public class UpdateResult
	{
		public bool EnabledUpdated { get; set; }

		public bool NameUpdated { get; set; }

		public bool DescriptionUpdated { get; set; }

		public bool PersonalizedLoginEnabledUpdated { get; set; }

		public List<string> ChangedProperties { get; set; }

		public bool AccessControlSystemConfigurationUpdated { get; set; }

		public bool CategoryMappingConfigurationUpdated { get; set; }

		public bool SurveillanceItemMappingConfigurationUpdated { get; set; }

		public bool OperationableInstanceConfigurationUpdated { get; set; }

		public bool RuleConfigurationUpdated { get; set; }

		public IEnumerable<Tuple<string, bool>> ChangedOperationableInstances { get; set; }

		public IEnumerable<Tuple<string, bool>> ChangedEventTypes { get; set; }

		public bool HasProperUpdates
		{
			get
			{
				if (!EnabledUpdated && !NameUpdated && !DescriptionUpdated && !PersonalizedLoginEnabledUpdated && !ChangedProperties.Any() && !AccessControlSystemConfigurationUpdated && !CategoryMappingConfigurationUpdated && !SurveillanceItemMappingConfigurationUpdated && !OperationableInstanceConfigurationUpdated)
				{
					return RuleConfigurationUpdated;
				}
				return true;
			}
		}
	}

	private AccessControlSystemConfiguration _accessControlSystemConfiguration;

	private AccessControlCategoryMappingConfiguration _accessControlCategoryMappingConfiguration;

	private AccessControlSurveillanceItemMappingConfiguration _accessControlSurveillanceItemMappingConfiguration;

	private AccessControlOperationableInstanceConfiguration _accessControlOperationableInstanceConfiguration;

	private AccessControlRuleConfiguration _accessControlRuleConfiguration;

	private IGuidProviderStrategy GuidProviderStrategy { get; } = new Sha1ProviderStrategy();

	public AccessControlSystemConfiguration AccessControlSystemConfiguration => _accessControlSystemConfiguration;

	public AccessControlCategoryMappingConfiguration AccessControlCategoryMappingConfiguration => _accessControlCategoryMappingConfiguration;

	public AccessControlSurveillanceItemMappingConfiguration AccessControlSurveillanceItemMappingConfiguration => _accessControlSurveillanceItemMappingConfiguration;

	public AccessControlOperationableInstanceConfiguration AccessControlOperationableInstanceConfiguration => _accessControlOperationableInstanceConfiguration;

	public AccessControlRuleConfiguration AccessControlRuleConfiguration => _accessControlRuleConfiguration;

	public Guid Id { get; private set; }

	public string Name { get; set; }

	public string Description { get; set; }

	public Guid PluginId { get; private set; }

	public bool PersonalizedLoginEnabled { get; set; }

	public IEnumerable<Property> Properties { get; private set; }

	public bool Enabled { get; set; }

	public AccessControlLicenseManager AccessControlLicenseManager { get; public set; }

	public AccessControlSystemManager(Guid id, string name, Guid pluginId, IEnumerable<Property> properties)
	{
		Id = id;
		Name = name;
		Description = "";
		PluginId = pluginId;
		Properties = properties;
		_accessControlSystemConfiguration = new AccessControlSystemConfiguration(this);
		_accessControlCategoryMappingConfiguration = new AccessControlCategoryMappingConfiguration(this);
		_accessControlSurveillanceItemMappingConfiguration = new AccessControlSurveillanceItemMappingConfiguration();
		_accessControlOperationableInstanceConfiguration = new AccessControlOperationableInstanceConfiguration();
		_accessControlRuleConfiguration = new AccessControlRuleConfiguration();
		Enabled = false;
	}

	public AccessControlSystemManager(Guid id, string serializedConfiguration)
		: this(id, string.Empty, Guid.Empty, new List<Property>())
	{
		Update(serializedConfiguration);
	}

	public AccessControlSystemManager(Guid id, string serializedConfiguration, IGuidProviderStrategy guidProviderStrategy)
		: this(id, string.Empty, Guid.Empty, new List<Property>())
	{
		GuidProviderStrategy = guidProviderStrategy;
		Update(serializedConfiguration);
	}

	public AccessControlSystemManager(Guid id, string serializedConfiguration, AccessControlLicenseManager licenseManager)
		: this(id, serializedConfiguration)
	{
		AccessControlLicenseManager = licenseManager;
	}

	public UpdateResult Update(string serializedConfiguration)
	{
		UpdateResult updateResult = new UpdateResult();
		SystemManagerSerializeData systemManagerSerializeData = SystemManagerSerializeData.Deserialize(serializedConfiguration);
		if (systemManagerSerializeData == null)
		{
			throw new ArgumentException("Configuration could not be deserialized.", "serializedConfiguration");
		}
		PluginId = systemManagerSerializeData.PluginId;
		if (Name != systemManagerSerializeData.Name)
		{
			Name = systemManagerSerializeData.Name;
			updateResult.NameUpdated = true;
		}
		if (Description != systemManagerSerializeData.Description)
		{
			Description = systemManagerSerializeData.Description;
			updateResult.DescriptionUpdated = true;
		}
		if (Enabled != systemManagerSerializeData.Enabled)
		{
			Enabled = systemManagerSerializeData.Enabled;
			updateResult.EnabledUpdated = true;
		}
		if (PersonalizedLoginEnabled != systemManagerSerializeData.PersonalizedLoginEnabled)
		{
			PersonalizedLoginEnabled = systemManagerSerializeData.PersonalizedLoginEnabled;
			updateResult.PersonalizedLoginEnabledUpdated = true;
		}
		updateResult.ChangedProperties = GetChangedProperties(Properties, systemManagerSerializeData.Properties).ToList();
		List<Property> list = new List<Property>();
		foreach (SystemManagerSerializeData.Property property in systemManagerSerializeData.Properties)
		{
			list.Add(new Property
			{
				Key = property.Key,
				Value = property.Value
			});
		}
		Properties = list.ToList();
		string obj = ((_accessControlSystemConfiguration != null) ? ACConfigurationSerializeData.Serialize(_accessControlSystemConfiguration.SerializeData()) : null);
		string text = ACConfigurationSerializeData.Serialize(systemManagerSerializeData.AccessControlSystemConfiguration);
		if (obj != text)
		{
			_accessControlSystemConfiguration = new AccessControlSystemConfiguration(this, text, GuidProviderStrategy);
			updateResult.AccessControlSystemConfigurationUpdated = true;
		}
		string text2 = CategoryMappingConfigurationSerializeData.Serialize(_accessControlCategoryMappingConfiguration.SerializeData());
		string text3 = CategoryMappingConfigurationSerializeData.Serialize(systemManagerSerializeData.AccessControlCategoryMappingConfiguration);
		if (updateResult.AccessControlSystemConfigurationUpdated || text2 != text3)
		{
			updateResult.ChangedEventTypes = GetChangedEventTypes(_accessControlCategoryMappingConfiguration.SerializeData(), systemManagerSerializeData.AccessControlCategoryMappingConfiguration);
			_accessControlCategoryMappingConfiguration = new AccessControlCategoryMappingConfiguration(this, text3);
			updateResult.CategoryMappingConfigurationUpdated = true;
		}
		string text4 = SurveillanceItemMappingSerializeData.Serialize(_accessControlSurveillanceItemMappingConfiguration.SerializeData());
		string text5 = SurveillanceItemMappingSerializeData.Serialize(systemManagerSerializeData.AccessControlSurveillanceItemMappingConfiguration);
		if (text4 != text5)
		{
			_accessControlSurveillanceItemMappingConfiguration = new AccessControlSurveillanceItemMappingConfiguration(text5);
			updateResult.SurveillanceItemMappingConfigurationUpdated = true;
		}
		updateResult.ChangedOperationableInstances = GetChangedOperationableInstance(_accessControlOperationableInstanceConfiguration.SerializeData().OperationableInstanceConfiguration, systemManagerSerializeData.AccessControlOperationableInstanceConfiguration?.OperationableInstanceConfiguration);
		if (updateResult.ChangedOperationableInstances.Any())
		{
			string serializedString = OperationableInstanceConfigurationSerializeData.Serialize(systemManagerSerializeData.AccessControlOperationableInstanceConfiguration);
			_accessControlOperationableInstanceConfiguration = new AccessControlOperationableInstanceConfiguration(serializedString);
			updateResult.OperationableInstanceConfigurationUpdated = true;
		}
		string text6 = RuleConfigurationSerializeData.Serialize(_accessControlRuleConfiguration.SerializeData());
		string text7 = RuleConfigurationSerializeData.Serialize(systemManagerSerializeData.AccessControlRuleConfiguration);
		if (text6 != text7)
		{
			_accessControlRuleConfiguration = new AccessControlRuleConfiguration(text7);
			updateResult.RuleConfigurationUpdated = true;
		}
		return updateResult;
	}

	private IEnumerable<Tuple<string, bool>> GetChangedEventTypes(CategoryMappingConfigurationSerializeData oldCategoryMappingConfigurationSerializeData, CategoryMappingConfigurationSerializeData newCategoryMappingConfigurationSerializeData)
	{
		foreach (CategoryMappingConfigurationSerializeData.CategoryMapping oldItem in oldCategoryMappingConfigurationSerializeData.CategoryMappings)
		{
			CategoryMappingConfigurationSerializeData.CategoryMapping categoryMapping = newCategoryMappingConfigurationSerializeData?.CategoryMappings?.FirstOrDefault((CategoryMappingConfigurationSerializeData.CategoryMapping x) => x.AccessControlTypeId == oldItem.AccessControlTypeId);
			if (categoryMapping != null)
			{
				bool num = oldItem.CategoryIds?.Intersect(CategoryHandler.CategoryEventTypesIgnore.Select((BuiltInCategory ceti) => ceti.Id)).Any() ?? false;
				bool flag = categoryMapping.CategoryIds?.Intersect(CategoryHandler.CategoryEventTypesIgnore.Select((BuiltInCategory ceti) => ceti.Id)).Any() ?? false;
				if (num ^ flag)
				{
					yield return new Tuple<string, bool>(AccessControlSystemConfiguration.GetExternalId(categoryMapping.AccessControlTypeId), !flag);
				}
			}
		}
	}

	private IEnumerable<Tuple<string, bool>> GetChangedOperationableInstance(List<OperationableInstanceConfigurationSerializeData.OperationableInstanceConfigurationData> operationableInstanceConfiguration1, List<OperationableInstanceConfigurationSerializeData.OperationableInstanceConfigurationData> operationableInstanceConfiguration2)
	{
		if (operationableInstanceConfiguration1 == null || operationableInstanceConfiguration2 == null)
		{
			return new List<Tuple<string, bool>>();
		}
		return from x in operationableInstanceConfiguration2.Except(operationableInstanceConfiguration1, new OperationableInstanceIdEnabledComparer())
			select new Tuple<string, bool>(AccessControlSystemConfiguration.GetExternalId(x.AccessControlOperationableInstanceId), x.Enabled);
	}

	public void ComputeAccessControlUnitEnabledState(IEnumerable<AccessControlUnit> accessControlUnits, int availableLicenses)
	{
		AccessControlSystemConfiguration.ComputeAccessControlUnitEnabledState(accessControlUnits, availableLicenses);
	}

	public string SerializeAccessControlSystem()
	{
		SystemManagerSerializeData systemManagerSerializeData = new SystemManagerSerializeData();
		systemManagerSerializeData.Name = Name;
		systemManagerSerializeData.Description = Description;
		systemManagerSerializeData.PluginId = PluginId;
		systemManagerSerializeData.Enabled = Enabled;
		systemManagerSerializeData.PersonalizedLoginEnabled = PersonalizedLoginEnabled;
		systemManagerSerializeData.Properties = new List<SystemManagerSerializeData.Property>();
		foreach (Property property in Properties)
		{
			systemManagerSerializeData.Properties.Add(new SystemManagerSerializeData.Property
			{
				Key = property.Key,
				Value = property.Value
			});
		}
		systemManagerSerializeData.AccessControlSystemConfiguration = _accessControlSystemConfiguration.SerializeData();
		systemManagerSerializeData.AccessControlCategoryMappingConfiguration = _accessControlCategoryMappingConfiguration.SerializeData();
		systemManagerSerializeData.AccessControlSurveillanceItemMappingConfiguration = _accessControlSurveillanceItemMappingConfiguration.SerializeData();
		systemManagerSerializeData.AccessControlOperationableInstanceConfiguration = _accessControlOperationableInstanceConfiguration.SerializeData();
		systemManagerSerializeData.AccessControlRuleConfiguration = _accessControlRuleConfiguration.SerializeData();
		return SystemManagerSerializeData.Serialize(systemManagerSerializeData);
	}

	public void SetProperties(IEnumerable<Property> properties)
	{
		Properties = properties;
	}

	public void SetAcConfiguration(ACConfiguration acConfiguration)
	{
		if (acConfiguration == null)
		{
			throw new ArgumentNullException("acConfiguration");
		}
		_accessControlSystemConfiguration.SetAcConfiguration(acConfiguration);
		string serializedString = CategoryMappingConfigurationSerializeData.Serialize(_accessControlCategoryMappingConfiguration.SerializeData());
		_accessControlCategoryMappingConfiguration = new AccessControlCategoryMappingConfiguration(this, serializedString);
	}

	public void UpdateACElementsWithEnabledState()
	{
		foreach (KeyValuePair<Guid, bool> item in AccessControlOperationableInstanceConfiguration?.GetOperationableInstancesEnabled())
		{
			if (GetACElementWithAccessControlElementId(item.Key) is ACOperationableInstance aCOperationableInstance)
			{
				aCOperationableInstance.IsEnabled = item.Value;
			}
		}
		foreach (KeyValuePair<Guid, List<Category>> accessControlTypeIdToCustomCategory in _accessControlCategoryMappingConfiguration.GetAccessControlTypeIdToCustomCategories())
		{
			if (!(GetACElementWithAccessControlElementId(accessControlTypeIdToCustomCategory.Key) is ACEventType aCEventType))
			{
				continue;
			}
			List<Category> value = accessControlTypeIdToCustomCategory.Value;
			aCEventType.IsEnabled = value == null || !value.Any((Category x) => CategoryHandler.CategoryEventTypesIgnore.Any((BuiltInCategory ceti) => ceti.Id == x.Id));
		}
	}

	public ACConfiguration GetAcConfiguration()
	{
		return _accessControlSystemConfiguration.GetAcConfiguration();
	}

	public AccessControlServer GetAccessControlServerElement()
	{
		return _accessControlSystemConfiguration.GetAccessControlElements(typeof(AccessControlServer)).FirstOrDefault() as AccessControlServer;
	}

	private ACElement GetACElementWithAccessControlElementId(Guid id)
	{
		return _accessControlSystemConfiguration.LookupACElement(_accessControlSystemConfiguration.GetExternalId(id));
	}

	public AccessControlElement GetAccessControlElement(Guid id)
	{
		return _accessControlSystemConfiguration.GetAccessControlElement(id);
	}

	public AccessControlElement GetAccessControlElement(string externalId)
	{
		return _accessControlSystemConfiguration.GetAccessControlElement(externalId);
	}

	public IEnumerable<AccessControlElement> GetAccessControlElements()
	{
		return _accessControlSystemConfiguration.GetAccessControlElements();
	}

	public IEnumerable<AccessControlElement> GetAccessControlElements(Type elementType)
	{
		return _accessControlSystemConfiguration.GetAccessControlElements(elementType);
	}

	public IEnumerable<AccessControlElement> GetAccessControlElements(IEnumerable<Category> categories)
	{
		return GetAccessControlElements(categories, null);
	}

	public IEnumerable<AccessControlElement> GetAccessControlElements(IEnumerable<Category> categories, Type elementType)
	{
		return GetAccessControlElements(categories, elementType, intersectingElementsOnly: false);
	}

	public IEnumerable<AccessControlElement> GetAccessControlElements(IEnumerable<Category> categories, Type elementType, bool intersectingElementsOnly)
	{
		List<AccessControlElement> list = new List<AccessControlElement>();
		if (elementType != null)
		{
			list.AddRange(GetAccessControlElements(elementType));
		}
		else
		{
			list.AddRange(GetAccessControlElements());
		}
		List<AccessControlElement> list2 = new List<AccessControlElement>();
		foreach (AccessControlElement item in list)
		{
			AccessControlType accessControlType = null;
			if (item is AccessControlInstance)
			{
				accessControlType = ((AccessControlInstance)item).AccessControlType;
			}
			else if (item is AccessControlType)
			{
				accessControlType = (AccessControlType)item;
			}
			if (accessControlType == null)
			{
				continue;
			}
			bool flag = false;
			bool flag2 = true;
			foreach (Category category in categories)
			{
				flag = flag || accessControlType.Categories.Contains(category);
				flag2 = flag2 && accessControlType.Categories.Contains(category);
			}
			if (intersectingElementsOnly && flag2)
			{
				list2.Add(item);
			}
			else if (!intersectingElementsOnly && flag)
			{
				list2.Add(item);
			}
		}
		return list2;
	}

	public string GetExternalFromInternalId(Guid internalId)
	{
		return _accessControlSystemConfiguration.GetExternalId(internalId);
	}

	public IEnumerable<AccessControlRule> GetAccessControlRules()
	{
		return _accessControlRuleConfiguration.GetAccessControlRules();
	}

	public void SetAccessControlRules(IEnumerable<AccessControlRule> accessControlRules)
	{
		_accessControlRuleConfiguration.SetAccessControlRules(accessControlRules);
	}

	private static IEnumerable<string> GetChangedProperties(IEnumerable<Property> propertiesA, IEnumerable<SystemManagerSerializeData.Property> propertiesB)
	{
		List<string> list = new List<string>();
		foreach (Property item in propertiesA)
		{
			bool flag = false;
			foreach (SystemManagerSerializeData.Property item2 in propertiesB)
			{
				if (item.Key.Equals(item2.Key))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				list.Add(item.Key);
			}
		}
		foreach (SystemManagerSerializeData.Property item3 in propertiesB)
		{
			bool flag2 = false;
			foreach (Property item4 in propertiesA)
			{
				if (item4.Key == item3.Key)
				{
					flag2 = true;
					break;
				}
			}
			if (!flag2)
			{
				list.Add(item3.Key);
			}
		}
		foreach (SystemManagerSerializeData.Property item5 in propertiesB)
		{
			bool flag3 = false;
			foreach (Property item6 in propertiesA)
			{
				if (item6.Key == item5.Key)
				{
					if (item6.Value != item5.Value)
					{
						flag3 = true;
					}
					break;
				}
			}
			if (flag3)
			{
				list.Add(item5.Key);
			}
		}
		return list;
	}
}
