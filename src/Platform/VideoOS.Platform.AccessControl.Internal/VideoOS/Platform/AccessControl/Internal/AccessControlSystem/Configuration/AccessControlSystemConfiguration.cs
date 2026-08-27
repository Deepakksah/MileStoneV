using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using VideoOS.Platform.AccessControl.Elements;
using VideoOS.Platform.AccessControl.Internal.AccessControlElements;
using VideoOS.Platform.AccessControl.Internal.InternalSerializationClasses;
using VideoOS.Platform.AccessControl.Internal.Providers.Strategies;
using VideoOS.Platform.AccessControl.Internal.Util;
using VideoOS.Platform.AccessControl.Plugin;
using VideoOS.Platform.Data;
using VideoOS.Platform.Resources;

namespace VideoOS.Platform.AccessControl.Internal.AccessControlSystem.Configuration;

public class AccessControlSystemConfiguration
{
	private DateTime _configurationTimestamp = DateTime.MinValue;

	private Dictionary<string, Guid> _acElementIdToAccessControlElementIdDictionary = new Dictionary<string, Guid>();

	private Dictionary<Guid, string> _accessControlElementIdToACElementIdDictionary = new Dictionary<Guid, string>();

	private Dictionary<string, ACElement> _acElementIdToACElementDictionary = new Dictionary<string, ACElement>();

	private Dictionary<Guid, AccessControlElement> _accessControlElementIdToAccessControlElementDictionary = new Dictionary<Guid, AccessControlElement>();

	private Dictionary<Guid, Item> _accessControlElementIdToIntegrationItemDictionary = new Dictionary<Guid, Item>();

	private Dictionary<string, IList<AccessControlUnit>> _parentIdToChildren = new Dictionary<string, IList<AccessControlUnit>>();

	private AccessControlSystemManager _accessControlSystemManager;

	private IGuidProviderStrategy GuidProviderStrategy { get; } = new Sha1ProviderStrategy();

	public AccessControlSystemConfiguration(AccessControlSystemManager accessControlSystemManager)
	{
		_accessControlSystemManager = accessControlSystemManager;
	}

	public AccessControlSystemConfiguration(AccessControlSystemManager accessControlSystemManager, string serializedXml)
		: this(accessControlSystemManager, serializedXml, new Sha1ProviderStrategy())
	{
	}

	public AccessControlSystemConfiguration(AccessControlSystemManager accessControlSystemManager, string serializedXml, IGuidProviderStrategy guidProviderStrategy)
		: this(accessControlSystemManager)
	{
		if (guidProviderStrategy == null)
		{
			throw new ArgumentNullException("guidProviderStrategy");
		}
		ACConfiguration aCConfiguration = ACConfigurationHelper.CreateACConfiguration(serializedXml);
		GuidProviderStrategy = guidProviderStrategy;
		if (aCConfiguration != null)
		{
			SetAcConfiguration(aCConfiguration);
		}
	}

	public void SetAcConfiguration(ACConfiguration acConfiguration)
	{
		if (acConfiguration == null)
		{
			throw new ArgumentNullException("acConfiguration");
		}
		lock (this)
		{
			_acElementIdToACElementDictionary.Clear();
			_acElementIdToAccessControlElementIdDictionary.Clear();
			_accessControlElementIdToACElementIdDictionary.Clear();
			_accessControlElementIdToAccessControlElementDictionary.Clear();
			_accessControlElementIdToIntegrationItemDictionary.Clear();
			_parentIdToChildren.Clear();
			_configurationTimestamp = acConfiguration.Timestamp;
			ACElement[] aCElements = acConfiguration.ACElements;
			foreach (ACElement acElement in aCElements)
			{
				AddACElement(acElement);
			}
			BuildIntegrationItems();
		}
	}

	public ACConfiguration GetAcConfiguration()
	{
		lock (this)
		{
			if (!IsEmpty())
			{
				try
				{
					return ACConfiguration.CreateACConfiguration(_configurationTimestamp, _acElementIdToACElementDictionary.Values);
				}
				catch (ACConfigurationException)
				{
				}
			}
			return null;
		}
	}

