using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Mobile.Portable.MetaChannel.CommEntries.AccessControl;
using VideoOS.Mobile.Portable.Utilities;

namespace VideoOS.Mobile.Portable.MetaChannel.CommEntries;

public class SerializableAccessControl
{
	public static List<SerializableItem> GetAccessControlEventsDetailsSerializedItem(IEnumerable<AccessControlEventDetails> accessControlEvents, bool includeDetails)
	{
		return accessControlEvents.Select((AccessControlEventDetails e) => GetAccessControlEventDetailsSerializedItem(e, includeDetails)).ToList();
	}

	public static SerializableItem GetAccessControlEventDetailsSerializedItem(AccessControlEventDetails accessControlEvent, bool includeDetails)
	{
		SerializableItem serializableItem = new SerializableItem
		{
			Id = accessControlEvent.Id
		};
		serializableItem.SetItemType(SerializableItemTypes.AccessControlEvent);
		serializableItem.Properties.Add("AccessControlRuleId", accessControlEvent.AccessControlRuleId.ToString());
		serializableItem.Properties.Add("AccessControlSystemName", accessControlEvent.AccessControlSystemName);
		serializableItem.Properties.Add("Message", accessControlEvent.Message);
		serializableItem.Properties.Add("Source", accessControlEvent.SourceName);
		serializableItem.Properties.Add("Timestamp", accessControlEvent.Timestamp.ToMilisecString());
		if (includeDetails)
		{
			serializableItem.Properties.Add("AccessControlSystemId", accessControlEvent.AccessControlSystemId.ToString());
			serializableItem.Properties.Add("SourceId", accessControlEvent.SourceId.ToString());
			serializableItem.Properties.Add("AccessControlReason", accessControlEvent.Reason);
			if (accessControlEvent.RelatedCameras != null && accessControlEvent.RelatedCameras.Any())
			{
				SerializableItem serializableItem2 = new SerializableItem
				{
					Name = "RelatedCameras"
				};
				serializableItem2.SetItemType(SerializableItemTypes.Folder);
				serializableItem2.Children.AddRange(accessControlEvent.RelatedCameras.Select((Tuple<Guid, string> x) => new SerializableItem(SerializableItemTypes.Camera)
				{
					Id = x.Item1,
					Name = x.Item2
				}).ToList());
				serializableItem.Children.Add(serializableItem2);
			}
			if (accessControlEvent.CredentialHolders != null && accessControlEvent.CredentialHolders.Any())
			{
				SerializableItem serializableItem3 = new SerializableItem
				{
					Name = "AccessControlCredentialHolders"
				};
				serializableItem3.SetItemType(SerializableItemTypes.Folder);
				serializableItem3.Children.AddRange(GetCredentialHoldersSerializedItems(accessControlEvent.CredentialHolders));
				serializableItem.Children.Add(serializableItem3);
			}
			if (accessControlEvent.Commands != null && accessControlEvent.Commands.Any())
			{
				SerializableItem serializableItem4 = new SerializableItem
				{
					Name = "AccessControlCommands"
				};
				serializableItem4.SetItemType(SerializableItemTypes.Folder);
				serializableItem4.Children.AddRange(GetCommandsSerializedItems(accessControlEvent.Commands));
				serializableItem.Children.Add(serializableItem4);
			}
		}
		return serializableItem;
	}

