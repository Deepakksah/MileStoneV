using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Definition;
using VideoOS.Common.Integration.Definition.Schema;
using VideoOS.Management.Common;
using VideoOS.Management.Server.ConfigurationApi;

namespace VideoOS.Management.Server;

public class DeviceEvents
{
	public string Id { get; set; }

	public int DeviceIndex { get; set; }

	public DateTime LastModified { get; set; }

	public string HardwareId { get; set; }

	public string HardwareDefinitionDigest { get; set; }

	public string EventSettings { get; set; }

	public string HardwareDefinition { get; set; }

	public static IEnumerable<DeviceEvent> QueryDeviceEvents(IGeminiRequestContext requestContext, ConfigurationItemPath path, out DateTime lastModified)
	{
		if (requestContext == null)
		{
			throw new ArgumentNullException("requestContext");
		}
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		string objectId = path.Id;
		Validation.CheckObjectId(ref objectId, "deviceId", allowNullOrEmpty: false);
		Guid namespaceId;
		try
		{
			namespaceId = requestContext.DeviceTypeLookup.QueryDeviceSecurityNamespaceId(requestContext, objectId);
		}
		catch (UnknownDeviceException)
		{
			throw new DeviceNotFoundException();
		}
		catch (DatabaseConnectionException)
		{
			throw;
		}
		requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, namespaceId, "GENERIC_READ", objectId);
		using IDeviceComponent deviceComponent = requestContext.CreateDeviceComponent();
		DeviceEvents deviceEvents = deviceComponent.QueryDeviceEvents(objectId);
		if (deviceEvents == null)
		{
			throw new DeviceNotFoundException();
		}
		lastModified = deviceEvents.LastModified;
		DeviceHwid deviceHwid = GetDeviceHwid(deviceComponent.QueryDeviceById(objectId));
		return ExtractEventsForSpecificDevice(GetEventDefinitions(deviceEvents.HardwareDefinition, deviceHwid), deviceEvents.EventSettings, objectId, deviceHwid);
	}

	public static IEnumerable<DeviceEvent> QueryHardwareEvents(IGeminiRequestContext requestContext, ConfigurationItemPath path, out DateTime lastModified)
	{
		string objectId = path.Id;
		Validation.CheckObjectId(ref objectId, "hardwareId", allowNullOrEmpty: false);
		requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.Hardware, "GENERIC_READ", objectId);
		using IHardwareComponent hardwareComponent = requestContext.CreateHardwareComponent();
		DeviceEvents deviceEvents = hardwareComponent.QueryHardwareEvents(objectId);
		if (deviceEvents == null)
		{
			throw new DeviceNotFoundException();
		}
		lastModified = deviceEvents.LastModified;
		return ExtractEventsForSpecificHardware(GetHardwareEventDefinitions(deviceEvents.HardwareDefinition), deviceEvents.EventSettings, objectId);
	}

	public static IEnumerable<DeviceEvent> UpdateDeviceEvents(IGeminiRequestContext requestContext, string hardwareDeviceId, IEnumerable<DeviceEvent> deviceEventConfigurations, DateTime? lastModified)
	{
		Validation.CheckObjectId(ref hardwareDeviceId, "hardwareDeviceId", allowNullOrEmpty: false);
		bool flag = IsDeviceHardware(requestContext, hardwareDeviceId);
		if (flag)
		{
			requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.Hardware, "GENERIC_WRITE", hardwareDeviceId);
		}
		else
		{
			Guid namespaceId;
			try
			{
				namespaceId = requestContext.DeviceTypeLookup.QueryDeviceSecurityNamespaceId(requestContext, hardwareDeviceId);
			}
			catch (UnknownDeviceException)
			{
				throw new DeviceNotFoundException();
			}
			requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, namespaceId, "GENERIC_WRITE", hardwareDeviceId);
		}
		DeviceEvents deviceEvents;
		if (flag)
		{
			using IHardwareComponent hardwareComponent = requestContext.CreateHardwareComponent();
			deviceEvents = hardwareComponent.QueryHardwareEvents(hardwareDeviceId);
			if (deviceEvents == null)
			{
				throw new DeviceNotFoundException();
			}
		}
		else
		{
			using IDeviceComponent deviceComponent = requestContext.CreateDeviceComponent();
			deviceEvents = deviceComponent.QueryDeviceEvents(hardwareDeviceId);
			if (deviceEvents == null)
			{
				throw new DeviceNotFoundException();
			}
		}
		if (!lastModified.HasValue)
		{
			lastModified = deviceEvents.LastModified;
		}
		using IDeviceComponent deviceComponent2 = requestContext.CreateDeviceComponent();
		DriverSettings driverSettings = new DriverSettings();
		string eventSettings = deviceEvents.EventSettings;
		driverSettings.LoadXml(eventSettings, SettingsLoadMethod.Reload);
		DriverSettings driverSettings2;
		if (flag)
		{
			HardwareHwid hardwareHwid = new HardwareHwid("hardware");
			EventDefinitionCollection hardwareEventDefinitions = GetHardwareEventDefinitions(deviceEvents.HardwareDefinition);
			DriverSettings existingEventSettings = FilterEventsExceptDeviceHwid(driverSettings, hardwareHwid);
			driverSettings2 = UpdateEventSettings(deviceEventConfigurations, existingEventSettings, hardwareEventDefinitions, hardwareDeviceId, hardwareHwid.Text, isHardware: true);
		}
		else
		{
			DeviceHwid deviceHwid = GetDeviceHwid(deviceComponent2.QueryDeviceById(hardwareDeviceId));
			EventDefinitionCollection hardwareEventDefinitions = GetEventDefinitions(deviceEvents.HardwareDefinition, deviceHwid);
			DriverSettings existingEventSettings = FilterEventsExceptDeviceHwid(driverSettings, deviceHwid);
			driverSettings2 = UpdateEventSettings(deviceEventConfigurations, existingEventSettings, hardwareEventDefinitions, hardwareDeviceId, deviceHwid.Text, isHardware: false);
		}
		string eventsXml = driverSettings2.SaveAsXml();
		Guid sessionId = Guid.NewGuid();
		deviceComponent2.UpdateEvents(deviceEvents.HardwareId, lastModified.Value, eventsXml, sessionId);
		return deviceEventConfigurations;
	}

	public static bool EventExists(IGeminiRequestContext requestContext, string id)
	{
		Validation.CheckObjectId(ref id, "id", allowNullOrEmpty: false);
		return requestContext != null;
	}

	public static void UpdateDeviceEventValidation(IGeminiRequestContext requestContext, string id, string name, string description, bool? eventUsed)
	{
		Validation.CheckObjectId(ref id, "Id", allowNullOrEmpty: false);
		Validation.CheckName(name, "Name", allowNull: true);
		Validation.CheckDescription(ref description, "Description", allowNull: true);
		Validation.CheckBooleanValue(eventUsed, "EventUsed", allowNull: false);
		requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.Hardware, "GENERIC_WRITE", id);
		requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.Camera, "GENERIC_WRITE", id);
	}

	public static bool IsDeviceHardware(IGeminiRequestContext requestContext, string hardwareDeviceId)
	{
		using IHardwareComponent hardwareComponent = requestContext.CreateHardwareComponent();
		return hardwareComponent.QueryHardwareById(hardwareDeviceId) != null;
	}

	public static DeviceHwid GetDeviceHwid(Device device)
	{
		return new DeviceHwid((DeviceTypeEnum)device.DeviceType, device.DeviceIndex);
	}

	public static DriverSettings FilterEventsByHwid(DriverSettings eventSettings, Hwid hwid)
	{
		if (eventSettings == null)
		{
			throw new ArgumentNullException("eventSettings");
		}
		if (hwid == null)
		{
			throw new ArgumentNullException("hwid");
		}
		IEnumerable<string> enumerable = from se in eventSettings.AllEntries
			where se.Name == "EventSource" && se.Value == hwid.Text
			select se.Section;
		DriverSettings driverSettings = new DriverSettings();
		foreach (string item in enumerable)
		{
			if (Regex.Match(item, "^(?<EventHwid>event:\\d+)\\|(?<EventIndex>\\d+)$").Success)
			{
				string[] sectionSettingNames = eventSettings.GetSectionSettingNames(item);
				foreach (string name in sectionSettingNames)
				{
					string value = eventSettings.Get(item, name);
					string typeId = eventSettings.GetTypeId(item, name);
					driverSettings.CreateOrUpdate(item, name, value, typeId);
				}
			}
		}
		return driverSettings;
	}

	public static int GetFirstFreeIndex(DriverSettings allConfiguredEvents)
	{
		List<int> list = new List<int>();
		string[] sectionNames = allConfiguredEvents.GetSectionNames();
		for (int i = 0; i < sectionNames.Length; i++)
		{
			Match match = Regex.Match(sectionNames[i], "^(?<EventHwid>event:\\d+)\\|(?<EventIndex>\\d+)$");
			if (match.Success)
			{
				int item = int.Parse(match.Groups["EventIndex"].Value, CultureInfo.InvariantCulture);
				list.Add(item);
			}
		}
		for (int j = 0; j < int.MaxValue; j++)
		{
			if (!list.Contains(j))
			{
				return j;
			}
		}
		throw new ArgumentOutOfRangeException("allConfiguredEvents", "Could not find a free index entry.");
	}

	private static EventDefinitionCollection GetEventDefinitions(string hardwareDefinitionXml, DeviceHwid deviceHwid)
	{
		return new VideoOS.Common.Integration.Definition.HardwareDefinition(MainDefinition.LoadXml(hardwareDefinitionXml)).DeviceDefinitions.FirstOrDefault((DeviceDefinition dev) => dev.Hwid.Equals(deviceHwid))?.Events;
	}

	private static EventDefinitionCollection GetHardwareEventDefinitions(string hardwareDefinitionXml)
	{
		return new VideoOS.Common.Integration.Definition.HardwareDefinition(MainDefinition.LoadXml(hardwareDefinitionXml)).Events;
	}

	private static IEnumerable<DeviceEvent> ExtractEventsForSpecificDevice(EventDefinitionCollection deviceEventDefinitions, string deviceEventSettingsXml, string deviceId, DeviceHwid deviceHwid)
	{
		List<DeviceEvent> list = new List<DeviceEvent>();
		foreach (VideoOS.Common.Integration.Definition.EventDefinition deviceEventDefinition in deviceEventDefinitions)
		{
			list.Add(new DeviceEvent
			{
				DeviceId = deviceId,
				EventName = deviceEventDefinition.DisplayNameResolved,
				EventId = deviceEventDefinition.ReferenceId.ToString(),
				EventIndexDisplayName = deviceEventDefinition.Capabilities.FirstOrDefault((CapabilityDefinition x) => x.Name == "EventIndex")?.TypeDefinition.DisplayNameResolved,
				Hwid = deviceEventDefinition.Hwid
			});
		}
		DriverSettings driverSettings = new DriverSettings();
		driverSettings.LoadXml(deviceEventSettingsXml, SettingsLoadMethod.Reload);
		DriverSettings driverSettings2 = FilterEventsByHwid(driverSettings, deviceHwid);
		string[] sectionNames = driverSettings2.GetSectionNames();
		foreach (string text in sectionNames)
		{
			EventHwid eventHwid = new EventHwid(text);
			DeviceEvent deviceEvent = list.Find((DeviceEvent e) => e.Hwid.Equals(eventHwid));
			if (deviceEvent != null)
			{
				deviceEvent.IsUsed = true;
				deviceEvent.IsEnabled = string.Equals(driverSettings2.Get(text, "EventEnabled"), "True", StringComparison.OrdinalIgnoreCase);
				deviceEvent.EventIndex = driverSettings2.Get(text, "EventIndex");
				deviceEvent.DriverEventId = driverSettings2.Get(text, "DriverEventId");
			}
		}
		return list;
	}

	private static IEnumerable<DeviceEvent> ExtractEventsForSpecificHardware(EventDefinitionCollection hardwareEventDefinitions, string hardwareEventsEventSettings, string hardwareId)
	{
		List<DeviceEvent> list = new List<DeviceEvent>();
		foreach (VideoOS.Common.Integration.Definition.EventDefinition hardwareEventDefinition in hardwareEventDefinitions)
		{
			DeviceEvent item = new DeviceEvent
			{
				DeviceId = hardwareId,
				EventName = hardwareEventDefinition.DisplayNameResolved,
				EventId = hardwareEventDefinition.ReferenceId.ToString(),
				EventIndexDisplayName = hardwareEventDefinition.Capabilities.FirstOrDefault((CapabilityDefinition x) => x.Name == "EventIndex")?.TypeDefinition.DisplayNameResolved,
				Hwid = hardwareEventDefinition.Hwid
			};
			list.Add(item);
		}
		DriverSettings driverSettings = new DriverSettings();
		driverSettings.LoadXml(hardwareEventsEventSettings, SettingsLoadMethod.Reload);
		HardwareHwid hwid = new HardwareHwid("hardware");
		DriverSettings driverSettings2 = FilterEventsByHwid(driverSettings, hwid);
		string[] sectionNames = driverSettings2.GetSectionNames();
		foreach (string text in sectionNames)
		{
			EventHwid eventHwid = new EventHwid(text);
			DeviceEvent deviceEvent = list.Find((DeviceEvent e) => e.Hwid.Equals(eventHwid));
			if (deviceEvent != null)
			{
				deviceEvent.IsUsed = true;
				deviceEvent.IsEnabled = string.Equals(driverSettings2.Get(text, "EventEnabled"), "True", StringComparison.OrdinalIgnoreCase);
				deviceEvent.EventIndex = driverSettings2.Get(text, "EventIndex");
				deviceEvent.DriverEventId = driverSettings2.Get(text, "DriverEventId");
			}
		}
		return list;
	}

	private static DriverSettings FilterEventsExceptDeviceHwid(DriverSettings eventSettings, Hwid deviceHwid)
	{
		if (eventSettings == null)
		{
			throw new ArgumentNullException("eventSettings");
		}
		if (deviceHwid == null)
		{
			throw new ArgumentNullException("deviceHwid");
		}
		IEnumerable<string> enumerable = from se in eventSettings.AllEntries
			where se.Name == "EventSource" && se.Value != deviceHwid.Text
			select se.Section;
		DriverSettings driverSettings = new DriverSettings();
		foreach (string item in enumerable)
		{
			if (Regex.Match(item, "^(?<EventHwid>event:\\d+)\\|(?<EventIndex>\\d+)$").Success)
			{
				string[] sectionSettingNames = eventSettings.GetSectionSettingNames(item);
				foreach (string name in sectionSettingNames)
				{
					string value = eventSettings.Get(item, name);
					string typeId = eventSettings.GetTypeId(item, name);
					driverSettings.CreateOrUpdate(item, name, value, typeId);
				}
			}
		}
		return driverSettings;
	}

	private static DriverSettings UpdateEventSettings(IEnumerable<DeviceEvent> eventConfigurations, DriverSettings existingEventSettings, EventDefinitionCollection eventDefinitions, string deviceId, string hwid, bool isHardware)
	{
		foreach (DeviceEvent eventConfig in eventConfigurations)
		{
			if (!eventConfig.IsUsed)
			{
				continue;
			}
			VideoOS.Common.Integration.Definition.EventDefinition eventDefinition = eventDefinitions.FirstOrDefault((VideoOS.Common.Integration.Definition.EventDefinition x) => x.ReferenceId == Guid.Parse(eventConfig.EventId));
			if (eventDefinition == null)
			{
				continue;
			}
			string text = eventDefinition.Hwid.Text;
			string section = text + "|" + GetFirstFreeIndex(existingEventSettings).ToString(CultureInfo.InvariantCulture);
			CapabilityDefinition capabilityDefinition = eventDefinition.Capabilities.FindByTypeReferenceId(TypeReferenceIds.EventUserData);
			CapabilityDefinition capabilityDefinition2 = eventDefinition.Capabilities.FindByTypeReferenceId(TypeReferenceIds.EventSource);
			CapabilityDefinition capabilityDefinition3 = eventDefinition.Capabilities.FindByTypeReferenceId(TypeReferenceIds.EventEnabled);
			foreach (CapabilityDefinition capability in eventDefinition.Capabilities)
			{
				if (capability.Name == capabilityDefinition.Name)
				{
					string value = (isHardware ? WriteEventUserDataXml("Hardware", deviceId, eventDefinition.ReferenceId.ToString()) : WriteEventUserDataXml("Device", deviceId, eventDefinition.ReferenceId.ToString()));
					existingEventSettings.CreateOrUpdate(section, capability.Name, value, TypeReferenceIds.EventUserData.ToString());
					continue;
				}
				if (capability.Name == capabilityDefinition2.Name)
				{
					existingEventSettings.CreateOrUpdate(section, capability.Name, hwid, capability.TypeDefinition.ReferenceId.ToString());
					continue;
				}
				if (capability.Name == capabilityDefinition3.Name)
				{
					string value2 = eventConfig.IsEnabled.ToString();
					existingEventSettings.CreateOrUpdate(section, capability.Name, value2, capability.TypeDefinition.ReferenceId.ToString());
					continue;
				}
				string text2 = null;
				if (capability.Name == "EventIndex")
				{
					text2 = eventConfig.EventIndex;
				}
				if (capability.Name == "DriverEventId")
				{
					text2 = (string.IsNullOrEmpty(eventConfig.DriverEventId) ? GetDefaultSettingValue("DriverEventId", eventDefinition, text) : eventConfig.DriverEventId);
				}
				if (text2 != null)
				{
					existingEventSettings.CreateOrUpdate(section, capability.Name, text2, capability.TypeDefinition.ReferenceId.ToString());
					continue;
				}
				TypeContent activeTypeContent = capability.TypeDefinition.GetActiveTypeContent();
				if (activeTypeContent != null)
				{
					if (activeTypeContent.IsData)
					{
						existingEventSettings.CreateOrUpdate(section, capability.Name, activeTypeContent.AsData.Base.defaultvalue, capability.TypeDefinition.ReferenceId.ToString());
					}
					else if (activeTypeContent.IsEnumeration)
					{
						existingEventSettings.CreateOrUpdate(section, capability.Name, activeTypeContent.AsEnumeration.Base.defaultvalue, capability.TypeDefinition.ReferenceId.ToString());
					}
				}
			}
		}
		return existingEventSettings;
	}

	private static string WriteEventUserDataXml(string sourceType, string sourceId, string eventId)
	{
		StringBuilder stringBuilder = new StringBuilder(512);
		StringWriter stringWriter = null;
		try
		{
			stringWriter = new StringWriter(stringBuilder, CultureInfo.InvariantCulture);
			using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter))
			{
				stringWriter = null;
				xmlWriter.WriteProcessingInstruction("xml", "version=\"1.0\" encoding=\"UTF-16\"");
				xmlWriter.WriteStartElement("data");
				xmlWriter.WriteElementString("sourcetype", sourceType);
				xmlWriter.WriteElementString("sourceid", sourceId);
				xmlWriter.WriteElementString("eventid", eventId);
				xmlWriter.WriteEndElement();
			}
			return stringBuilder.ToString();
		}
		finally
		{
			stringWriter?.Dispose();
		}
	}

	private static string GetDefaultSettingValue(string settingName, VideoOS.Common.Integration.Definition.EventDefinition eventDefinition, string eventHwid)
	{
		foreach (settingsType item in eventDefinition.MainDefinition.Base.properties.Where((settingsType p) => p.hwid == eventHwid))
		{
			settingType[] setting = item.setting;
			foreach (settingType settingType2 in setting)
			{
				if (settingType2.name == settingName)
				{
					return settingType2.value;
				}
			}
		}
		return string.Empty;
	}
}