	public bool IsEmpty()
	{
		return !_acElementIdToAccessControlElementIdDictionary.Any();
	}

	public ACConfigurationSerializeData SerializeData()
	{
		lock (this)
		{
			if (!IsEmpty())
			{
				return ACConfigurationHelper.SerializeData(GetAcConfiguration());
			}
			return null;
		}
	}

	public AccessControlElement GetAccessControlElement(Guid id)
	{
		lock (this)
		{
			return LookupAccessControlElement(id);
		}
	}

	public AccessControlElement GetAccessControlElement(string externalId)
	{
		lock (this)
		{
			return LookupAccessControlElement(externalId);
		}
	}

	public IEnumerable<AccessControlElement> GetAccessControlElements()
	{
		lock (this)
		{
			return new List<AccessControlElement>(_accessControlElementIdToAccessControlElementDictionary.Values.ToArray());
		}
	}

	public IEnumerable<AccessControlElement> GetAccessControlElements(Type itemType)
	{
		lock (this)
		{
			List<AccessControlElement> list = new List<AccessControlElement>();
			foreach (AccessControlElement value in _accessControlElementIdToAccessControlElementDictionary.Values)
			{
				if (itemType == value.GetType() || value.GetType().IsSubclassOf(itemType))
				{
					list.Add(value);
				}
			}
			return list;
		}
	}

	public string GetExternalId(AccessControlElement accessControlElement)
	{
		lock (this)
		{
			string value;
			return _accessControlElementIdToACElementIdDictionary.TryGetValue(accessControlElement.Id, out value) ? value : string.Empty;
		}
	}

	public string GetExternalId(Guid accessControlElementId)
	{
		lock (this)
		{
			string value;
			return _accessControlElementIdToACElementIdDictionary.TryGetValue(accessControlElementId, out value) ? value : string.Empty;
		}
	}

	public string GetName(AccessControlElement accessControlElement)
	{
		lock (this)
		{
			ACElement aCElement = LookupACElement(accessControlElement.Id);
			if (aCElement != null)
			{
				return aCElement.Name;
			}
			return string.Empty;
		}
	}

	public Guid GetIconKey(AccessControlElement accessControlElement)
	{
		lock (this)
		{
			ACElement aCElement = LookupACElement(accessControlElement.Id);
			if (aCElement != null)
			{
				return GetIconKey(aCElement);
			}
			return IconHandler.GetUnspecifiedIconKey();
		}
	}

	public Icon GetIcon(AccessControlElement accessControlElement)
	{
		lock (this)
		{
			ACElement aCElement = LookupACElement(accessControlElement.Id);
			if (aCElement != null)
			{
				return GetIcon(aCElement);
			}
			return IconHandler.GetUnspecifiedIcon();
		}
	}

	public Bitmap GetIconBitmap(AccessControlElement accessControlElement)
	{
		lock (this)
		{
			ACElement acElement = LookupACElement(accessControlElement.Id);
			return GetIconBitmap(acElement);
		}
	}

	public IEnumerable<Property> GetProperties(AccessControlElement accessControlElement)
	{
		lock (this)
		{
			ACElement aCElement = LookupACElement(accessControlElement.Id);
			if (aCElement != null && aCElement.Properties != null)
			{
				return PropertyConverter.ToProperties(aCElement.Properties);
			}
			return new Property[0];
		}
	}

	public AccessControlType GetAccessControlType(AccessControlInstance accessControlInstance)
	{
		lock (this)
		{
			if (LookupACElement(accessControlInstance.Id) is ACInstance aCInstance)
			{
				return LookupAccessControlElement(aCInstance.InstanceTypeId) as AccessControlType;
			}
			return null;
		}
	}