	public static AccessControlEventDetails GetAccessControlEventDetails(SerializableItem item)
	{
		AccessControlEventDetails accessControlEventDetails = null;
		if (item != null && item.GetItemType() == SerializableItemTypes.AccessControlEvent)
		{
			accessControlEventDetails = new AccessControlEventDetails
			{
				Id = item.Id
			};
			SerializableProperties<string, string, StringStringFactory> properties = item.Properties;
			if (properties.ContainsKey("AccessControlRuleId"))
			{
				accessControlEventDetails.AccessControlRuleId = new Guid(properties["AccessControlRuleId"]);
			}
			if (properties.ContainsKey("AccessControlSystemId"))
			{
				accessControlEventDetails.AccessControlSystemId = new Guid(properties["AccessControlSystemId"]);
			}
			if (properties.ContainsKey("AccessControlSystemName"))
			{
				accessControlEventDetails.AccessControlSystemName = properties["AccessControlSystemName"];
			}
			if (properties.ContainsKey("Message"))
			{
				accessControlEventDetails.Message = properties["Message"];
			}
			if (properties.ContainsKey("SourceId"))
			{
				accessControlEventDetails.SourceId = new Guid(properties["SourceId"]);
			}
			if (properties.ContainsKey("Source"))
			{
				accessControlEventDetails.SourceName = properties["Source"];
			}
			if (properties.ContainsKey("Timestamp"))
			{
				accessControlEventDetails.Timestamp = TimeConverter.FromString(properties["Timestamp"]);
			}
			if (properties.ContainsKey("AccessControlReason"))
			{
				accessControlEventDetails.Reason = properties["AccessControlReason"];
			}
			if (item.Children.Count > 0)
			{
				SerializableItem serializableItem = item.Children.SingleOrDefault((SerializableItem child) => !string.IsNullOrEmpty(child.Name) && child.Name.Equals("RelatedCameras"));
				accessControlEventDetails.RelatedCameras = ((serializableItem != null) ? serializableItem.Children.Select((SerializableItem camera) => new Tuple<Guid, string>(camera.Id, camera.Name)).ToList() : new List<Tuple<Guid, string>>());
				SerializableItem serializableItem2 = item.Children.SingleOrDefault((SerializableItem child) => !string.IsNullOrEmpty(child.Name) && child.Name.Equals("AccessControlCredentialHolders"));
				if (serializableItem2 != null && serializableItem2.Children != null && serializableItem2.Children.Any())
				{
					accessControlEventDetails.CredentialHolders = new List<AccessControlCredentialHolder>();
					foreach (SerializableItem child in serializableItem2.Children)
					{
						AccessControlCredentialHolder accessControlCredentialHolder = new AccessControlCredentialHolder
						{
							Name = (child.Properties.ContainsKey("AccessControlCredentialHolderName") ? child.Properties["AccessControlCredentialHolderName"] : string.Empty),
							PictureData = ((child.Properties.ContainsKey("AccessControlCredentialHolderPictureData") && child.Properties["AccessControlCredentialHolderPictureData"] != null) ? Convert.FromBase64String(child.Properties["AccessControlCredentialHolderPictureData"]) : null)
						};
						accessControlCredentialHolder.CredentialHolderRoles = child.Children.SingleOrDefault((SerializableItem child) => !string.IsNullOrEmpty(child.Name) && child.Name.Equals("AccessControlCredentialHolderRoles"))?.Children.Select((SerializableItem role) => role.Name).ToList();
						SerializableItem serializableItem3 = child.Children.SingleOrDefault((SerializableItem child) => !string.IsNullOrEmpty(child.Name) && child.Name.Equals("AccessControlCredentialHolderProperties"));
						if (serializableItem3 != null && serializableItem3.Children != null && serializableItem3.Children.Any())
						{
							accessControlCredentialHolder.Properties = new Dictionary<string, string>();
							foreach (SerializableItem child2 in serializableItem3.Children)
							{
								accessControlCredentialHolder.Properties.Add((child2 != null && child2.Properties.ContainsKey("AccessControlCredentialHolderPropertyName")) ? child2.Properties["AccessControlCredentialHolderPropertyName"] : string.Empty, (child2 != null && child2.Properties.ContainsKey("AccessControlCredentialHolderPropertyValue")) ? child2.Properties["AccessControlCredentialHolderPropertyValue"] : string.Empty);
							}
						}
						accessControlEventDetails.CredentialHolders.Add(accessControlCredentialHolder);
					}
				}
				SerializableItem serializableItem4 = item.Children.SingleOrDefault((SerializableItem child) => !string.IsNullOrEmpty(child.Name) && child.Name.Equals("AccessControlCommands"));
				if (serializableItem4 != null && serializableItem4.Children != null && serializableItem4.Children.Any())
				{
					accessControlEventDetails.Commands = new List<AccessControlCommandBase>();
					foreach (SerializableItem child3 in serializableItem4.Children)
					{
						if (Enum.TryParse<AccessControlCommandType>(child3.Properties.ContainsKey("AccessControlCommandType") ? child3.Properties["AccessControlCommandType"] : null, out var result))
						{
							switch (result)
							{
							case AccessControlCommandType.RuleCommand:
								accessControlEventDetails.Commands.Add(new AccessControlRuleCommand
								{
									AccessControlOperationalInstanceId = (child3.Properties.ContainsKey("AccessControlOperationalInstanceId") ? new Guid(child3.Properties["AccessControlOperationalInstanceId"]) : Guid.Empty),
									AccessControlCommandTypeId = (child3.Properties.ContainsKey("AccessControlCommandTypeId") ? new Guid(child3.Properties["AccessControlCommandTypeId"]) : Guid.Empty),
									Label = (child3.Properties.ContainsKey("AccessControlCommandLabel") ? child3.Properties["AccessControlCommandLabel"] : string.Empty),
									Order = (child3.Properties.ContainsKey("AccessControlCommandOrder") ? Convert.ToInt32(child3.Properties["AccessControlCommandOrder"]) : 0)
								});
								break;
							case AccessControlCommandType.SystemCommand:
								accessControlEventDetails.Commands.Add(new AccessControlSystemCommand
								{
									EventId = (child3.Properties.ContainsKey("AccessControlEventId") ? new Guid(child3.Properties["AccessControlEventId"]) : Guid.Empty),
									TriggerSourceObjectId = (child3.Properties.ContainsKey("AccessControlTriggerSourceObjectId") ? new Guid(child3.Properties["AccessControlTriggerSourceObjectId"]) : Guid.Empty),
									KindId = (child3.Properties.ContainsKey("AccessControlKindId") ? new Guid(child3.Properties["AccessControlKindId"]) : Guid.Empty),
									Label = (child3.Properties.ContainsKey("AccessControlCommandLabel") ? child3.Properties["AccessControlCommandLabel"] : string.Empty)
								});
								break;
							}
						}
					}
				}
			}
		}
		return accessControlEventDetails;
	}

