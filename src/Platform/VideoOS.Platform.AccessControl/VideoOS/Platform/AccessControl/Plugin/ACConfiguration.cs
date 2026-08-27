using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using VideoOS.Platform.AccessControl.Elements;
using VideoOS.Platform.AccessControl.Internal.Util;
using VideoOS.Platform.AccessControl.TypeCategories;

namespace VideoOS.Platform.AccessControl.Plugin;

public sealed class ACConfiguration
{
	public DateTime Timestamp { get; private set; }

	public ACServerType ACServerType { get; private set; }

	public ACServer ACServer { get; private set; }

	public ACElement[] ACElements { get; private set; }

	public static ACConfiguration CreateACConfiguration(DateTime timestamp, IEnumerable<ACElement> acElements)
	{
		acElements = new ReadOnlyCollection<ACElement>((acElements != null) ? acElements.ToList() : new List<ACElement>());
		try
		{
			ValidateConfiguration(acElements);
		}
		catch (Exception ex)
		{
			ACUtil.Log(error: true, "ACConfiguration.ValidateConfiguration", "Access control configuration failed. Error: " + ex.Message);
			throw;
		}
		return new ACConfiguration(timestamp.ToUniversalTime(), acElements);
	}

	private ACConfiguration(DateTime timestamp, IEnumerable<ACElement> acElements)
	{
		Timestamp = timestamp;
		ACServerType = acElements.OfType<ACServerType>().First();
		ACServer = acElements.OfType<ACServer>().First();
		ACElements = acElements.ToArray();
	}

