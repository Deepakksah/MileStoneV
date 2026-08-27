using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using VideoOS.ConfigurationAPI;
using VideoOS.ConfigurationAPI.ConfigurationFaultException;
using VideoOS.EventServer.Communication.Configuration;
using VideoOS.Management.Common;
using VideoOS.Management.Server.ConfigurationApi.AccessControl.Builders;
using VideoOS.Management.Server.ConfigurationApi.Converters;
using VideoOS.Management.Server.ConfigurationApi.Validators;
using VideoOS.Management.Server.Properties;
using VideoOS.Platform;
using VideoOS.Platform.ConfigurationItems;

namespace VideoOS.Management.Server.ConfigurationApi;

[EntityName("ConfigAPIAlarmDefinition")]
[EntityName("ConfigAPIAlarmDefinitionFolder")]
public class AlarmDefinitionConfigurationApi : ConfigurationApiBase, IConfigurationApiSupport
{
	private static VideoOS.EventServer.Communication.Configuration.EventTypeGroup EventTypeGroupDefault = new VideoOS.EventServer.Communication.Configuration.EventTypeGroup
	{
		Name = "",
		ID = AlarmDefinitionConstants.SystemEventsGroup
	};

	private readonly ICommonValidator _commonValidator = new CommonValidator();

	private readonly IFQIDConverter _fqidConverter = new FQIDConverter();