	public static IEnumerable<AccessControlEventDetails> GetAccessControlEventsDetail(IEnumerable<SerializableItem> accessControlEvents)
	{
		return accessControlEvents?.Select(GetAccessControlEventDetails).ToList();
	}

	public static List<SerializableItem> GetAccessControlDoorsSerializedItems(IEnumerable<AccessControlDoor> accessControlDoors)
	{
		return accessControlDoors.Select(GetAccessControlDoorSerializedItem).ToList();
	}

	public static SerializableItem GetAccessControlDoorSerializedItem(AccessControlDoor accessControlDoor)
	{
		SerializableItem serializableItem = new SerializableItem
		{
			Id = accessControlDoor.Id
		};
		serializableItem.SetItemType(SerializableItemTypes.AccessControlDoor);
		serializableItem.Properties.Add("AccessControlSystemId", accessControlDoor.AccessControlSystemId.ToString());
		serializableItem.Properties.Add("AccessControlDoorName", accessControlDoor.Name);
		serializableItem.Properties.Add("AccessControlSystemName", accessControlDoor.AccessControlSystemName);
		serializableItem.Properties.Add("AccessControlDoorType", accessControlDoor.Type);
		if (accessControlDoor.Categories != null && accessControlDoor.Categories.Any())
		{
			SerializableItem serializableItem2 = new SerializableItem
			{
				Name = "AccessControlDoorCategories"
			};
			serializableItem2.SetItemType(SerializableItemTypes.Folder);
			serializableItem2.Children.AddRange(GetDoorCategoriesSerializedItems(accessControlDoor.Categories));
			serializableItem.Children.Add(serializableItem2);
		}
		if (accessControlDoor.RelatedCameras != null && accessControlDoor.RelatedCameras.Any())
		{
			SerializableItem serializableItem3 = new SerializableItem
			{
				Name = "RelatedCameras"
			};
			serializableItem3.SetItemType(SerializableItemTypes.Folder);
			serializableItem3.Children.AddRange(accessControlDoor.RelatedCameras.Select((Tuple<Guid, string> x) => new SerializableItem(SerializableItemTypes.Camera)
			{
				Id = x.Item1,
				Name = x.Item2
			}).ToList());
			serializableItem.Children.Add(serializableItem3);
		}
		if (accessControlDoor.Commands != null && accessControlDoor.Commands.Any())
		{
			SerializableItem serializableItem4 = new SerializableItem
			{
				Name = "AccessControlCommands"
			};
			serializableItem4.SetItemType(SerializableItemTypes.Folder);
			serializableItem4.Children.AddRange(GetCommandsSerializedItems(accessControlDoor.Commands));
			serializableItem.Children.Add(serializableItem4);
		}
		return serializableItem;
	}

