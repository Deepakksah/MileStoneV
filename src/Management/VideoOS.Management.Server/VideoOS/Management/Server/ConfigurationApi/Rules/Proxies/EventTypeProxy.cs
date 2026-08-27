using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using VideoOS.Common.Integration.Configuration;
using VideoOS.Management.Server.BusinessObjects.EventTypes;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;
using VideoOS.Management.Server.Properties;
using VideoOS.Rules.Interfaces;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Proxies;

public class EventTypeProxy : IEventTypeProxy
{
	private const string EventGroupResourcePrefix = "PropertyRuleEventGroup";

	public const string AccessControlSourceItemKind = "6d3bdb33-d913-470f-8784-3dd4ee94db8c";

	private readonly Dictionary<string, EventGroupType> _eventGroupTypeMapping = new Dictionary<string, EventGroupType>
	{
		{
			"DriverDevice",
			EventGroupType.DeviceConfigurable
		},
		{
			"Device",
			EventGroupType.DevicePredefined
		},
		{
			"RecorderPlugin",
			EventGroupType.DevicePredefined
		},
		{
			"Hardware",
			EventGroupType.HardwarePredefined
		},
		{
			"DriverHardware",
			EventGroupType.HardwareConfigurable
		},
		{
			"Recorder",
			EventGroupType.RecordingServers
		},
		{
			"Server",
			EventGroupType.Other
		}
	};

	private readonly Dictionary<string, EventGroupType> _externalEventsEventGroupTypeMapping = new Dictionary<string, EventGroupType>
	{
		{
			EventGeneratorSubtypeName.System,
			EventGroupType.PredefinedEvents
		},
		{
			EventGeneratorSubtypeName.UserDefined,
			EventGroupType.UserDefinedEvents
		},
		{
			EventGeneratorSubtypeName.Generic,
			EventGroupType.GenericEvents
		}
	};

	private readonly Dictionary<string, EventGroupType> _systemDashboardEventsEventGroupTypeMapping = new Dictionary<string, EventGroupType>
	{
		{
			SystemDashboardEventGeneratorSubtypeName.Server,
			EventGroupType.SystemDashboardServer
		},
		{
			SystemDashboardEventGeneratorSubtypeName.Device,
			EventGroupType.SystemDashboardCamera
		},
		{
			SystemDashboardEventGeneratorSubtypeName.Disk,
			EventGroupType.SystemDashboardDisk
		},
		{
			SystemDashboardEventGeneratorSubtypeName.Storage,
			EventGroupType.SystemDashboardStorage
		}
	};

	private static readonly IDictionary<string, string[]> _eventToEventSourceItemTypes = new Dictionary<string, string[]>
	{
		{
			"6d3bdb33-d913-470f-8784-3dd4ee94db8c",
			new string[3] { "AccessControlSystem", "AccessControlUnit", "AccessControlRuleGroup" }
		},
		{
			"Camera",
			new string[2] { "Camera", "CameraGroup" }
		},
		{
			"AudioInput",
			new string[2] { "Microphone", "MicrophoneGroup" }
		},
		{
			"AudioOutput",
			new string[2] { "Speaker", "SpeakerGroup" }
		},
		{
			"Metadata",
			new string[2] { "Metadata", "MetadataGroup" }
		},
		{
			"Input",
			new string[2] { "InputEvent", "InputEventGroup" }
		},
		{
			"Output",
			new string[2] { "Output", "OutputGroup" }
		},
		{
			"Hardware",
			new string[1] { "Hardware" }
		},
		{
			"Recorder",
			new string[1] { "RecordingServer" }
		},
		{
			"Database",
			new string[1] { "RecordingServer" }
		},
		{
			"External",
			new string[1] { "External" }
		},
		{
			"Server",
			new string[1] { "System" }
		},
		{
			"GenericServer",
			new string[2] { "System", "RecordingServer" }
		}
	};

