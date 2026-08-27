using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Configuration;
using VideoOS.Common.Integration.Definition;
using VideoOS.Common.Integration.Definition.Schema;
using VideoOS.Management.Common;

namespace VideoOS.Server.Configuration;

public class DynamicEventUtility
{
	private static readonly object _lock = new object();

	public static string CheckDynamicEventIds(IServerEnvironment serverEnvironment, EventTypes allEventsTypes, string mainDefinitionXml)
	{
		lock (_lock)
		{
			MainDefinition mainDefinition = MainDefinition.LoadXml(mainDefinitionXml);
			Dictionary<Guid, Guid> replacements = new Dictionary<Guid, Guid>();
			List<VideoOS.Management.Common.EventDefinition> newEvents = new List<VideoOS.Management.Common.EventDefinition>();
			foreach (DeviceDefinition deviceDefinition in mainDefinition.HardwareDefinition.DeviceDefinitions)
			{
				DeviceTypeEnum deviceTypeEnum = (DeviceTypeEnum)deviceDefinition.Base.GetDeviceType();
				CheckForDynamicEvents(serverEnvironment, mainDefinition.HardwareDefinition, allEventsTypes, deviceDefinition.Events, deviceTypeEnum.ToString(), ref replacements, ref newEvents);
			}
			CheckForDynamicEvents(serverEnvironment, mainDefinition.HardwareDefinition, allEventsTypes, mainDefinition.HardwareDefinition.Events, "Hardware", ref replacements, ref newEvents);
			foreach (VideoOS.Common.Integration.Definition.EventDefinition @event in mainDefinition.Events)
			{
				if (@event.HasCounterEvent && replacements.ContainsKey(@event.CounterEventReferenceId))
				{
					@event.CounterEventReferenceId = replacements[@event.CounterEventReferenceId];
				}
			}
			if (mainDefinition.HardwareDefinition.Base.supportedevents != null)
			{
				supportedeventType[] supportedevents = mainDefinition.HardwareDefinition.Base.supportedevents;
				foreach (supportedeventType supportedeventType2 in supportedevents)
				{
					if (replacements.ContainsKey(new Guid(supportedeventType2.eventreferenceid)))
					{
						supportedeventType2.eventreferenceid = replacements[new Guid(supportedeventType2.eventreferenceid)].ToString();
					}
				}
			}
			foreach (DeviceDefinition deviceDefinition2 in mainDefinition.HardwareDefinition.DeviceDefinitions)
			{
				foreach (VideoOS.Common.Integration.Definition.EventDefinition event2 in deviceDefinition2.Events)
				{
					if (event2.HasCounterEvent && replacements.ContainsKey(event2.CounterEventReferenceId))
					{
						event2.CounterEventReferenceId = replacements[event2.CounterEventReferenceId];
					}
				}
				if (deviceDefinition2.Base.supportedevents == null)
				{
					continue;
				}
				supportedeventType[] supportedevents2 = deviceDefinition2.Base.supportedevents;
				foreach (supportedeventType supportedeventType3 in supportedevents2)
				{
					if (replacements.ContainsKey(new Guid(supportedeventType3.eventreferenceid)))
					{
						supportedeventType3.eventreferenceid = replacements[new Guid(supportedeventType3.eventreferenceid)].ToString();
					}
				}
			}
			if (newEvents.Count > 0)
			{
				CreateOrUpdateNewEvents(serverEnvironment, newEvents);
			}
			return mainDefinition.SaveAsXml();
		}
	}