	public static IEnumerable<AccessControlDoor> GetAccessControlDoors(IEnumerable<SerializableItem> accessControlDoors)
	{
		return accessControlDoors?.Select(GetAccessControlDoor).ToList();
	}

	public static AccessControlDoor GetAccessControlDoor(SerializableItem item)
	{
		AccessControlDoor accessControlDoor = null;
		if (item != null && item.GetItemType() == SerializableItemTypes.AccessControlDoor)
		{
			accessControlDoor = new AccessControlDoor
			{
				Id = item.Id
			};
			SerializableProperties<string, string, StringStringFactory> properties = item.Properties;
			if (properties.ContainsKey("AccessControlSystemId"))
			{
				accessControlDoor.AccessControlSystemId = new Guid(properties["AccessControlSystemId"]);
			}
			if (properties.ContainsKey("AccessControlDoorName"))
			{
				accessControlDoor.Name = properties["AccessControlDoorName"];
			}
			if (properties.ContainsKey("AccessControlSystemName"))
			{
				accessControlDoor.AccessControlSystemName = properties["AccessControlSystemName"];
			}
			if (properties.ContainsKey("AccessControlDoorType"))
			{
				accessControlDoor.Type = properties["AccessControlDoorType"];
			}
			if (item.Children.Count > 0)
			{
				SerializableItem serializableItem = item.Children.SingleOrDefault((SerializableItem child) => !string.IsNullOrEmpty(child.Name) && child.Name.Equals("AccessControlDoorCategories"));
				accessControlDoor.Categories = ((serializableItem != null) ? serializableItem.Children.Select((SerializableItem category) => category.Name).ToList() : new List<string>());
				SerializableItem serializableItem2 = item.Children.SingleOrDefault((SerializableItem child) => !string.IsNullOrEmpty(child.Name) && child.Name.Equals("RelatedCameras"));
				accessControlDoor.RelatedCameras = ((serializableItem2 != null) ? serializableItem2.Children.Select((SerializableItem camera) => new Tuple<Guid, string>(camera.Id, camera.Name)).ToList() : new List<Tuple<Guid, string>>());
				SerializableItem serializableItem3 = item.Children.SingleOrDefault((SerializableItem child) => !string.IsNullOrEmpty(child.Name) && child.Name.Equals("AccessControlCommands"));
				if (serializableItem3 != null && serializableItem3.Children != null && serializableItem3.Children.Any())
				{
					accessControlDoor.Commands = new List<AccessControlCommandBase>();
					foreach (SerializableItem child in serializableItem3.Children)
					{
						if (Enum.TryParse<AccessControlCommandType>(child.Properties.ContainsKey("AccessControlCommandType") ? child.Properties["AccessControlCommandType"] : null, out var result))
						{
							switch (result)
							{
							case AccessControlCommandType.RuleCommand:
								accessControlDoor.Commands.Add(new AccessControlRuleCommand
								{
									AccessControlOperationalInstanceId = (child.Properties.ContainsKey("AccessControlOperationalInstanceId") ? new Guid(child.Properties["AccessControlOperationalInstanceId"]) : Guid.Empty),
									AccessControlCommandTypeId = (child.Properties.ContainsKey("AccessControlCommandTypeId") ? new Guid(child.Properties["AccessControlCommandTypeId"]) : Guid.Empty),
									Label = (child.Properties.ContainsKey("AccessControlCommandLabel") ? child.Properties["AccessControlCommandLabel"] : string.Empty),
									Order = (child.Properties.ContainsKey("AccessControlCommandOrder") ? Convert.ToInt32(child.Properties["AccessControlCommandOrder"]) : 0)
								});
								break;
							case AccessControlCommandType.SystemCommand:
								accessControlDoor.Commands.Add(new AccessControlSystemCommand
								{
									EventId = (child.Properties.ContainsKey("AccessControlEventId") ? new Guid(child.Properties["AccessControlEventId"]) : Guid.Empty),
									TriggerSourceObjectId = (child.Properties.ContainsKey("AccessControlTriggerSourceObjectId") ? new Guid(child.Properties["AccessControlTriggerSourceObjectId"]) : Guid.Empty),
									KindId = (child.Properties.ContainsKey("AccessControlKindId") ? new Guid(child.Properties["AccessControlKindId"]) : Guid.Empty),
									Label = (child.Properties.ContainsKey("AccessControlCommandLabel") ? child.Properties["AccessControlCommandLabel"] : string.Empty)
								});
								break;
							}
						}
					}
				}
			}
		}
		return accessControlDoor;
	}