	public IEnumerable<AccessControlCommandType> GetSupportedAccessControlCommandTypes(AccessControlOperationableInstance accessControlOperationableInstance)
	{
		lock (this)
		{
			List<AccessControlCommandType> list = new List<AccessControlCommandType>();
			if (LookupACElement(accessControlOperationableInstance.Id) is ACOperationableInstance aCOperationableInstance)
			{
				list = GetAccessControlCommandTypes((AccessControlOperationableType)accessControlOperationableInstance.AccessControlType).ToList();
				foreach (string unsupportedCommandTypeId in aCOperationableInstance.UnsupportedCommandTypeIds)
				{
					foreach (AccessControlCommandType item in list)
					{
						if (item.ExternalId == unsupportedCommandTypeId)
						{
							list.Remove(item);
							break;
						}
					}
				}
			}
			return list;
		}
	}

	public IEnumerable<AccessControlCommandType> GetAccessControlCommandTypes(AccessControlOperationableType accessControlOperationableType)
	{
		lock (this)
		{
			List<AccessControlCommandType> list = new List<AccessControlCommandType>();
			if (LookupACElement(accessControlOperationableType.Id) is ACOperationableType { CommandTypeIds: not null } aCOperationableType)
			{
				foreach (string commandTypeId in aCOperationableType.CommandTypeIds)
				{
					if (LookupAccessControlElement(commandTypeId) is AccessControlCommandType item)
					{
						list.Add(item);
					}
				}
			}
			return list;
		}
	}

	public IEnumerable<AccessControlEventType> GetAccessControlEventTypes(AccessControlOperationableType accessControlOperationableType)
	{
		lock (this)
		{
			List<AccessControlEventType> list = new List<AccessControlEventType>();
			if (LookupACElement(accessControlOperationableType.Id) is ACOperationableType { EventTypeIds: not null } aCOperationableType)
			{
				foreach (string eventTypeId in aCOperationableType.EventTypeIds)
				{
					if (LookupAccessControlElement(eventTypeId) is AccessControlEventType item)
					{
						list.Add(item);
					}
				}
			}
			return list;
		}
	}

	public void ComputeAccessControlUnitEnabledState(IEnumerable<AccessControlUnit> accessControlUnits, int availableLicenses)
	{
		int num = availableLicenses;
		foreach (AccessControlUnit accessControlUnit in accessControlUnits)
		{
			if (LookupACElement(accessControlUnit.Id) is ACOperationableInstance aCOperationableInstance)
			{
				if (accessControlUnit.Enabled && !aCOperationableInstance.IsEnabled)
				{
					num++;
				}
				else if (!accessControlUnit.Enabled && aCOperationableInstance.IsEnabled)
				{
					num--;
				}
				accessControlUnit.Enabled = aCOperationableInstance.IsEnabled;
			}
		}
		if (num >= 0)
		{
			return;
		}
		foreach (AccessControlUnit item in accessControlUnits.Where((AccessControlUnit x) => x.Enabled).Skip(Math.Max(0, accessControlUnits.Count() - Math.Abs(num))))
		{
			item.Enabled = false;
		}
	}

	public IEnumerable<AccessControlStateType> GetAccessControlStateTypes(AccessControlOperationableType accessControlOperationableType)
	{
		lock (this)
		{
			List<AccessControlStateType> list = new List<AccessControlStateType>();
			if (LookupACElement(accessControlOperationableType.Id) is ACOperationableType { StateTypeIds: not null } aCOperationableType)
			{
				foreach (string stateTypeId in aCOperationableType.StateTypeIds)
				{
					if (LookupAccessControlElement(stateTypeId) is AccessControlStateType item)
					{
						list.Add(item);
					}
				}
			}
			return list;
		}
	}