	private static void CheckForDynamicEvents(IServerEnvironment serverEnvironment, HardwareDefinition hardwareDefinition, EventTypes allEventsTypes, EventDefinitionCollection driverEventTypes, string deviceType, ref Dictionary<Guid, Guid> replacements, ref List<VideoOS.Management.Common.EventDefinition> newEvents)
	{
		bool flag = false;
		string generatorType = ((deviceType == "Hardware") ? "DriverHardware" : "DriverDevice");
		HashSet<Guid> hashSet = new HashSet<Guid>();
		string name = hardwareDefinition.Name + "(" + deviceType + ") ";
		foreach (VideoOS.Common.Integration.Definition.EventDefinition driverEventType in driverEventTypes)
		{
			if (!driverEventType.IsDynamic)
			{
				continue;
			}
			EventType eventType2 = null;
			eventType2 = ((!DriverNumbering.IsIcpDriver(hardwareDefinition.MainDefinition.Driver.Number) && !DriverNumbering.IsMipDriver(hardwareDefinition.MainDefinition.Driver.Number)) ? allEventsTypes.GetByName(driverEventType.Base.name) : GuidLookup(hardwareDefinition, allEventsTypes, driverEventType.ReferenceId));
			if (eventType2 != null)
			{
				flag = true;
			}
			else
			{
				eventType2 = allEventsTypes.GetByDisplayName(driverEventType.Base.displayname.Value);
				Log(serverEnvironment, name, (eventType2 != null) ? "EventType found via displayName" : "EventType NOT found via displayName", eventType2);
				flag = false;
			}
			Guid id;
			if (eventType2 == null)
			{
				if (replacements.ContainsKey(driverEventType.ReferenceId))
				{
					id = replacements[driverEventType.ReferenceId];
					Guid guid = id;
					Log(serverEnvironment, name, "Id found in previous table: " + guid.ToString());
				}
				else
				{
					id = driverEventType.ReferenceId;
					newEvents.Add(new VideoOS.Management.Common.EventDefinition(id, driverEventType.Base.name, driverEventType.Base.displayname.Value, id.ToString(), "", "", generatorType, "", "", "DynamicEvent", deviceType, EventSource.FilterTypes.None, occursGlobally: false, builtIn: false, id));
					Guid guid = id;
					Log(serverEnvironment, name, "Creating/Updating EventType:" + guid.ToString() + "  " + driverEventType.Base.displayname.Value);
				}
			}
			else
			{
				id = eventType2.ID;
				VideoOS.Management.Common.EventDefinition eventDefinition = newEvents.FirstOrDefault((VideoOS.Management.Common.EventDefinition ev) => ev.ID == id);
				VideoOS.Management.Common.EventDefinition eventDefinition2 = ((eventDefinition == null) ? eventType2.Definition : eventDefinition);
				bool flag2 = false;
				EventSource[] sources = eventDefinition2.Sources;
				foreach (EventSource eventSource in sources)
				{
					if (eventSource.Type == deviceType)
					{
						flag2 = true;
						break;
					}
				}
				if (!flag2)
				{
					List<EventSource> list = new List<EventSource>(eventDefinition2.Sources);
					list.Add(new EventSource(deviceType, EventSource.FilterTypes.None));
					eventDefinition2.Sources = list.ToArray();
					if (eventDefinition == null)
					{
						newEvents.Add(eventDefinition2);
						eventDefinition = eventDefinition2;
					}
				}
				if (flag && eventDefinition2.GeneratorName == "DynamicEvent" && eventDefinition2.DisplayName != driverEventType.Base.displayname.Value)
				{
					eventDefinition2.DisplayName = driverEventType.Base.displayname.Value;
					eventType2.Definition = eventDefinition2;
					if (eventDefinition == null)
					{
						newEvents.Add(eventDefinition2);
					}
					Log(serverEnvironment, name, "DisplayName updated to: " + driverEventType.Base.displayname.Value);
				}
			}
			if (hashSet.Contains(id))
			{
				continue;
			}
			if (driverEventType.ReferenceId != id)
			{
				if (!driverEventType.HasCounterEvent && driverEventType.CounterEventReferenceId == driverEventType.ReferenceId)
				{
					driverEventType.CounterEventReferenceId = id;
				}
				replacements[driverEventType.ReferenceId] = id;
				if ((flag || driverEventType.ReferenceId == driverEventType.DisplayNameReferenceId) && eventType2 != null && Guid.TryParse(eventType2.Definition.DisplayNameReferenceId, out var result))
				{
					driverEventType.DisplayNameReferenceId = result;
				}
				driverEventType.ReferenceId = id;
			}
			hashSet.Add(id);
		}
	}

	private static EventType GuidLookup(HardwareDefinition hardwareDefinition, EventTypes allEventsTypes, Guid dynamicId)
	{
		EventType byReferenceId = allEventsTypes.GetByReferenceId(dynamicId);
		if (byReferenceId == null)
		{
			eventType eventType2 = hardwareDefinition.Base.events.FindByReferenceId(dynamicId.ToString());
			if (eventType2 != null)
			{
				string referenceid = eventType2.referenceid;
				if (Guid.TryParse(referenceid, out var result))
				{
					byReferenceId = allEventsTypes.GetByReferenceId(result);
				}
			}
		}
		return byReferenceId;
	}

	private static void CreateOrUpdateNewEvents(IServerEnvironment serverEnvironment, List<VideoOS.Management.Common.EventDefinition> newEvents)
	{
		StringBuilder stringBuilder = new StringBuilder();
		StringWriter stringWriter = new StringWriter(stringBuilder, CultureInfo.InvariantCulture);
		try
		{
			using XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
			stringWriter = null;
			xmlTextWriter.WriteStartElement("EventTypes");
			foreach (VideoOS.Management.Common.EventDefinition newEvent in newEvents)
			{
				newEvent.SaveAsXml(xmlTextWriter, includeHeader: false);
			}
			xmlTextWriter.WriteEndElement();
		}
		finally
		{
			stringWriter?.Dispose();
		}
		serverEnvironment.Server.UpdateEventTypes(stringBuilder.ToString(), serverEnvironment.Server.CreateServerServiceCallContextUserInfo());
	}

	private static void Log(IServerEnvironment serverEnvironment, string name, string text, EventType eventType = null)
	{
		if (eventType == null)
		{
			serverEnvironment.Server.Logger.LogDebug("DynamicEventUtility: {Name} - {Text}", name, text);
		}
		else
		{
			serverEnvironment.Server.Logger.LogDebug("DynamicEventUtility: {Name} - {Text} EventId: {EventId}", name, text, eventType.ID);
		}
	}
}