	public static SerializableItem GetAccessControlDoorStatesSerializedItem(AccessControlDoorStates accessControlDoorStates)
	{
		SerializableItem serializableItem = new SerializableItem();
		serializableItem.SetItemType(SerializableItemTypes.AccessControlDoorStates);
		serializableItem.Properties.Add("AccessControlDoorStatesReceivedTime", accessControlDoorStates.ReceivedTime.ToMilisecString());
		if (accessControlDoorStates.States != null && accessControlDoorStates.States.Any())
		{
			SerializableItem serializableItem2 = new SerializableItem
			{
				Name = "AccessControlDoorStateNames"
			};
			serializableItem2.SetItemType(SerializableItemTypes.Folder);
			serializableItem2.Children.AddRange(GetDoorStateNamesSerializedItems(accessControlDoorStates.States));
			serializableItem.Children.Add(serializableItem2);
		}
		return serializableItem;
	}

	public static AccessControlDoorStates GetAccessControlDoorStates(SerializableItem item)
	{
		AccessControlDoorStates accessControlDoorStates = null;
		if (item != null && item.GetItemType() == SerializableItemTypes.AccessControlDoorStates)
		{
			accessControlDoorStates = new AccessControlDoorStates();
			SerializableProperties<string, string, StringStringFactory> properties = item.Properties;
			if (properties.ContainsKey("AccessControlDoorStatesReceivedTime"))
			{
				accessControlDoorStates.ReceivedTime = TimeConverter.FromString(properties["AccessControlDoorStatesReceivedTime"]);
			}
			if (item.Children.Count > 0)
			{
				SerializableItem serializableItem = item.Children.SingleOrDefault((SerializableItem child) => !string.IsNullOrEmpty(child.Name) && child.Name.Equals("AccessControlDoorStateNames"));
				accessControlDoorStates.States = ((serializableItem != null) ? serializableItem.Children.Select((SerializableItem state) => state.Name).ToList() : new List<string>());
			}
		}
		return accessControlDoorStates;
	}

	public static List<SerializableItem> GetAccessControlEventTypesSerializedItems(IEnumerable<AccessControlEventType> accessControlEventTypes)
	{
		return accessControlEventTypes.Select(GetAccessControlEventTypeSerializedItem).ToList();
	}