	public ValidateResult SetItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		ConfigurationApiBase.ThrowIfAnyParameterNull(requestContext, item, path, parentPath);
		_commonValidator.ValidatePath(path);
		_commonValidator.ValidatePath(parentPath);
		MIPFactory.CheckAndInitialize(requestContext);
		if (path.IsFolder)
		{
			throw new NotSupportedException();
		}
		int mapType = 0;
		bool? assignableToAdmins = null;
		AlarmDataSettings alarmDataSettings = AlarmDefinitionConfigurationManager.Instance.GetAlarmDataSettings(requestContext);
		List<VideoOS.EventServer.Communication.Configuration.EventTypeGroup> eventTypeGroups = AlarmDefinitionConfigurationManager.Instance.GetEventTypeGroups(requestContext);
		List<VideoOS.EventServer.Communication.Configuration.EventType> eventTypes = AlarmDefinitionConfigurationManager.Instance.GetEventTypes(requestContext, CultureInfo.InvariantCulture);
		Dictionary<string, MapUpdateData> mapList = AlarmDefinitionConfigurationManager.Instance.GetMapList(requestContext);
		ValidateResult validateResult = ValidateItemInternal(requestContext, item, path, alarmDataSettings, eventTypeGroups, eventTypes, out var name, out var description, out var _, out var eventTypeId, out var sourceList, out var enableRule, out var timeProfileId, out var enableEventList, out var disableEventList, out var managementTimeoutTime, out var managementTimeoutEventList, out var relatedCameraList, out var relatedMapId, out var owner, out var priority, out var category, out var triggerEventList, out var autoClose, out mapType, out assignableToAdmins);
		if (!validateResult.ValidatedOk)
		{
			return validateResult;
		}
		VideoOS.EventServer.Communication.Configuration.AlarmDefinition alarmDefinition = AlarmDefinitionConfigurationManager.Instance.GetAlarmDefinition(requestContext, new Guid(path.Id));
		if (alarmDefinition == null)
		{
			throw new PathNotFoundException();
		}
		AlarmAction[] alarmActions = ((relatedMapId == Guid.Empty) ? new AlarmAction[0] : new AlarmAction[1]
		{
			new AlarmAction
			{
				Id = Guid.NewGuid(),
				ActionType = AlarmActionConstants.AlarmActionMapChange,
				Data = PackAction(relatedMapId, "Name unknown")
			}
		});
		alarmDefinition.Enabled = item.EnableProperty.Enabled;
		alarmDefinition.Name = name;
		alarmDefinition.Description = description;
		if (alarmDefinition.EventType != eventTypeId)
		{
			sourceList = new FQID[0];
		}
		alarmDefinition.EventType = eventTypeId;
		alarmDefinition.Sources = sourceList;
		alarmDefinition.EnableRule = enableRule;
		alarmDefinition.TimeProfileId = timeProfileId;
		alarmDefinition.EnablingEvents = enableEventList;
		alarmDefinition.DisablingEvents = disableEventList;
		alarmDefinition.ManagementTimeoutTime = managementTimeoutTime;
		alarmDefinition.ManagementTimeoutEvents = managementTimeoutEventList;
		alarmDefinition.RelatedCameras = relatedCameraList;
		alarmDefinition.AlarmActions = alarmActions;
		alarmDefinition.Owner = owner;
		alarmDefinition.Priority = priority;
		alarmDefinition.Category = category;
		alarmDefinition.TriggerEvents = triggerEventList;
		alarmDefinition.AutoClose = autoClose;
		alarmDefinition.MapType = mapType;
		if (assignableToAdmins.HasValue)
		{
			alarmDefinition.AssignableToAdmins = assignableToAdmins.Value;
		}
		AlarmDefinitionConfigurationManager.Instance.SaveAlarmDefinition(requestContext, alarmDefinition);
		Integration.FireConfigurationChanged(requestContext, "AlarmDefinition", alarmDefinition.Id.ToString(), EventAction.Updated, alarmDefinition.Name);
		ConfigurationItem resultItem = ConvertToAlarmDefinitionConfigurationItem(requestContext, parentPath, alarmDefinition, alarmDataSettings, eventTypeGroups, eventTypes, mapList);
		validateResult = new ValidateResult();
		validateResult.ValidatedOk = true;
		validateResult.ResultItem = resultItem;
		return validateResult;
	}

	public ValidateResult ValidateItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		ConfigurationApiBase.ThrowIfAnyParameterNull(requestContext, item, path, parentPath);
		_commonValidator.ValidatePath(path);
		if (path.ItemType == "AlarmDefinitionFolder")
		{
			if (parentPath.Text != "/")
			{
				throw new PathInvalidException("Invalid parent. Path: " + path.Text);
			}
		}
		else
		{
			_commonValidator.ValidatePath(parentPath);
		}
		MIPFactory.CheckAndInitialize(requestContext);
		if (!path.IsFolder && AlarmDefinitionConfigurationManager.Instance.GetAlarmDefinition(requestContext, new Guid(path.Id)) == null)
		{
			throw new PathNotFoundException(path.Text);
		}
		int mapType = 0;
		bool? assignableToAdmins = null;
		AlarmDataSettings alarmDataSettings = AlarmDefinitionConfigurationManager.Instance.GetAlarmDataSettings(requestContext);
		List<VideoOS.EventServer.Communication.Configuration.EventTypeGroup> eventTypeGroups = AlarmDefinitionConfigurationManager.Instance.GetEventTypeGroups(requestContext);
		List<VideoOS.EventServer.Communication.Configuration.EventType> eventTypes = AlarmDefinitionConfigurationManager.Instance.GetEventTypes(requestContext, CultureInfo.InvariantCulture);
		string name;
		string description;
		Guid eventTypeGroupId;
		string eventTypeId;
		FQID[] sourceList;
		EnableRule enableRule;
		Guid timeProfileId;
		FQID[] enableEventList;
		FQID[] disableEventList;
		TimeSpan managementTimeoutTime;
		FQID[] managementTimeoutEventList;
		FQID[] relatedCameraList;
		Guid relatedMapId;
		string owner;
		Guid priority;
		Guid category;
		FQID[] triggerEventList;
		bool autoClose;
		return ValidateItemInternal(requestContext, item, path, alarmDataSettings, eventTypeGroups, eventTypes, out name, out description, out eventTypeGroupId, out eventTypeId, out sourceList, out enableRule, out timeProfileId, out enableEventList, out disableEventList, out managementTimeoutTime, out managementTimeoutEventList, out relatedCameraList, out relatedMapId, out owner, out priority, out category, out triggerEventList, out autoClose, out mapType, out assignableToAdmins);
	}

	private ValidateResult ValidateItemInternal(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, AlarmDataSettings alarmDataSettings, List<VideoOS.EventServer.Communication.Configuration.EventTypeGroup> eventTypeGroups, List<VideoOS.EventServer.Communication.Configuration.EventType> eventTypes, out string name, out string description, out Guid eventTypeGroupId, out string eventTypeId, out FQID[] sourceList, out EnableRule enableRule, out Guid timeProfileId, out FQID[] enableEventList, out FQID[] disableEventList, out TimeSpan managementTimeoutTime, out FQID[] managementTimeoutEventList, out FQID[] relatedCameraList, out Guid relatedMapId, out string owner, out Guid priority, out Guid category, out FQID[] triggerEventList, out bool autoClose, out int mapType, out bool? assignableToAdmins)
	{
		name = null;
		description = "";
		eventTypeGroupId = Guid.Empty;
		eventTypeId = "";
		sourceList = new FQID[0];
		enableRule = EnableRule.Always;
		timeProfileId = Guid.Empty;
		enableEventList = new FQID[0];
		disableEventList = new FQID[0];
		managementTimeoutTime = TimeSpan.FromMinutes(1.0);
		managementTimeoutEventList = new FQID[0];
		relatedCameraList = new FQID[0];
		relatedMapId = Guid.Empty;
		owner = "";
		priority = Guid.Empty;
		category = Guid.Empty;
		triggerEventList = new FQID[0];
		autoClose = false;
		mapType = 0;
		assignableToAdmins = null;
		string value = "";
		string value2 = "";
		string value3 = "";
		string value4 = "";
		string value5 = "";
		string value6 = "";
		string value7 = "";
		string value8 = "";
		string value9 = "";
		string value10 = "";
		string value11 = "";
		string value12 = "";
		string value13 = "";
		string value14 = "";
		string value15 = "False";
		string value16 = "";
		string value17 = "";
		try
		{
			if (item.Properties != null)
			{
				((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Name", out name);
				((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Description", out description);
				((IEnumerable<Property>)item.Properties).TryGetPropertyValue("EventTypeGroup", out value2);
				((IEnumerable<Property>)item.Properties).TryGetPropertyValue("EventType", out value3);
				((IEnumerable<Property>)item.Properties).TryGetPropertyValue("SourceList", out value4);
				((IEnumerable<Property>)item.Properties).TryGetPropertyValue("EnableRule", out value5);
				((IEnumerable<Property>)item.Properties).TryGetPropertyValue("TimeProfile", out value14);
				((IEnumerable<Property>)item.Properties).TryGetPropertyValue("EnableEventList", out value6);
				((IEnumerable<Property>)item.Properties).TryGetPropertyValue("DisableEventList", out value7);
				((IEnumerable<Property>)item.Properties).TryGetPropertyValue("ManagementTimeoutTime", out value);
				((IEnumerable<Property>)item.Properties).TryGetPropertyValue("ManagementTimeoutEventList", out value8);
				((IEnumerable<Property>)item.Properties).TryGetPropertyValue("RelatedCameraList", out value9);
				((IEnumerable<Property>)item.Properties).TryGetPropertyValue("MapType", out value16);
				((IEnumerable<Property>)item.Properties).TryGetPropertyValue("RelatedMap", out value13);
				((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Owner", out owner);
				((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Priority", out value12);
				((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Category", out value11);
				((IEnumerable<Property>)item.Properties).TryGetPropertyValue("TriggerEventlist", out value10);
				((IEnumerable<Property>)item.Properties).TryGetPropertyValue("AutoClose", out value15);
				((IEnumerable<Property>)item.Properties).TryGetPropertyValue("AssignableToAdmins", out value17);
			}
			if (!path.IsFolder)
			{
				ThrowIfDuplicateName(requestContext, name, new Guid(path.Id));
			}
			VideoOS.EventServer.Communication.Configuration.EventTypeGroup eventTypeGroup = ValidateEventTypeGroup(value2, eventTypeGroups);
			VideoOS.EventServer.Communication.Configuration.EventType eventType = ValidateEventType(value3, eventTypes, eventTypeGroup.ID);
			string text = null;
			if (eventType == null)
			{
				eventTypeId = "";
				sourceList = new FQID[0];
				value4 = "";
				text = "EventType";
				UpdateEventTypes(item, eventTypeGroup, eventTypes);
			}
			else
			{
				UpdateSourceTypes(item, eventType);
				eventTypeId = eventType.ID.ToString();
				sourceList = ValidateSourceList(requestContext, eventType, value4);
				if (sourceList.Length == 0)
				{
					text = "SourceList";
				}
			}
			UpdateAutoClose(item, eventType, value15, out autoClose);
			ValidateEnableRules(requestContext, value5, value14, value6, value7, out enableRule, out timeProfileId, out enableEventList, out disableEventList);
			ValidateManagementTimeout(requestContext, value, value8, out managementTimeoutTime, out managementTimeoutEventList);
			relatedCameraList = ValidateRelatedList(requestContext, value9);
			mapType = ValidateMapType(value16);
			relatedMapId = ValidateMapId(value13);
			ValidateOwner(owner);
			priority = ValidatePriority(alarmDataSettings, value12);
			category = ValidateCategory(alarmDataSettings, value11);
			triggerEventList = ValidateTriggerEvent(requestContext, value10);
			if (bool.TryParse(value17, out var result))
			{
				assignableToAdmins = result;
			}
			if (text != null)
			{
				throw new ArgumentException("Incomplete field", text);
			}
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
				validateResult.ResultItem = item;
				return validateResult;
			}
			throw;
		}
	}

	public ConfigurationItem[] GetChildItems(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, path);
		_commonValidator.ValidatePath(path);
		MIPFactory.CheckAndInitialize(requestContext);
		if (path.IsFolder)
		{
			List<ConfigurationItem> list = new List<ConfigurationItem>();
			VideoOS.EventServer.Communication.Configuration.AlarmDefinition[] alarmDefinitions = AlarmDefinitionConfigurationManager.Instance.GetAlarmDefinitions(requestContext);
			if (alarmDefinitions != null)
			{
				AlarmDataSettings alarmDataSettings = AlarmDefinitionConfigurationManager.Instance.GetAlarmDataSettings(requestContext);
				List<VideoOS.EventServer.Communication.Configuration.EventTypeGroup> eventTypeGroups = AlarmDefinitionConfigurationManager.Instance.GetEventTypeGroups(requestContext);
				List<VideoOS.EventServer.Communication.Configuration.EventType> eventTypes = AlarmDefinitionConfigurationManager.Instance.GetEventTypes(requestContext, CultureInfo.InvariantCulture);
				Dictionary<string, MapUpdateData> mapList = AlarmDefinitionConfigurationManager.Instance.GetMapList(requestContext);
				VideoOS.EventServer.Communication.Configuration.AlarmDefinition[] array = alarmDefinitions;
				foreach (VideoOS.EventServer.Communication.Configuration.AlarmDefinition alarmDefinition in array)
				{
					list.Add(ConvertToAlarmDefinitionConfigurationItem(requestContext, path, alarmDefinition, alarmDataSettings, eventTypeGroups, eventTypes, mapList));
				}
			}
			return list.ToArray();
		}
		return new ConfigurationItem[0];
	}

	public ConfigurationItem GetItem(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, path);
		_commonValidator.ValidatePath(path);
		MIPFactory.CheckAndInitialize(requestContext);
		if (path.IsFolder)
		{
			return GroupConfigurationItemFactory.CreateFolderItem("AlarmDefinitionFolder", ConfigurationItemPath.CreateSystemPath());
		}
		VideoOS.EventServer.Communication.Configuration.AlarmDefinition alarmDefinition = AlarmDefinitionConfigurationManager.Instance.GetAlarmDefinition(requestContext, new Guid(path.Id));
		if (alarmDefinition == null)
		{
			throw new PathNotFoundException(path.Text);
		}
		AlarmDataSettings alarmDataSettings = AlarmDefinitionConfigurationManager.Instance.GetAlarmDataSettings(requestContext);
		List<VideoOS.EventServer.Communication.Configuration.EventTypeGroup> eventTypeGroups = AlarmDefinitionConfigurationManager.Instance.GetEventTypeGroups(requestContext);
		List<VideoOS.EventServer.Communication.Configuration.EventType> eventTypes = AlarmDefinitionConfigurationManager.Instance.GetEventTypes(requestContext, CultureInfo.InvariantCulture);
		Dictionary<string, MapUpdateData> mapList = AlarmDefinitionConfigurationManager.Instance.GetMapList(requestContext);
		ConfigurationItemPath parentPath = new ConfigurationItemPathBuilder(ConfigurationItemPath.CreateSystemPath(), "AlarmDefinitionFolder").CreatePath();
		return ConvertToAlarmDefinitionConfigurationItem(requestContext, parentPath, alarmDefinition, alarmDataSettings, eventTypeGroups, eventTypes, mapList);
	}

	public ConfigurationItem InvokeMethod(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath, string method)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, item, path);
		MIPFactory.CheckAndInitialize(requestContext);
		if (!item.MethodIds.Contains(method))
		{
			throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Invalid MethodId {0} on ItemType: {1}", method, item.ItemType), "method");
		}
		bool flag = string.Equals(item.ItemType, "InvokeInfo", StringComparison.Ordinal);
		if (!(method == "AddAlarmDefinition"))
		{
			if (method == "RemoveAlarmDefinition")
			{
				if (!flag)
				{
					return CreateInvokeInfoRemoveAlarmDefinition(item, GetChildItems(requestContext, path), method);
				}
				return RemoveAlarmDefinition(requestContext, item, path);
			}
			throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Unknown methodId: {0}", method), method);
		}
		if (!flag)
		{
			return CreateInvokeInfoAddAlarmDefinition(requestContext, item, method);
		}
		return AddAlarmDefinition(requestContext, item, path);
	}

	private static VideoOS.EventServer.Communication.Configuration.EventTypeGroup ValidateEventTypeGroup(string eventTypeGroupString, List<VideoOS.EventServer.Communication.Configuration.EventTypeGroup> eventTypeGroups)
	{
		if (!Guid.TryParse(eventTypeGroupString, out var eventTypeGroupId))
		{
			throw new ArgumentException("EventTypeGroup not valid");
		}
		return eventTypeGroups.FirstOrDefault((VideoOS.EventServer.Communication.Configuration.EventTypeGroup e) => e.ID == eventTypeGroupId) ?? throw new ArgumentException("EventTypeGroup not valid");
	}

	private static VideoOS.EventServer.Communication.Configuration.EventType ValidateEventType(string eventTypeIdString, List<VideoOS.EventServer.Communication.Configuration.EventType> eventTypes, Guid eventTypeGroupId)
	{
		if (!Guid.TryParse(eventTypeIdString, out var result))
		{
			return null;
		}
		foreach (VideoOS.EventServer.Communication.Configuration.EventType eventType in eventTypes)
		{
			if (eventType.GroupID == eventTypeGroupId && eventType.ID == result)
			{
				return eventType;
			}
		}
		return null;
	}

	private FQID[] ValidateSourceList(IGeminiRequestContext requestContext, VideoOS.EventServer.Communication.Configuration.EventType eventType, string sourceListString)
	{
		ISet<string> set = ConvertKindToItemTypes(eventType.SourceKinds, eventType.ID);
		List<Guid> list = new List<Guid>();
		if (eventType.SourceKinds != null)
		{
			list.AddRange(eventType.SourceKinds);
		}
		if (eventType.GroupID == AlarmDefinitionConstants.SystemMonitorEventsGroup)
		{
			set.Add("MIPItem");
			list.Add(Kind.LprServer);
		}
		else if (eventType.GroupID == AlarmDefinitionConstants.AccessControlCategoryEventGroup)
		{
			list.Add(Kind.AccessControlDoor);
			set.Add("AccessControlRuleGroup");
		}
		return ValidateList(requestContext, set, sourceListString, "SourceList", (list.Count == 0) ? null : list.ToArray());
	}

	private FQID[] ValidateRelatedList(IGeminiRequestContext requestContext, string listString)
	{
		HashSet<string> itemTypes = new HashSet<string> { "Camera" };
		return ValidateList(requestContext, itemTypes, listString, "RelatedCameraList");
	}

	private FQID[] ValidateTriggerEvent(IGeminiRequestContext requestContext, string listString)
	{
		HashSet<string> itemTypes = new HashSet<string> { "UserDefinedEvent", "Output" };
		return ValidateList(requestContext, itemTypes, listString, "TriggerEventlist");
	}

	private FQID[] ValidateList(IGeminiRequestContext requestContext, ISet<string> itemTypes, string listString, string propertyKey, Guid[] validKinds = null)
	{
		List<FQID> list = new List<FQID>();
		string[] array = listString.Split(',', ';');
		foreach (string pathString in array)
		{
			_commonValidator.ValidateItemListPath(pathString, itemTypes, propertyKey, out var path);
			if (path == null)
			{
				continue;
			}
			if (validKinds != null && path.ItemType == "MIPItem")
			{
				Guid id = new Guid(path.Id);
				if (id != Guid.Empty)
				{
					Item item = MIPItem.GetItem(requestContext, id);
					if (item == null)
					{
						Item item2 = null;
						if (!validKinds.Contains(Kind.LprServer) || (item2 = MIPItem.GetItems(requestContext, Kind.LprServer).FirstOrDefault((Item item3) => item3.FQID.ObjectId == id)) == null)
						{
							throw new ArgumentException("Source item not found", propertyKey);
						}
						list.Add(item2.FQID);
						continue;
					}
					if (!validKinds.Contains(item.FQID.Kind))
					{
						throw new ArgumentException("Source item has invalid type", propertyKey);
					}
				}
			}
			Guid defaultMipKind = Guid.Empty;
			if (validKinds != null && validKinds.Length == 1)
			{
				defaultMipKind = validKinds[0];
			}
			FQID fQID = _fqidConverter.ConvertPathToFQID(requestContext, path, defaultMipKind);
			if (fQID != null)
			{
				list.Add(fQID);
			}
		}
		return list.ToArray();
	}

	private void ValidateEnableRules(IGeminiRequestContext requestContext, string enableRuleString, string timeProfileIdString, string enableEventListString, string disableEventListString, out EnableRule enableRule, out Guid timeProfileId, out FQID[] enableEventList, out FQID[] disableEventList)
	{
		enableEventList = new FQID[0];
		disableEventList = new FQID[0];
		timeProfileId = Guid.Empty;
		switch (enableRuleString)
		{
		case "0":
			enableRule = EnableRule.Always;
			break;
		case "1":
		{
			enableRule = EnableRule.TimeProfile;
			ConfigurationItemPath configurationItemPath = ConfigurationItemPath.CreatePath(timeProfileIdString);
			if (TimeProfile.QueryTimeProfile(requestContext, configurationItemPath.Id) == null)
			{
				throw new ArgumentException("TimeProfile not found", "TimeProfile");
			}
			timeProfileId = new Guid(configurationItemPath.Id);
			break;
		}
		case "2":
		{
			enableRule = EnableRule.EventTriggered;
			HashSet<string> itemTypes = new HashSet<string> { "UserDefinedEvent", "InputEvent" };
			enableEventList = ValidateList(requestContext, itemTypes, enableEventListString, "EnableEventList");
			disableEventList = ValidateList(requestContext, itemTypes, disableEventListString, "DisableEventList");
			if (enableEventList.Length == 0)
			{
				throw new ArgumentException("Missing EnableEvent", "EnableEventList");
			}
			if (disableEventList.Length == 0)
			{
				throw new ArgumentException("Missing DisableEvent", "DisableEventList");
			}
			break;
		}
		default:
			throw new ArgumentException("Invalid value", "EnableRule");
		}
	}

	private void ValidateManagementTimeout(IGeminiRequestContext requestContext, string managementTimeoutTimeString, string managementTimeoutEventListString, out TimeSpan managementTimeout, out FQID[] managementTimeoutEventList)
	{
		if (string.IsNullOrEmpty(managementTimeoutEventListString))
		{
			managementTimeout = TimeSpan.FromMinutes(1.0);
			managementTimeoutEventList = new FQID[0];
			return;
		}
		if (!TimeSpan.TryParse(managementTimeoutTimeString, out managementTimeout))
		{
			throw new ArgumentException("Invalid ManagementTimeout format", "ManagementTimeoutTime");
		}
		HashSet<string> itemTypes = new HashSet<string> { "UserDefinedEvent", "Output" };
		managementTimeoutEventList = ValidateList(requestContext, itemTypes, managementTimeoutEventListString, "ManagementTimeoutEventList");
	}

	private static Guid ValidateCategory(AlarmDataSettings alarmDataSettings, string categoryGuid)
	{
		if (!Guid.TryParse(categoryGuid, out var result))
		{
			throw new ArgumentException("Not a GUID", "Category");
		}
		if (result == Guid.Empty)
		{
			return result;
		}
		AlarmCategory[] categories = alarmDataSettings.Categories;
		for (int i = 0; i < categories.Length; i++)
		{
			if (categories[i].Id == result)
			{
				return result;
			}
		}
		throw new ArgumentException("Category id not found", "Category");
	}

	private static Guid ValidatePriority(AlarmDataSettings alarmDataSettings, string priorityGuid)
	{
		if (!Guid.TryParse(priorityGuid, out var result))
		{
			throw new ArgumentException("Not a GUID", "Priority");
		}
		AlarmPriority[] priorities = alarmDataSettings.Priorities;
		for (int i = 0; i < priorities.Length; i++)
		{
			if (priorities[i].Id == result)
			{
				return result;
			}
		}
		throw new ArgumentException("Category id not found", "Priority");
	}

	private static void ValidateOwner(string owner)
	{
	}

	private static int ValidateMapType(string mapTypeString)
	{
		if (int.TryParse(mapTypeString, out var result) && (result < 0 || result > 2))
		{
			throw new ArgumentException("", "MapType");
		}
		return result;
	}

	private static Guid ValidateMapId(string mapId)
	{
		Guid result = Guid.Empty;
		if (string.IsNullOrEmpty(mapId))
		{
			return result;
		}
		if (!Guid.TryParse(mapId, out result))
		{
			throw new ArgumentException("", "RelatedMap");
		}
		return result;
	}

	private static ConfigurationItem CreateInvokeInfoAddAlarmDefinition(IGeminiRequestContext requestContext, ConfigurationItem item, string methodId)
	{
		AlarmDataSettings alarmDataSettings = AlarmDefinitionConfigurationManager.Instance.GetAlarmDataSettings(requestContext);
		if (alarmDataSettings == null)
		{
			throw new InternalConfigurationApiException("Event Server not responding");
		}
		List<VideoOS.EventServer.Communication.Configuration.EventTypeGroup> eventTypeGroups = AlarmDefinitionConfigurationManager.Instance.GetEventTypeGroups(requestContext);
		if (eventTypeGroups == null)
		{
			throw new InternalConfigurationApiException("Event Server not responding");
		}
		Dictionary<string, MapUpdateData> mapList = AlarmDefinitionConfigurationManager.Instance.GetMapList(requestContext);
		if (mapList == null)
		{
			throw new InternalConfigurationApiException("Event Server not responding");
		}
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionVertical";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.WizardAddAlarmDefinition;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		configurationItem.MethodIds = new string[1] { methodId };
		configurationItem.EnableProperty = GeneralPropertyDefinitions.GetEnabledProperty(enabled: true, uiFollowsEnabled: true);
		configurationItem.Properties = new Property[19]
		{
			GeneralPropertyDefinitions.GetNameProperty(""),
			GeneralPropertyDefinitions.GetDescriptionProperty("", isSettable: true),
			AlarmDefinitionProperties.EventTypeGroupProperty(AlarmDefinitionConstants.SystemEventsGroup, isSettable: true, eventTypeGroups),
			AlarmDefinitionProperties.EventTypeProperty("", isSettable: true, new List<VideoOS.EventServer.Communication.Configuration.EventType>()),
			AlarmDefinitionProperties.SourceListProperty("", isSettable: true, new List<string>(), allowAll: true),
			AlarmDefinitionProperties.EnableRuleProperty(EnableRule.Always, isSettable: true),
			AlarmDefinitionProperties.TimeProfileProperty("", isSettable: true),
			AlarmDefinitionProperties.EnableEventListProperty("", isSettable: true),
			AlarmDefinitionProperties.DisableEventListProperty("", isSettable: true),
			AlarmDefinitionProperties.TimeLimitProperty(TimeSpan.FromMinutes(1.0), isSettable: true),
			AlarmDefinitionProperties.ManagementTimeOutEventListProperty("", isSettable: true),
			AlarmDefinitionProperties.RelatedCameraListProperty("", isSettable: true),
			AlarmDefinitionProperties.MapTypeProperty(0, isSettable: true),
			AlarmDefinitionProperties.RelatedMapProperty("", isSettable: true, mapList),
			AlarmDefinitionProperties.OwnerProperty("", isSettable: true),
			AlarmDefinitionProperties.PriorityProperty(alarmDataSettings.Priorities[0].Id.ToString(), isSettable: true, alarmDataSettings.Priorities),
			AlarmDefinitionProperties.AlarmCategoryProperty(Guid.Empty, isSettable: true, alarmDataSettings.Categories),
			AlarmDefinitionProperties.TriggerEventListProperty("", isSettable: true),
			AlarmDefinitionProperties.AssignableToAdminsProperty(val: true, isSettable: true)
		};
		return configurationItem;
	}

	private static ConfigurationItem CreateInvokeInfoRemoveAlarmDefinition(ConfigurationItem item, ConfigurationItem[] members, string methodId)
	{
		return MethodTypesHelper.CreateInvokeMemberSelectionInfo(methodId, ConfigurationApiResources.WizardRemoveAlarmDefinition, "PropertyRemoveAlarmDefinition", ConfigurationApiResources.PropertyRemoveAlarmDefinition, item, members);
	}

	private ConfigurationItem AddAlarmDefinition(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, item, path);
		if (!string.Equals(path.ParentItemType, "System", StringComparison.Ordinal))
		{
			throw new PathInvalidException("Invalid parent. Path: " + path.Text);
		}
		MIPFactory.CheckAndInitialize(requestContext);
		int mapType = 0;
		bool? assignableToAdmins = true;
		AlarmDataSettings alarmDataSettings = AlarmDefinitionConfigurationManager.Instance.GetAlarmDataSettings(requestContext);
		List<VideoOS.EventServer.Communication.Configuration.EventTypeGroup> eventTypeGroups = AlarmDefinitionConfigurationManager.Instance.GetEventTypeGroups(requestContext);
		List<VideoOS.EventServer.Communication.Configuration.EventType> eventTypes = AlarmDefinitionConfigurationManager.Instance.GetEventTypes(requestContext, CultureInfo.InvariantCulture);
		Dictionary<string, MapUpdateData> mapList = AlarmDefinitionConfigurationManager.Instance.GetMapList(requestContext);
		string name;
		string description;
		Guid eventTypeGroupId;
		string eventTypeId;
		FQID[] sourceList;
		EnableRule enableRule;
		Guid timeProfileId;
		FQID[] enableEventList;
		FQID[] disableEventList;
		TimeSpan managementTimeoutTime;
		FQID[] managementTimeoutEventList;
		FQID[] relatedCameraList;
		Guid relatedMapId;
		string owner;
		Guid priority;
		Guid category;
		FQID[] triggerEventList;
		bool autoClose;
		ValidateResult validateResult = ValidateItemInternal(requestContext, item, path, alarmDataSettings, eventTypeGroups, eventTypes, out name, out description, out eventTypeGroupId, out eventTypeId, out sourceList, out enableRule, out timeProfileId, out enableEventList, out disableEventList, out managementTimeoutTime, out managementTimeoutEventList, out relatedCameraList, out relatedMapId, out owner, out priority, out category, out triggerEventList, out autoClose, out mapType, out assignableToAdmins);
		if (!validateResult.ValidatedOk)
		{
			string message = "";
			if (validateResult.ErrorResults != null && validateResult.ErrorResults.Length != 0)
			{
				message = validateResult.ErrorResults[0].ErrorText;
			}
			throw new PropertyInvalidException(message);
		}
		try
		{
			VideoOS.EventServer.Communication.Configuration.AlarmDefinition alarmDefinition = AlarmDefinitionConfigurationManager.Instance.CreateAlarmDefinition(requestContext, name);
			AlarmAction[] alarmActions = ((relatedMapId == Guid.Empty) ? new AlarmAction[0] : new AlarmAction[1]
			{
				new AlarmAction
				{
					Id = Guid.NewGuid(),
					ActionType = AlarmActionConstants.AlarmActionMapChange,
					Data = PackAction(relatedMapId, "Name unknown")
				}
			});
			alarmDefinition.Description = description;
			alarmDefinition.EventType = eventTypeId;
			alarmDefinition.Sources = sourceList;
			alarmDefinition.EnableRule = enableRule;
			alarmDefinition.TimeProfileId = timeProfileId;
			alarmDefinition.EnablingEvents = enableEventList;
			alarmDefinition.DisablingEvents = disableEventList;
			alarmDefinition.ManagementTimeoutTime = managementTimeoutTime;
			alarmDefinition.ManagementTimeoutEvents = managementTimeoutEventList;
			alarmDefinition.RelatedCameras = relatedCameraList;
			alarmDefinition.AlarmActions = alarmActions;
			alarmDefinition.Owner = owner;
			alarmDefinition.Priority = priority;
			alarmDefinition.Category = category;
			alarmDefinition.TriggerEvents = triggerEventList;
			alarmDefinition.AutoClose = autoClose;
			alarmDefinition.MapType = mapType;
			alarmDefinition.AssignableToAdmins = !assignableToAdmins.HasValue || assignableToAdmins.Value;
			AlarmDefinitionConfigurationManager.Instance.SaveAlarmDefinition(requestContext, alarmDefinition);
			Integration.FireConfigurationChanged(requestContext, "AlarmDefinition", alarmDefinition.Id.ToString(), EventAction.Added, alarmDefinition.Name);
			ConfigurationItem configurationItem = ConvertToAlarmDefinitionConfigurationItem(requestContext, path, alarmDefinition, alarmDataSettings, eventTypeGroups, eventTypes, mapList);
			return MethodTypesHelper.CreateInvokeResult(configurationItem, configurationItem.Path, "Result", "Success", true);
		}
		catch (Exception ex)
		{
			throw new PropertyInvalidException(ex.Message);
		}
	}

	private static ConfigurationItem RemoveAlarmDefinition(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, item, path);
		if (!string.Equals(path.ParentItemType, "System", StringComparison.Ordinal))
		{
			throw new PathInvalidException("Invalid parent. Path: " + path.Text);
		}
		ConfigurationItemPath invokeMemberItemSelectionPath = MethodTypesHelper.GetInvokeMemberItemSelectionPath(item);
		VideoOS.EventServer.Communication.Configuration.AlarmDefinition alarmDefinition = AlarmDefinitionConfigurationManager.Instance.GetAlarmDefinition(requestContext, new Guid(invokeMemberItemSelectionPath.Id));
		if (alarmDefinition == null)
		{
			throw new PathNotFoundException();
		}
		AlarmDefinitionConfigurationManager.Instance.DeleteAlarmDefinition(requestContext, new Guid(invokeMemberItemSelectionPath.Id));
		Integration.FireConfigurationChanged(requestContext, "AlarmDefinition", alarmDefinition.Id.ToString(), EventAction.Deleted, alarmDefinition.Name);
		return MethodTypesHelper.CreateInvokeResultWithoutItemPropertiesAndChildren(item);
	}

	private static void ThrowIfDuplicateName(IGeminiRequestContext requestContext, string name, Guid objectId)
	{
		Validation.CheckName(name, "Name", allowNull: false);
		VideoOS.EventServer.Communication.Configuration.AlarmDefinition[] alarmDefinitions = AlarmDefinitionConfigurationManager.Instance.GetAlarmDefinitions(requestContext);
		foreach (VideoOS.EventServer.Communication.Configuration.AlarmDefinition alarmDefinition in alarmDefinitions)
		{
			if (alarmDefinition.Id != objectId && alarmDefinition.Name == name)
			{
				throw new ArgumentException("Duplicate name");
			}
		}
	}

	private ConfigurationItem ConvertToAlarmDefinitionConfigurationItem(IGeminiRequestContext requestContext, ConfigurationItemPath parentPath, VideoOS.EventServer.Communication.Configuration.AlarmDefinition alarmDefinition, AlarmDataSettings alarmDataSettings, List<VideoOS.EventServer.Communication.Configuration.EventTypeGroup> eventTypeGroups, List<VideoOS.EventServer.Communication.Configuration.EventType> eventTypes, Dictionary<string, MapUpdateData> mapDatas)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "AlarmDefinition";
		configurationItem.UIHint = "DirectionHorizontal";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = alarmDefinition.Name;
		configurationItem.ParentPath = parentPath.Text;
		configurationItem.Path = ConfigurationItemPath.CreateItemPath(configurationItem.ItemType, alarmDefinition.Id.ToString()).Text;
		configurationItem.EnableProperty = GeneralPropertyDefinitions.GetEnabledProperty(alarmDefinition.Enabled, uiFollowsEnabled: true);
		bool isSettable = false;
		string messageName = "";
		VideoOS.EventServer.Communication.Configuration.EventType eventTypeFromEvent = GetEventTypeFromEvent(alarmDefinition.EventType, eventTypes);
		List<Property> list = new List<Property>();
		list.Add(GeneralPropertyDefinitions.GetIdPropertyGuid(alarmDefinition.Id.ToString()));
		list.Add(GeneralPropertyDefinitions.GetNameProperty(alarmDefinition.Name, isSettable: true));
		list.Add(GeneralPropertyDefinitions.GetDescriptionProperty(alarmDefinition.Description, isSettable: true));
		Guid guid = GetEventTypeGroupFromEvent(alarmDefinition.EventType, eventTypeGroups, eventTypes)?.ID ?? Guid.Empty;
		List<VideoOS.EventServer.Communication.Configuration.EventType> list2 = new List<VideoOS.EventServer.Communication.Configuration.EventType>();
		if (guid != Guid.Empty)
		{
			foreach (VideoOS.EventServer.Communication.Configuration.EventType eventType in eventTypes)
			{
				if (eventType.GroupID == guid)
				{
					list2.Add(eventType);
				}
				if ((eventType.ID.ToString() == alarmDefinition.EventType || eventType.Message == alarmDefinition.EventType) && !string.IsNullOrEmpty(eventType.AutoCloseMessage))
				{
					isSettable = true;
					messageName = eventType.AutoCloseMessage;
				}
			}
		}
		if (eventTypeFromEvent == null)
		{
			list.Add(AlarmDefinitionProperties.EventTypeGroupProperty(Guid.Empty, isSettable: true, eventTypeGroups));
			list.Add(AlarmDefinitionProperties.EventTypeProperty("", isSettable: true, new List<VideoOS.EventServer.Communication.Configuration.EventType>()));
			list.Add(AlarmDefinitionProperties.SourceListProperty(ConvertFQIDsToPaths(requestContext, alarmDefinition.Sources), isSettable: true, new List<string>(), allowAll: true));
		}
		else
		{
			list.Add(AlarmDefinitionProperties.EventTypeGroupProperty(guid, isSettable: true, eventTypeGroups));
			list.Add(AlarmDefinitionProperties.EventTypeProperty(alarmDefinition.EventType, isSettable: true, list2));
			list.Add(AlarmDefinitionProperties.SourceListProperty(ConvertFQIDsToPaths(requestContext, alarmDefinition.Sources), isSettable: true, ConvertKindToItemTypes(eventTypeFromEvent.SourceKinds, eventTypeFromEvent.ID).ToList(), allowAll: true));
		}
		list.Add(AlarmDefinitionProperties.EnableRuleProperty(alarmDefinition.EnableRule, isSettable: true));
		list.Add(AlarmDefinitionProperties.TimeProfileProperty(alarmDefinition.TimeProfileId.ToString(), isSettable: true));
		list.Add(AlarmDefinitionProperties.EnableEventListProperty(_fqidConverter.ConvertFQIDsToPaths(alarmDefinition.EnablingEvents), isSettable: true));
		list.Add(AlarmDefinitionProperties.DisableEventListProperty(_fqidConverter.ConvertFQIDsToPaths(alarmDefinition.DisablingEvents), isSettable: true));
		list.Add(AlarmDefinitionProperties.TimeLimitProperty(alarmDefinition.ManagementTimeoutTime, isSettable: true));
		list.Add(AlarmDefinitionProperties.ManagementTimeOutEventListProperty(ConvertFQIDsToPaths(requestContext, alarmDefinition.ManagementTimeoutEvents), isSettable: true));
		list.Add(AlarmDefinitionProperties.RelatedCameraListProperty(ConvertFQIDsToPaths(requestContext, alarmDefinition.RelatedCameras), isSettable: true));
		list.Add(AlarmDefinitionProperties.MapTypeProperty(alarmDefinition.MapType, isSettable: true));
		list.Add(AlarmDefinitionProperties.RelatedMapProperty(GetMapId(alarmDefinition), isSettable: true, mapDatas));
		list.Add(AlarmDefinitionProperties.OwnerProperty(alarmDefinition.Owner, isSettable: true));
		list.Add(AlarmDefinitionProperties.PriorityProperty(alarmDefinition.Priority.ToString(), isSettable: true, alarmDataSettings.Priorities));
		list.Add(AlarmDefinitionProperties.AlarmCategoryProperty(alarmDefinition.Category, isSettable: true, alarmDataSettings.Categories));
		list.Add(AlarmDefinitionProperties.TriggerEventListProperty(ConvertFQIDsToPaths(requestContext, alarmDefinition.TriggerEvents), isSettable: true));
		list.Add(AlarmDefinitionProperties.AutoCloseProperty(alarmDefinition.AutoClose, isSettable, messageName));
		list.Add(AlarmDefinitionProperties.AssignableToAdminsProperty(alarmDefinition.AssignableToAdmins, isSettable: true));
		configurationItem.Properties = list.ToArray();
		configurationItem.MethodIds = Array.Empty<string>();
		return configurationItem;
	}

	private string ConvertFQIDsToPaths(IGeminiRequestContext requestContext, FQID[] fqids)
	{
		if (fqids == null)
		{
			return "";
		}
		string text = _fqidConverter.ConvertFQIDsToPaths(fqids.Where((FQID f) => f.Kind != Kind.AccessControlDoor));
		List<string> list = new List<string>();
		foreach (FQID item in fqids.Where((FQID f) => f.Kind == Kind.AccessControlDoor))
		{
			if (item.ObjectId == Guid.Empty)
			{
				list.Add(string.Format(CultureInfo.InvariantCulture, "{0}[{1}]", "AccessControlRuleGroup", item.ObjectId));
				continue;
			}
			MIPObject mIPObject = MIPObject.QueryMIPObject(requestContext, item.ObjectId.ToString());
			if (mIPObject != null)
			{
				list.Add(ConfigurationItemBuilder.GetRuleGroupPath(mIPObject));
			}
		}
		if (list.Count == 0)
		{
			return text;
		}
		if (string.IsNullOrEmpty(text))
		{
			return string.Join(",", list);
		}
		return string.Join(",", text, string.Join(",", list));
	}

	private static void UpdateEventTypes(ConfigurationItem item, VideoOS.EventServer.Communication.Configuration.EventTypeGroup eventTypeGroup, List<VideoOS.EventServer.Communication.Configuration.EventType> eventTypes)
	{
		Guid guid = eventTypeGroup?.ID ?? Guid.Empty;
		List<VideoOS.EventServer.Communication.Configuration.EventType> list = new List<VideoOS.EventServer.Communication.Configuration.EventType>();
		if (guid != Guid.Empty)
		{
			foreach (VideoOS.EventServer.Communication.Configuration.EventType eventType in eventTypes)
			{
				if (eventType.GroupID == guid)
				{
					list.Add(eventType);
				}
			}
		}
		Property[] properties = item.Properties;
		foreach (Property property in properties)
		{
			if (!(property.Key == "EventType"))
			{
				continue;
			}
			List<ValueTypeInfo> list2 = new List<ValueTypeInfo>();
			foreach (VideoOS.EventServer.Communication.Configuration.EventType item2 in list)
			{
				list2.Add(new ValueTypeInfo
				{
					Name = item2.Message,
					Value = item2.ID.ToString()
				});
			}
			property.ValueTypeInfos = list2.ToArray();
		}
	}

	private static void UpdateSourceTypes(ConfigurationItem item, VideoOS.EventServer.Communication.Configuration.EventType eventType)
	{
		List<ValueTypeInfo> list = new List<ValueTypeInfo>();
		foreach (string item2 in ConvertKindToItemTypes(eventType.SourceKinds, eventType.ID))
		{
			list.Add(new ValueTypeInfo
			{
				Name = "PathItemType",
				Value = item2
			});
		}
		list.Add(new ValueTypeInfo
		{
			Name = "PathAllowAllItemType",
			Value = ""
		});
		Property[] properties = item.Properties;
		foreach (Property property in properties)
		{
			if (property.Key == "SourceList")
			{
				property.ValueTypeInfos = list.ToArray();
				break;
			}
		}
	}

	private static void UpdateAutoClose(ConfigurationItem item, VideoOS.EventServer.Communication.Configuration.EventType eventType, string autoCloseString, out bool autoClose)
	{
		if (!bool.TryParse(autoCloseString, out autoClose))
		{
			autoClose = false;
		}
		Property[] properties = item.Properties;
		foreach (Property property in properties)
		{
			if (!(property.Key == "AutoClose"))
			{
				continue;
			}
			if (eventType == null || string.IsNullOrEmpty(eventType.AutoCloseMessage))
			{
				property.DisplayName = string.Format(CultureInfo.CurrentCulture, ConfigurationApiResources.PropertyAlarmDefinitionAutoClose, "");
				property.Value = autoClose.ToString(CultureInfo.InvariantCulture);
				property.IsSettable = false;
				continue;
			}
			property.DisplayName = string.Format(CultureInfo.CurrentCulture, ConfigurationApiResources.PropertyAlarmDefinitionAutoClose, eventType.AutoCloseMessage);
			if (string.IsNullOrEmpty(property.Value))
			{
				property.Value = autoClose.ToString(CultureInfo.InvariantCulture);
			}
			property.IsSettable = true;
		}
	}

	private static string GetMapId(VideoOS.EventServer.Communication.Configuration.AlarmDefinition alarmDefinition)
	{
		string result = "";
		if (alarmDefinition.AlarmActions != null)
		{
			AlarmAction[] alarmActions = alarmDefinition.AlarmActions;
			foreach (AlarmAction alarmAction in alarmActions)
			{
				if (alarmAction.ActionType.Equals(AlarmActionConstants.AlarmActionMapChange))
				{
					XmlDocument xmlDocument = new XmlDocument();
					xmlDocument.LoadXml(alarmAction.Data);
					XmlElement xmlElement = xmlDocument["mapChange"];
					XmlNode xmlNode = xmlElement["serverId"];
					XmlNode xmlNode2 = xmlElement["mapId"];
					if (xmlNode != null && xmlNode.HasChildNodes && xmlNode2 != null && xmlNode2.HasChildNodes)
					{
						result = xmlNode2.InnerText;
					}
					break;
				}
			}
		}
		return result;
	}

	private static string PackAction(Guid mapId, string mapName)
	{
		using MemoryStream memoryStream = new MemoryStream();
		XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
		xmlWriterSettings.Encoding = Encoding.UTF8;
		xmlWriterSettings.OmitXmlDeclaration = true;
		using XmlWriter xmlWriter = XmlWriter.Create(memoryStream, xmlWriterSettings);
		xmlWriter.WriteStartElement("mapChange");
		xmlWriter.WriteElementString("serverId", GeminiApplicationCore.SiteManager.HomeSiteId.ToString());
		xmlWriter.WriteElementString("mapId", mapId.ToString());
		xmlWriter.WriteElementString("mapName", mapName);
		xmlWriter.WriteEndElement();
		xmlWriter.Flush();
		memoryStream.Seek(0L, SeekOrigin.Begin);
		return new StreamReader(memoryStream, Encoding.UTF8).ReadToEnd();
	}

	private static VideoOS.EventServer.Communication.Configuration.EventTypeGroup GetEventTypeGroupFromEvent(string idOrMessage, List<VideoOS.EventServer.Communication.Configuration.EventTypeGroup> eventTypeGroups, List<VideoOS.EventServer.Communication.Configuration.EventType> eventTypes)
	{
		if (idOrMessage != null)
		{
			foreach (VideoOS.EventServer.Communication.Configuration.EventTypeGroup eventTypeGroup in eventTypeGroups)
			{
				foreach (VideoOS.EventServer.Communication.Configuration.EventType eventType in eventTypes)
				{
					if (eventType.GroupID == eventTypeGroup.ID && (eventType.ID.ToString() == idOrMessage || eventType.Message == idOrMessage))
					{
						return eventTypeGroup;
					}
				}
			}
		}
		return EventTypeGroupDefault;
	}

	private static VideoOS.EventServer.Communication.Configuration.EventType GetEventTypeFromEvent(string idOrMessage, List<VideoOS.EventServer.Communication.Configuration.EventType> eventTypes)
	{
		if (idOrMessage != null)
		{
			foreach (VideoOS.EventServer.Communication.Configuration.EventType eventType in eventTypes)
			{
				if (eventType.ID.ToString() == idOrMessage || eventType.Message == idOrMessage)
				{
					return eventType;
				}
			}
		}
		return null;
	}

	private static ISet<string> ConvertKindToItemTypes(Guid[] kinds, Guid eventTypeId)
	{
		HashSet<string> hashSet = new HashSet<string>();
		if (kinds != null)
		{
			foreach (Guid guid in kinds)
			{
				string itemTypeFromKind = Factory.GetItemTypeFromKind(guid);
				if (!string.IsNullOrEmpty(itemTypeFromKind))
				{
					hashSet.Add(itemTypeFromKind);
					if (guid == Kind.Server)
					{
						hashSet.Add("RecordingServer");
					}
				}
			}
		}
		if (eventTypeId == AlarmDefinitionConstants.ExternalEventType)
		{
			hashSet.Add("GenericEvent");
		}
		return hashSet;
	}
}