	private static void ValidateConfiguration(IEnumerable<ACElement> acElements)
	{
		ACServerType aCServerType = null;
		ACServer aCServer = null;
		HashSet<string> hashSet = new HashSet<string>();
		HashSet<string> hashSet2 = new HashSet<string>();
		Dictionary<string, ACUnitType> dictionary = new Dictionary<string, ACUnitType>();
		List<ACInstance> list = new List<ACInstance>();
		Dictionary<string, ACUnit> dictionary2 = new Dictionary<string, ACUnit>();
		List<ACOperationableType> list2 = new List<ACOperationableType>();
		HashSet<string> hashSet3 = new HashSet<string>();
		HashSet<string> hashSet4 = new HashSet<string>();
		HashSet<string> hashSet5 = new HashSet<string>();
		HashSet<string> hashSet6 = new HashSet<string>();
		foreach (ACElement acElement in acElements)
		{
			if (string.IsNullOrEmpty(acElement.Id))
			{
				throw new ACConfigurationException("The ACElement with the name: '" + acElement.Name + "' does not have a Id.");
			}
			if (string.IsNullOrEmpty(acElement.Name))
			{
				throw new ACConfigurationException("The ACElement with the id: '" + acElement.Id + "' does not have a Name.");
			}
			if (hashSet.Contains(acElement.Id))
			{
				throw new ACConfigurationException("More than one ACElement has the id: '" + acElement.Id + "', " + acElement.Name);
			}
			hashSet.Add(acElement.Id);
			if (acElement is ACType)
			{
				ACType aCType = (ACType)acElement;
				hashSet2.Add(aCType.Id);
				if (aCType is ACOperationableType)
				{
					ACOperationableType aCOperationableType = (ACOperationableType)aCType;
					list2.Add(aCOperationableType);
					if (aCOperationableType is ACServerType)
					{
						if (aCServerType != null)
						{
							throw new ACConfigurationException("The configuration must contain only one ACServerType element.");
						}
						aCServerType = (ACServerType)aCOperationableType;
					}
					else if (aCOperationableType is ACUnitType)
					{
						ACUnitType aCUnitType = (ACUnitType)aCOperationableType;
						dictionary.Add(aCUnitType.Id, aCUnitType);
					}
				}
				else if (aCType is ACCommandType)
				{
					hashSet5.Add(aCType.Id);
				}
				else if (aCType is ACEventType)
				{
					hashSet4.Add(aCType.Id);
				}
				else if (aCType is ACStateType)
				{
					hashSet3.Add(aCType.Id);
				}
			}
			else
			{
				if (!(acElement is ACInstance))
				{
					continue;
				}
				list.Add((ACInstance)acElement);
				if (acElement is ACServer)
				{
					if (aCServer != null)
					{
						throw new ACConfigurationException("The configuration must contain only one ACServer element.");
					}
					aCServer = (ACServer)acElement;
				}
				else if (acElement is ACUnit)
				{
					ACUnit aCUnit = (ACUnit)acElement;
					dictionary2.Add(aCUnit.Id, aCUnit);
				}
			}
		}
		if (aCServerType == null)
		{
			throw new ACConfigurationException("The configuration does not contain an ACServerType element.");
		}
		if (aCServer == null)
		{
			throw new ACConfigurationException("The configuration does not contain an ACServer element.");
		}
		if (aCServer.InstanceTypeId != aCServerType.Id)
		{
			throw new ACConfigurationException("The InstanceType of the ACServer element must match the Id if the ACServerType element.");
		}
		foreach (ACInstance item in list)
		{
			if (!hashSet2.Contains(item.InstanceTypeId))
			{
				throw new ACConfigurationException("The InstanceType '" + item.InstanceTypeId + "' was not found in the configuration.");
			}
			if (!(item is ACUnit))
			{
				continue;
			}
			ACUnit aCUnit2 = (ACUnit)item;
			if (!string.IsNullOrWhiteSpace(aCUnit2.ParentUnitId))
			{
				if (aCUnit2.Id == aCUnit2.ParentUnitId)
				{
					throw new ACConfigurationException("The ACUnit '" + aCUnit2.Name + "' has the parent property set to itselves.");
				}
				if (!dictionary2.Keys.Contains(aCUnit2.ParentUnitId))
				{
					throw new ACConfigurationException("The ACUnit.ParentUnit '" + aCUnit2.ParentUnitId + "' was not found in the configuration.");
				}
			}
			if (IsAccessPoint(dictionary, aCUnit2) && (string.IsNullOrWhiteSpace(aCUnit2.ParentUnitId) || !IsDoor(dictionary, dictionary2[aCUnit2.ParentUnitId])))
			{
				throw new ACConfigurationException("The parent of the access point ACUnit '" + aCUnit2.Name + "' must be a door ACUnit");
			}
			HashSet<string> hashSet7 = new HashSet<string> { aCUnit2.Id };
			string parentUnitId = aCUnit2.ParentUnitId;
			while (!string.IsNullOrWhiteSpace(parentUnitId) && !hashSet6.Contains(parentUnitId))
			{
				if (hashSet7.Contains(parentUnitId))
				{
					ACUnit aCUnit3 = dictionary2[parentUnitId];
					throw new ACConfigurationException("The ACUnit '" + aCUnit3.Name + "' has a circular reference to itself");
				}
				hashSet7.Add(parentUnitId);
				parentUnitId = dictionary2[parentUnitId].ParentUnitId;
			}
			hashSet6.UnionWith(hashSet7);
			if (!IsDoor(dictionary, aCUnit2))
			{
				continue;
			}
			string parentUnitId2 = aCUnit2.ParentUnitId;
			while (!string.IsNullOrWhiteSpace(parentUnitId2))
			{
				ACUnit aCUnit4 = dictionary2[parentUnitId2];
				if (IsDoor(dictionary, aCUnit4))
				{
					throw new ACConfigurationException("The door ACUnit '" + aCUnit2.Name + "' can not be a decendant of another door ('" + aCUnit4.Name + "')");
				}
				parentUnitId2 = aCUnit4.ParentUnitId;
			}
		}
		foreach (ACOperationableType item2 in list2)
		{
			if (item2.CommandTypeIds != null)
			{
				foreach (string commandTypeId in item2.CommandTypeIds)
				{
					if (!hashSet5.Contains(commandTypeId))
					{
						throw new ACConfigurationException("The CommandType '" + commandTypeId + "' was not found in the configuration.");
					}
				}
			}
			if (item2.EventTypeIds != null)
			{
				foreach (string eventTypeId in item2.EventTypeIds)
				{
					if (!hashSet4.Contains(eventTypeId))
					{
						throw new ACConfigurationException("The EventType '" + eventTypeId + "' was not found in the configuration.");
					}
				}
			}
			if (item2.StateTypeIds == null)
			{
				continue;
			}
			foreach (string stateTypeId in item2.StateTypeIds)
			{
				if (!hashSet3.Contains(stateTypeId))
				{
					throw new ACConfigurationException("The StateType '" + stateTypeId + "' was not found in the configuration.");
				}
			}
		}
	}

	private static bool IsDoor(Dictionary<string, ACUnitType> acUnitTypeMapping, ACUnit acUnit)
	{
		return CheckCategory(acUnitTypeMapping, acUnit, ACBuiltInUnitTypeCategories.Door);
	}

	private static bool IsAccessPoint(Dictionary<string, ACUnitType> acUnitTypeMapping, ACUnit acUnit)
	{
		return CheckCategory(acUnitTypeMapping, acUnit, ACBuiltInUnitTypeCategories.AccessPoint);
	}

	private static bool CheckCategory(Dictionary<string, ACUnitType> acUnitTypeMapping, ACUnit acUnit, string category)
	{
		if (!acUnitTypeMapping.TryGetValue(acUnit.InstanceTypeId, out var value))
		{
			return false;
		}
		return value.Categories.Contains(category);
	}

	public bool EquivalentTo(ACConfiguration other)
	{
		if (other == null)
		{
			return false;
		}
		if (this == other)
		{
			return true;
		}
		return CollectionComparisonUtil.MultisetCompare(ACElements, other.ACElements);
	}
}