	public static SerializableItem GetAccessControlEventTypeSerializedItem(AccessControlEventType accessControlEventType)
	{
		SerializableItem serializableItem = new SerializableItem();
		serializableItem.Id = accessControlEventType.Id;
		serializableItem.SetItemType(SerializableItemTypes.AccessControlEventType);
		serializableItem.Properties.Add("AccessControlSystemId", accessControlEventType.AccessControlSystemId.ToString());
		serializableItem.Properties.Add("AccessControlEventTypeName", accessControlEventType.Name);
		return serializableItem;
	}

	public static IEnumerable<AccessControlEventType> GetAccessControlEventTypes(IEnumerable<SerializableItem> accessControlEventTypes)
	{
		return accessControlEventTypes?.Select(GetAccessControlEventType).ToList();
	}

	public static AccessControlEventType GetAccessControlEventType(SerializableItem item)
	{
		AccessControlEventType accessControlEventType = null;
		if (item != null && item.GetItemType() == SerializableItemTypes.AccessControlEventType)
		{
			accessControlEventType = new AccessControlEventType
			{
				Id = item.Id
			};
			SerializableProperties<string, string, StringStringFactory> properties = item.Properties;
			if (properties.ContainsKey("AccessControlSystemId"))
			{
				accessControlEventType.AccessControlSystemId = new Guid(properties["AccessControlSystemId"]);
			}
			if (properties.ContainsKey("AccessControlEventTypeName"))
			{
				accessControlEventType.Name = properties["AccessControlEventTypeName"];
			}
		}
		return accessControlEventType;
	}

	public static List<SerializableItem> GetAccessControlSystemsSerializedItems(IEnumerable<AccessControlSystem> accessControlSystems)
	{
		return accessControlSystems.Select(GetAccessControlSystemSerializedItem).ToList();
	}

	public static SerializableItem GetAccessControlSystemSerializedItem(AccessControlSystem accessControlSystem)
	{
		SerializableItem serializableItem = new SerializableItem();
		serializableItem.Id = accessControlSystem.Id;
		serializableItem.SetItemType(SerializableItemTypes.AccessControlSystem);
		serializableItem.Properties.Add("AccessControlSystemName", accessControlSystem.Name);
		return serializableItem;
	}

	public static IEnumerable<AccessControlSystem> GetAccessControlSystems(IEnumerable<SerializableItem> accessControlSystems)
	{
		return accessControlSystems?.Select(GetAccessControlSystem).ToList();
	}

	public static AccessControlSystem GetAccessControlSystem(SerializableItem item)
	{
		AccessControlSystem accessControlSystem = null;
		if (item != null && item.GetItemType() == SerializableItemTypes.AccessControlSystem)
		{
			accessControlSystem = new AccessControlSystem
			{
				Id = item.Id
			};
			SerializableProperties<string, string, StringStringFactory> properties = item.Properties;
			if (properties.ContainsKey("AccessControlSystemName"))
			{
				accessControlSystem.Name = properties["AccessControlSystemName"];
			}
		}
		return accessControlSystem;
	}

	public static List<SerializableItem> GetAccessControlDoorCategoriesSerializedItems(IEnumerable<AccessControlDoorCategory> accessControlDoorCategories)
	{
		return accessControlDoorCategories.Select(GetAccessControlDoorCategorySerializedItem).ToList();
	}

	public static SerializableItem GetAccessControlDoorCategorySerializedItem(AccessControlDoorCategory accessControlDoorCategory)
	{
		SerializableItem serializableItem = new SerializableItem();
		serializableItem.Id = accessControlDoorCategory.Id;
		serializableItem.SetItemType(SerializableItemTypes.AccessControlDoorCategory);
		serializableItem.Properties.Add("AccessControlDoorCategoryName", accessControlDoorCategory.Name);
		return serializableItem;
	}

	public static IEnumerable<AccessControlDoorCategory> GetAccessControlDoorCategories(IEnumerable<SerializableItem> accessControlDoorCategoies)
	{
		return accessControlDoorCategoies?.Select(GetAccessControlDoorCategory).ToList();
	}