	public IEnumerable<Category> GetTypeCategories(AccessControlType accessControlType)
	{
		lock (this)
		{
			List<Category> list = new List<Category>();
			if (LookupACElement(accessControlType.Id) is ACType { Categories: not null } aCType)
			{
				foreach (Category category in CategoryHandler.GetCategories(accessControlType.AccessControlSystemManager.PluginId, accessControlType.GetType()))
				{
					if (category is BuiltInCategory)
					{
						if (aCType.Categories.Contains(((BuiltInCategory)category).BuiltInId))
						{
							list.Add(category);
						}
					}
					else if (category is PluginCategory && aCType.Categories.Contains(((PluginCategory)category).ExternalId))
					{
						list.Add(category);
					}
				}
			}
			return list;
		}
	}

	public AccessControlUnit GetParentAccessControlUnit(AccessControlUnit accessControlUnit)
	{
		lock (this)
		{
			if (LookupACElement(accessControlUnit.Id) is ACUnit aCUnit && !string.IsNullOrWhiteSpace(aCUnit.ParentUnitId))
			{
				return LookupAccessControlElement(aCUnit.ParentUnitId) as AccessControlUnit;
			}
			return null;
		}
	}

	public IEnumerable<AccessControlUnit> GetChildAccessControlUnits(AccessControlUnit accessControlUnit)
	{
		lock (this)
		{
			if (!(LookupACElement(accessControlUnit.Id) is ACUnit aCUnit))
			{
				return Enumerable.Empty<AccessControlUnit>();
			}
			if (!_parentIdToChildren.TryGetValue(aCUnit.Id, out var value))
			{
				return Enumerable.Empty<AccessControlUnit>();
			}
			return value;
		}
	}

	public PositioningInformation GetAccessControlUnitPosition(AccessControlUnit accessControlUnit)
	{
		lock (this)
		{
			if (!(LookupACElement(accessControlUnit.Id) is ACUnit aCUnit))
			{
				return null;
			}
			double latitude = 0.0;
			double longitude = 0.0;
			if (!string.IsNullOrEmpty(aCUnit.GisPoint))
			{
				string[] array = aCUnit.GisPoint.Split(',');
				latitude = Convert.ToDouble(array[0].Trim(), CultureInfo.InvariantCulture);
				longitude = Convert.ToDouble(array[1].Trim(), CultureInfo.InvariantCulture);
			}
			return new PositioningInformation(latitude, longitude, 0.0, 0.0, 0.0);
		}
	}

	public void SetAccessControlUnitPosition(AccessControlUnit accessControlUnit, PositioningInformation positioningInformation)
	{
		lock (this)
		{
			if (LookupACElement(accessControlUnit.Id) is ACUnit aCUnit)
			{
				aCUnit.SetPosition(positioningInformation.Latitude, positioningInformation.Longitude);
			}
		}
	}

	public int GetOrder(AccessControlCommandType accessControlCommandType)
	{
		lock (this)
		{
			if (!(LookupACElement(accessControlCommandType.Id) is ACCommandType { Order: var order }))
			{
				return 0;
			}
			return order;
		}
	}

	public Item GetIntegrationItem(AccessControlOperationableInstance accessControlOperationableInstance)
	{
		lock (this)
		{
			Item value;
			return _accessControlElementIdToIntegrationItemDictionary.TryGetValue(accessControlOperationableInstance.Id, out value) ? value : null;
		}
	}

	public ACElement LookupACElement(string acElementId)
	{
		_acElementIdToACElementDictionary.TryGetValue(acElementId, out var value);
		return value;
	}

	private AccessControlElement LookupAccessControlElement(ACElement acElement)
	{
		if (!_acElementIdToAccessControlElementIdDictionary.TryGetValue(acElement.Id, out var value))
		{
			return null;
		}
		return _accessControlElementIdToAccessControlElementDictionary[value];
	}

	private AccessControlElement LookupAccessControlElement(Guid itemId)
	{
		_accessControlElementIdToAccessControlElementDictionary.TryGetValue(itemId, out var value);
		return value;
	}

	private ACElement LookupACElement(Guid accessControlElementId)
	{
		lock (this)
		{
			string value;
			return _accessControlElementIdToACElementIdDictionary.TryGetValue(accessControlElementId, out value) ? _acElementIdToACElementDictionary[value] : null;
		}
	}