	private static EventGroupType[] HiddenEventGroupTypes = new EventGroupType[4]
	{
		EventGroupType.None,
		EventGroupType.NotSupported,
		EventGroupType.PluginEvent,
		EventGroupType.AnalyticsEvent
	};

	public static string[] GetItemTypesFromEventSource(string surceType)
	{
		if (_eventToEventSourceItemTypes.TryGetValue(surceType, out var value))
		{
			return value;
		}
		return new string[0];
	}

	public static Dictionary<string, string> GetBuildInEventGroups()
	{
		return (from EventGroupType e in Enum.GetValues(typeof(EventGroupType))
			where !HiddenEventGroupTypes.Contains(e)
			select e).ToDictionary((EventGroupType e) => e.ToString(), (EventGroupType e) => ConfigurationApiResources.ResourceManager.GetString("PropertyRuleEventGroup" + e));
	}

	public Dictionary<string, string> GetEventGroups(IGeminiRequestContext requestContext)
	{
		Dictionary<string, string> buildInEventGroups = GetBuildInEventGroups();
		foreach (EventTypeInfo item in from et in GetEventTypes(requestContext, new string[2] { "MIP", "MIPDevice" }, null, Guid.Empty)
			where et.IDGenerator != SourceGuid.AccessControl
			select et)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(item.Definition);
			XmlNode xmlNode = xmlDocument.SelectSingleNode(".//Generator");
			string innerText = xmlNode.SelectSingleNode(".//GroupId").InnerText;
			string innerText2 = xmlNode.SelectSingleNode(".//GroupName").InnerText;
			if (!string.IsNullOrEmpty(innerText) && !buildInEventGroups.ContainsKey(innerText))
			{
				buildInEventGroups.Add(innerText, innerText2);
			}
		}
		return buildInEventGroups;
	}

	public Guid GetPluginEventTypeIdGenerator(IGeminiRequestContext requestContext, Guid eventGroupId)
	{
		foreach (EventTypeInfo item in from et in GetEventTypes(requestContext, new string[2] { "MIP", "MIPDevice" }, null, Guid.Empty)
			where et.IDGenerator != SourceGuid.AccessControl
			select et)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(item.Definition);
			string innerText = xmlDocument.SelectSingleNode(".//Generator").SelectSingleNode(".//GroupId").InnerText;
			if (!string.IsNullOrEmpty(innerText) && new Guid(innerText) == eventGroupId)
			{
				return item.IDGenerator;
			}
		}
		return Guid.Empty;
	}

	public EventTypeInfo GetEventType(IGeminiRequestContext requestContext, Guid eventTypeId)
	{
		return VideoOS.Management.Server.BusinessObjects.EventTypes.EventType.GetEventType(requestContext, eventTypeId);
	}

	public IEnumerable<EventTypeInfo> GetEventTypes(IGeminiRequestContext requestContext, string[] generatorTypes, string idGenerator, Guid eventGroupId)
	{
		return from et in VideoOS.Management.Server.BusinessObjects.EventTypes.EventType.GetEventTypes(requestContext, generatorTypes, idGenerator)
			where eventGroupId == Guid.Empty || GetMipEventGroupIdFromDefinition(et.Definition) == eventGroupId
			select et;
	}

	public EventGroupType GetEventGroupType(IGeminiRequestContext requestContext, Guid eventType)
	{
		EventTypeInfo eventType2 = GetEventType(requestContext, eventType);
		if (eventType2 == null)
		{
			return EventGroupType.None;
		}
		if (eventType2.GeneratorType == "MIP" || eventType2.GeneratorType == "MIPDevice")
		{
			return GetMipEventGroupTypeFromDefinition(eventType2.Definition);
		}
		if (eventType2.GeneratorType == "External")
		{
			return _externalEventsEventGroupTypeMapping[eventType2.GeneratorSubType];
		}
		if (eventType2.GeneratorType == "SystemDashboard")
		{
			return _systemDashboardEventsEventGroupTypeMapping[eventType2.GeneratorSubType];
		}
		if (_eventGroupTypeMapping.ContainsKey(eventType2.GeneratorType))
		{
			return _eventGroupTypeMapping[eventType2.GeneratorType];
		}
		return EventGroupType.NotSupported;
	}

	public Guid GetPluginEventGroupId(IGeminiRequestContext requestContext, Guid eventType)
	{
		EventTypeInfo eventType2 = GetEventType(requestContext, eventType);
		if (eventType2 == null || (eventType2.GeneratorType != "MIP" && eventType2.GeneratorType != "MIPDevice"))
		{
			return Guid.Empty;
		}
		return GetMipEventGroupIdFromDefinition(eventType2.Definition);
	}

	public EventSourceData[] GetEventSourceData(IGeminiRequestContext requestContext, Guid eventType)
	{
		EventTypeInfo eventType2 = GetEventType(requestContext, eventType);
		if (eventType2 == null)
		{
			return new EventSourceData[0];
		}
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(eventType2.Definition);
		return (from r in xmlDocument.SelectNodes("/EventType/Sources/Source").Cast<XmlNode>().Select(ReadSourceTypeData)
			where r != null
			select r).ToArray();
	}

	public string[] GetEventSourceItemTypes(IGeminiRequestContext requestContext, Guid eventType)
	{
		return GetEventSourceData(requestContext, eventType).SelectMany((EventSourceData s) => s.ItemTypes).Distinct().ToArray();
	}

	public EventGroupType GetHandlerEventGroupType(IGeminiRequestContext requestContext, EventGroupType eventGroupType, Guid eventType)
	{
		EventGroupType result = eventGroupType;
		if (eventGroupType == EventGroupType.PluginEvent && GetEventType(requestContext, eventType).GeneratorType == "MIPDevice")
		{
			result = EventGroupType.AnalyticsEvent;
		}
		return result;
	}

	private EventSourceData ReadSourceTypeData(XmlNode sourceNode)
	{
		string text = sourceNode["Type"]?.InnerText;
		if (string.IsNullOrEmpty(text))
		{
			return null;
		}
		string pluginType = string.Empty;
		if (!_eventToEventSourceItemTypes.TryGetValue(text, out var value))
		{
			if (!Guid.TryParse(text, out var _))
			{
				return null;
			}
			value = new string[2] { "MIPItem", "MIPKind" };
			pluginType = text;
		}
		return new EventSourceData
		{
			ItemTypes = value,
			Filters = (from XmlNode n in sourceNode.SelectNodes("./Filters/Filter/@type")
				select n.InnerText.Trim()).ToArray(),
			PluginType = pluginType
		};
	}

	public static EventGroupType GetMipEventGroupTypeFromDefinition(string xmlDefinition)
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(xmlDefinition);
		XmlNode xmlNode = xmlDocument.SelectSingleNode(".//Generator");
		string innerText = xmlNode.SelectSingleNode(".//Id").InnerText;
		string innerText2 = xmlNode.SelectSingleNode(".//GroupId").InnerText;
		if (!string.IsNullOrEmpty(innerText) && !string.IsNullOrEmpty(innerText2))
		{
			Guid g = new Guid(innerText);
			Guid g2 = new Guid(innerText2);
			if (!SourceGuid.AccessControl.Equals(g))
			{
				return EventGroupType.PluginEvent;
			}
			if (AlarmDefinitionConstants.AccessControlCategoryEventGroup.Equals(g2))
			{
				return EventGroupType.AccessControlCategory;
			}
			if (AlarmDefinitionConstants.AccessControlEventGroup.Equals(g2))
			{
				return EventGroupType.AccessControlEvent;
			}
		}
		return EventGroupType.NotSupported;
	}

	private static Guid GetMipEventGroupIdFromDefinition(string xmlDefinition)
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(xmlDefinition);
		return new Guid(xmlDocument.SelectSingleNode(".//Generator").SelectSingleNode(".//GroupId").InnerText);
	}
}