	public static AccessControlDoorCategory GetAccessControlDoorCategory(SerializableItem item)
	{
		AccessControlDoorCategory accessControlDoorCategory = null;
		if (item != null && item.GetItemType() == SerializableItemTypes.AccessControlDoorCategory)
		{
			accessControlDoorCategory = new AccessControlDoorCategory
			{
				Id = item.Id
			};
			SerializableProperties<string, string, StringStringFactory> properties = item.Properties;
			if (properties.ContainsKey("AccessControlDoorCategoryName"))
			{
				accessControlDoorCategory.Name = properties["AccessControlDoorCategoryName"];
			}
		}
		return accessControlDoorCategory;
	}

	public static List<SerializableItem> GetAccessControlDoorTypesSerializedItems(IEnumerable<AccessControlDoorType> accessControlDoorTypes)
	{
		return accessControlDoorTypes.Select(GetAccessControlDoorTypeSerializedItem).ToList();
	}

	public static SerializableItem GetAccessControlDoorTypeSerializedItem(AccessControlDoorType accessControlDoorType)
	{
		SerializableItem serializableItem = new SerializableItem();
		serializableItem.Id = accessControlDoorType.Id;
		serializableItem.SetItemType(SerializableItemTypes.AccessControlDoorType);
		serializableItem.Properties.Add("AccessControlSystemId", accessControlDoorType.AccessControlSystemId.ToString());
		serializableItem.Properties.Add("AccessControlDoorTypeName", accessControlDoorType.Name);
		return serializableItem;
	}

	public static IEnumerable<AccessControlDoorType> GetAccessControlDoorTypes(IEnumerable<SerializableItem> accessControlDoorTypes)
	{
		return accessControlDoorTypes?.Select(GetAccessControlDoorType).ToList();
	}

	public static AccessControlDoorType GetAccessControlDoorType(SerializableItem item)
	{
		AccessControlDoorType accessControlDoorType = null;
		if (item != null && item.GetItemType() == SerializableItemTypes.AccessControlDoorType)
		{
			accessControlDoorType = new AccessControlDoorType
			{
				Id = item.Id
			};
			SerializableProperties<string, string, StringStringFactory> properties = item.Properties;
			if (properties.ContainsKey("AccessControlSystemId"))
			{
				accessControlDoorType.AccessControlSystemId = new Guid(properties["AccessControlSystemId"]);
			}
			if (properties.ContainsKey("AccessControlDoorTypeName"))
			{
				accessControlDoorType.Name = properties["AccessControlDoorTypeName"];
			}
		}
		return accessControlDoorType;
	}

	public static List<SerializableItem> GetAccessControlEventCategoriesSerializedItems(IEnumerable<AccessControlEventCategory> accessControlEventCategories)
	{
		return accessControlEventCategories.Select(GetAccessControlEventCategorySerializedItem).ToList();
	}

	public static SerializableItem GetAccessControlEventCategorySerializedItem(AccessControlEventCategory accessControlEventCategory)
	{
		SerializableItem serializableItem = new SerializableItem();
		serializableItem.Id = accessControlEventCategory.Id;
		serializableItem.SetItemType(SerializableItemTypes.AccessControlEventCategory);
		serializableItem.Properties.Add("AccessControlEventCategoryName", accessControlEventCategory.Name);
		return serializableItem;
	}

	public static IEnumerable<AccessControlEventCategory> GetAccessControlEventCategories(IEnumerable<SerializableItem> accessControlEventCategoies)
	{
		return accessControlEventCategoies?.Select(GetAccessControlEventCategory).ToList();
	}

	public static AccessControlEventCategory GetAccessControlEventCategory(SerializableItem item)
	{
		AccessControlEventCategory accessControlEventCategory = null;
		if (item != null && item.GetItemType() == SerializableItemTypes.AccessControlEventCategory)
		{
			accessControlEventCategory = new AccessControlEventCategory
			{
				Id = item.Id
			};
			SerializableProperties<string, string, StringStringFactory> properties = item.Properties;
			if (properties.ContainsKey("AccessControlEventCategoryName"))
			{
				accessControlEventCategory.Name = properties["AccessControlEventCategoryName"];
			}
		}
		return accessControlEventCategory;
	}