	private AccessControlElement LookupAccessControlElement(string elementId)
	{
		lock (this)
		{
			Guid value;
			return _acElementIdToAccessControlElementIdDictionary.TryGetValue(elementId, out value) ? _accessControlElementIdToAccessControlElementDictionary[value] : null;
		}
	}

	private void AddACElement(ACElement acElement)
	{
		Guid guid = Guid.Empty;
		if (acElement is ACType)
		{
			guid = GenerateAccessControlTypeElementId(acElement.Id, GuidProviderStrategy);
		}
		else if (acElement is ACInstance)
		{
			guid = GenerateAccessControlInstanceElementId(acElement.Id, GuidProviderStrategy);
		}
		_accessControlElementIdToACElementIdDictionary.Add(guid, acElement.Id);
		_acElementIdToAccessControlElementIdDictionary.Add(acElement.Id, guid);
		_acElementIdToACElementDictionary.Add(acElement.Id, acElement);
		AccessControlElement accessControlElement = BuildAccessControlElement(guid, acElement);
		_accessControlElementIdToAccessControlElementDictionary.Add(guid, accessControlElement);
		BuildChildrenDictionary(acElement, accessControlElement);
	}

	private void BuildChildrenDictionary(ACElement acElement, AccessControlElement accessControlElement)
	{
		ACUnit aCUnit = acElement as ACUnit;
		AccessControlUnit accessControlUnit = accessControlElement as AccessControlUnit;
		if (aCUnit != null && aCUnit.ParentUnitId != null && accessControlUnit != null)
		{
			if (!_parentIdToChildren.TryGetValue(aCUnit.ParentUnitId, out var value))
			{
				value = new List<AccessControlUnit>();
				_parentIdToChildren.Add(aCUnit.ParentUnitId, value);
			}
			value.Add(accessControlUnit);
		}
	}

	private IEnumerable<ACElement> GetACElements(Type acElementType)
	{
		List<ACElement> list = new List<ACElement>();
		foreach (ACElement value in _acElementIdToACElementDictionary.Values)
		{
			if (acElementType == value.GetType() || value.GetType().IsSubclassOf(acElementType))
			{
				list.Add(value);
			}
		}
		return list;
	}

	private Icon GetIcon(ACElement acElement)
	{
		return IconHandler.GetIcon(_accessControlSystemManager.PluginId, acElement.IconKey);
	}

	private Guid GetIconKey(ACElement acElement)
	{
		return IconHandler.GetIconKey(_accessControlSystemManager.PluginId, acElement.IconKey ?? string.Empty);
	}

	public Bitmap GetIconBitmap(ACElement acElement)
	{
		return IconHandler.GetIconBitmap(_accessControlSystemManager.PluginId, acElement.IconKey ?? string.Empty);
	}

	private AccessControlElement BuildAccessControlElement(Guid id, ACElement acElement)
	{
		AccessControlElement result = null;
		if (acElement is ACUnitType)
		{
			result = new AccessControlUnitType(id, _accessControlSystemManager);
		}
		if (acElement is ACServerType)
		{
			result = new AccessControlServerType(id, _accessControlSystemManager);
		}
		if (acElement is ACStateType)
		{
			result = new AccessControlStateType(id, _accessControlSystemManager);
		}
		if (acElement is ACCommandType)
		{
			result = new AccessControlCommandType(id, _accessControlSystemManager);
		}
		if (acElement is ACEventType)
		{
			result = new AccessControlEventType(id, _accessControlSystemManager);
		}
		if (acElement is ACServer)
		{
			result = new AccessControlServer(id, _accessControlSystemManager);
		}
		if (acElement is ACUnit)
		{
			result = new AccessControlUnit(id, _accessControlSystemManager);
		}
		return result;
	}

