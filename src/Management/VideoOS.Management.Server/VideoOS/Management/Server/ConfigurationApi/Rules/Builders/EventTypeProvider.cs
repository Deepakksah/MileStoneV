using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using VideoOS.Common.Integration.Configuration;
using VideoOS.Management.Server.BusinessObjects.EventTypes;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;
using VideoOS.Management.Server.ConfigurationApi.Rules.Proxies;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Builders;

public class EventTypeProvider
{
	private readonly IEventTypeProxy _eventTypeProxy;

	private readonly Dictionary<EventGroupType, BaseEventTypeFilter> _eventTypeFilters = new Dictionary<EventGroupType, BaseEventTypeFilter>
	{
		{
			EventGroupType.DeviceConfigurable,
			new BaseEventTypeFilter(new string[1] { "DriverDevice" })
		},
		{
			EventGroupType.DevicePredefined,
			new BaseEventTypeFilter(new string[2] { "Device", "RecorderPlugin" })
		},
		{
			EventGroupType.AccessControlEvent,
			new AccessControlEventTypeFilter(new string[1] { "MIP" }, EventGroupType.AccessControlEvent)
		},
		{
			EventGroupType.AccessControlCategory,
			new AccessControlEventTypeFilter(new string[1] { "MIP" }, EventGroupType.AccessControlCategory)
		},
		{
			EventGroupType.AnalyticsEvent,
			new BaseEventTypeFilter(new string[1] { "MIPDevice" })
		},
		{
			EventGroupType.HardwarePredefined,
			new BaseEventTypeFilter(new string[1] { "Hardware" })
		},
		{
			EventGroupType.HardwareConfigurable,
			new BaseEventTypeFilter(new string[1] { "DriverHardware" })
		},
		{
			EventGroupType.RecordingServers,
			new BaseEventTypeFilter(new string[1] { "Recorder" })
		},
		{
			EventGroupType.Other,
			new BaseEventTypeFilter(new string[1] { "Server" })
		},
		{
			EventGroupType.SystemDashboardServer,
			new SystemDashboardEventTypeFilter(SystemDashboardEventGeneratorSubtypeName.Server)
		},
		{
			EventGroupType.SystemDashboardStorage,
			new SystemDashboardEventTypeFilter(SystemDashboardEventGeneratorSubtypeName.Storage)
		},
		{
			EventGroupType.SystemDashboardCamera,
			new SystemDashboardEventTypeFilter(SystemDashboardEventGeneratorSubtypeName.Device)
		},
		{
			EventGroupType.SystemDashboardDisk,
			new SystemDashboardEventTypeFilter(SystemDashboardEventGeneratorSubtypeName.Disk)
		},
		{
			EventGroupType.PredefinedEvents,
			new ExternalEventTypeFilter(EventGeneratorSubtypeName.System)
		},
		{
			EventGroupType.UserDefinedEvents,
			new ExternalEventTypeFilter(EventGeneratorSubtypeName.UserDefined)
		},
		{
			EventGroupType.GenericEvents,
			new ExternalEventTypeFilter(EventGeneratorSubtypeName.Generic)
		},
		{
			EventGroupType.None,
			new BaseEventTypeFilter(new string[0])
		}
	};

	public EventTypeProvider(IEventTypeProxy eventTypeProxy)
	{
		_eventTypeProxy = eventTypeProxy;
		_eventTypeFilters.Add(EventGroupType.PluginEvent, new PluginEventTypeFilter(eventTypeProxy));
	}

	public Dictionary<string, string> GetEventTypes(IGeminiRequestContext requestContext, EventGroupType eventGroupType, Guid eventGroupId)
	{
		if (!_eventTypeFilters.TryGetValue(eventGroupType, out var eventTypeFilter))
		{
			throw new NotSupportedException(eventGroupType.ToString());
		}
		if (!eventTypeFilter.GeneratorTypes.Any())
		{
			return new Dictionary<string, string>();
		}
		return (from et in _eventTypeProxy.GetEventTypes(requestContext, eventTypeFilter.GeneratorTypes.ToArray(), eventTypeFilter.GetIdGenerator(requestContext, eventGroupId), eventGroupId)
			where eventTypeFilter.Filter(et)
			select et).ToDictionary((EventTypeInfo et) => et.IDEventType.ToString(), (EventTypeInfo et) => GetEventTypeDisplayName(et.Definition));
	}

	private static string GetEventTypeDisplayName(string xmlDefinition)
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(xmlDefinition);
		return xmlDocument.SelectSingleNode("EventType").SelectSingleNode("DisplayName").InnerText;
	}
}