	private static List<SerializableItem> GetDoorCategoriesSerializedItems(IEnumerable<string> categories)
	{
		return categories.Select((string category) => new SerializableItem
		{
			Name = category
		}).ToList();
	}

	private static List<SerializableItem> GetDoorStateNamesSerializedItems(IEnumerable<string> states)
	{
		return states.Select((string state) => new SerializableItem
		{
			Name = state
		}).ToList();
	}

	private static List<SerializableItem> GetCredentialHoldersSerializedItems(IEnumerable<AccessControlCredentialHolder> holders)
	{
		return holders.Select(GetCredentialHolderSerializedItem).ToList();
	}

	private static SerializableItem GetCredentialHolderSerializedItem(AccessControlCredentialHolder holder)
	{
		SerializableItem serializableItem = new SerializableItem();
		serializableItem.Properties.Add("AccessControlCredentialHolderName", holder.Name);
		serializableItem.Properties.Add("AccessControlCredentialHolderPictureData", (holder.PictureData != null) ? Convert.ToBase64String(holder.PictureData) : null);
		if (holder.CredentialHolderRoles != null && holder.CredentialHolderRoles.Any())
		{
			SerializableItem serializableItem2 = new SerializableItem
			{
				Name = "AccessControlCredentialHolderRoles"
			};
			serializableItem2.SetItemType(SerializableItemTypes.Folder);
			serializableItem2.Children.AddRange(holder.CredentialHolderRoles.Select((string role) => new SerializableItem
			{
				Name = role
			}).ToList());
			serializableItem.Children.Add(serializableItem2);
		}
		if (holder.Properties != null && holder.Properties.Any())
		{
			SerializableItem serializableItem3 = new SerializableItem
			{
				Name = "AccessControlCredentialHolderProperties"
			};
			serializableItem3.SetItemType(SerializableItemTypes.Folder);
			foreach (KeyValuePair<string, string> property in holder.Properties)
			{
				SerializableItem serializableItem4 = new SerializableItem();
				serializableItem4.Properties.Add("AccessControlCredentialHolderPropertyName", property.Key);
				serializableItem4.Properties.Add("AccessControlCredentialHolderPropertyValue", property.Value);
				serializableItem3.Children.Add(serializableItem4);
			}
			serializableItem.Children.Add(serializableItem3);
		}
		return serializableItem;
	}

	private static List<SerializableItem> GetCommandsSerializedItems(IEnumerable<AccessControlCommandBase> commands)
	{
		return commands.Select(GetCommandSerializedItem).ToList();
	}

	private static SerializableItem GetCommandSerializedItem(AccessControlCommandBase command)
	{
		SerializableItem serializableItem = new SerializableItem();
		serializableItem.Properties.Add("AccessControlCommandType", command.CommandType.ToString());
		serializableItem.Properties.Add("AccessControlCommandLabel", command.Label);
		if (command is AccessControlRuleCommand)
		{
			AddRuleCommandProperties(serializableItem, (AccessControlRuleCommand)command);
		}
		else if (command is AccessControlSystemCommand)
		{
			AddSystemCommandProperties(serializableItem, (AccessControlSystemCommand)command);
		}
		return serializableItem;
	}

	private static void AddRuleCommandProperties(SerializableItem result, AccessControlRuleCommand command)
	{
		result.Properties.Add("AccessControlOperationalInstanceId", command.AccessControlOperationalInstanceId.ToString());
		result.Properties.Add("AccessControlCommandTypeId", command.AccessControlCommandTypeId.ToString());
		result.Properties.Add("AccessControlCommandOrder", command.Order.ToString());
	}

	private static void AddSystemCommandProperties(SerializableItem result, AccessControlSystemCommand command)
	{
		result.Properties.Add("AccessControlEventId", command.EventId.ToString());
		result.Properties.Add("AccessControlTriggerSourceObjectId", command.TriggerSourceObjectId.ToString());
		result.Properties.Add("AccessControlKindId", command.KindId.ToString());
	}
}