	private Guid GenerateAccessControlTypeElementId(string elementId, IGuidProviderStrategy guidProviderStrategy)
	{
		return guidProviderStrategy.Create(_accessControlSystemManager.PluginId, elementId);
	}

	private Guid GenerateAccessControlInstanceElementId(string elementId, IGuidProviderStrategy guidProviderStrategy)
	{
		return guidProviderStrategy.Create(_accessControlSystemManager.Id, elementId);
	}

	private void BuildIntegrationItems()
	{
		ServerId serverId = new ServerId("AC", "Access Control", 0, _accessControlSystemManager.Id);
		Dictionary<string, HashSet<ACUnit>> dictionary = new Dictionary<string, HashSet<ACUnit>>();
		HashSet<ACUnit> hashSet = new HashSet<ACUnit>();
		foreach (ACUnit aCElement in GetACElements(typeof(ACUnit)))
		{
			if (string.IsNullOrWhiteSpace(aCElement.ParentUnitId))
			{
				hashSet.Add(aCElement);
				continue;
			}
			if (dictionary.TryGetValue(aCElement.ParentUnitId, out var value))
			{
				value.Add(aCElement);
				continue;
			}
			value = new HashSet<ACUnit> { aCElement };
			dictionary.Add(aCElement.ParentUnitId, value);
		}
		foreach (ACServer aCElement2 in GetACElements(typeof(ACServer)))
		{
			AccessControlIntegrationItem accessControlIntegrationItem = new AccessControlIntegrationItem(serverId, null, _acElementIdToAccessControlElementIdDictionary[aCElement2.Id], _accessControlSystemManager.Name, FolderType.No, AccessControlKind.AccessControlServerKind, this);
			accessControlIntegrationItem.Icon = GetIconBitmap(aCElement2);
			accessControlIntegrationItem.MapIconKey = GetIconKey(aCElement2);
			List<Item> list = new List<Item>();
			AccessControlIntegrationItem accessControlIntegrationItem2 = new AccessControlIntegrationItem(serverId, accessControlIntegrationItem, AccessControlKind.AccessControlUnitKind, Strings.AccessControlUnitGroupName, FolderType.SystemDefined, AccessControlKind.AccessControlUnitKind, this);
			accessControlIntegrationItem2.SetChildren(BuildUnitIntegrationItemsRecursively(serverId, accessControlIntegrationItem2, null, hashSet, dictionary));
			list.Add(accessControlIntegrationItem2);
			accessControlIntegrationItem.SetChildren(list);
			_accessControlElementIdToIntegrationItemDictionary.Add(accessControlIntegrationItem.FQID.ObjectId, accessControlIntegrationItem);
		}
	}

	private List<Item> BuildUnitIntegrationItemsRecursively(ServerId serverId, AccessControlIntegrationItem parentItem, ACElement parentACElement, IEnumerable<ACUnit> units, Dictionary<string, HashSet<ACUnit>> parentIdToChildrenMap)
	{
		List<Item> list = new List<Item>();
		foreach (ACUnit unit in units)
		{
			AccessControlIntegrationItem accessControlIntegrationItem = new AccessControlIntegrationItem(serverId, parentItem, _acElementIdToAccessControlElementIdDictionary[unit.Id], unit.Name, FolderType.No, AccessControlKind.AccessControlUnitKind, this);
			accessControlIntegrationItem.Icon = GetIconBitmap(unit);
			accessControlIntegrationItem.MapIconKey = GetIconKey(unit);
			if (!parentIdToChildrenMap.TryGetValue(unit.Id, out var value))
			{
				value = new HashSet<ACUnit>();
			}
			accessControlIntegrationItem.SetChildren(BuildUnitIntegrationItemsRecursively(serverId, accessControlIntegrationItem, unit, value, parentIdToChildrenMap));
			_accessControlElementIdToIntegrationItemDictionary.Add(accessControlIntegrationItem.FQID.ObjectId, accessControlIntegrationItem);
			list.Add(accessControlIntegrationItem);
		}
		return list;
	}
}
