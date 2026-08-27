using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace VideoOS.Common.Integration.Definition.Schema;

[Serializable]
[GeneratedCode("xsd", "4.7.2558.0")]
[DebuggerStepThrough]
[DesignerCategory("code")]
public class hardwareType
{
	private implementationType implementationField;

	private autodetectType autodetectField;

	private eventparserType[] eventparsersField;

	private eventsType eventsField;

	private supportedeventType[] supportedeventsField;

	private capabilitiesType capabilitiesField;

	private hardwareptzsettingsType ptzsettingsField;

	private deviceType[] devicesField;

	public implementationType implementation
	{
		get
		{
			return implementationField;
		}
		set
		{
			implementationField = value;
		}
	}

	public autodetectType autodetect
	{
		get
		{
			return autodetectField;
		}
		set
		{
			autodetectField = value;
		}
	}

	[XmlArrayItem("eventparser", IsNullable = false)]
	public eventparserType[] eventparsers
	{
		get
		{
			return eventparsersField;
		}
		set
		{
			eventparsersField = value;
		}
	}

	public eventsType events
	{
		get
		{
			return eventsField;
		}
		set
		{
			eventsField = value;
		}
	}

	[XmlArrayItem("event", IsNullable = false)]
	public supportedeventType[] supportedevents
	{
		get
		{
			return supportedeventsField;
		}
		set
		{
			supportedeventsField = value;
		}
	}

	public capabilitiesType capabilities
	{
		get
		{
			return capabilitiesField;
		}
		set
		{
			capabilitiesField = value;
		}
	}

	public hardwareptzsettingsType ptzsettings
	{
		get
		{
			return ptzsettingsField;
		}
		set
		{
			ptzsettingsField = value;
		}
	}

	[XmlArrayItem("device", IsNullable = false)]
	public deviceType[] devices
	{
		get
		{
			return devicesField;
		}
		set
		{
			devicesField = value;
		}
	}

	public deviceType FindDeviceByIndex(int index)
	{
		if (devices == null)
		{
			return null;
		}
		deviceType[] array = devices;
		foreach (deviceType deviceType2 in array)
		{
			if (deviceType2.GetDeviceIndex() == index)
			{
				return deviceType2;
			}
		}
		return null;
	}

	public deviceType FindFirstDeviceByType(int type)
	{
		if (devices == null)
		{
			return null;
		}
		deviceType[] array = devices;
		foreach (deviceType deviceType2 in array)
		{
			if (deviceType2.GetDeviceType() == type)
			{
				return deviceType2;
			}
		}
		return null;
	}

	public eventparserType FindFirstEventParserByType(notificationtype type)
	{
		if (eventparsers == null)
		{
			return null;
		}
		eventparserType[] array = eventparsers;
		foreach (eventparserType eventparserType2 in array)
		{
			if (eventparserType2.notificationtype == type)
			{
				return eventparserType2;
			}
		}
		return null;
	}

	public void InitializeImplementationDetails(hardwareType sourceHardwareType)
	{
		if (sourceHardwareType.implementation != null)
		{
			implementation = sourceHardwareType.implementation.Clone();
		}
		if (sourceHardwareType.autodetect != null && autodetect != null && sourceHardwareType.autodetect.implementation != null)
		{
			autodetect.implementation = sourceHardwareType.autodetect.implementation.Clone();
		}
		if (eventparsers != null)
		{
			eventparserType[] array = eventparsers;
			foreach (eventparserType eventparserType2 in array)
			{
				eventparserType eventparserType3 = sourceHardwareType.FindFirstEventParserByType(eventparserType2.notificationtype);
				if (eventparserType3 != null && eventparserType3.implementation != null)
				{
					eventparserType2.implementation = eventparserType3.implementation.Clone();
				}
			}
		}
		if (devices == null)
		{
			return;
		}
		deviceType[] array2 = devices;
		foreach (deviceType deviceType2 in array2)
		{
			deviceType deviceType3 = sourceHardwareType.FindFirstDeviceByType(deviceType2.GetDeviceType());
			if (deviceType3 != null && deviceType3.implementation != null)
			{
				deviceType2.implementation = deviceType3.implementation.Clone();
			}
		}
	}

	public eventType FindEventByReferenceId(string referenceId)
	{
		if (events?.@event == null)
		{
			return null;
		}
		eventType[] array = events.@event;
		foreach (eventType eventType2 in array)
		{
			if (string.Compare(eventType2.referenceid, referenceId, ignoreCase: true) == 0)
			{
				return eventType2;
			}
		}
		return null;
	}

	public eventType[] FindSupportedEvents()
	{
		List<eventType> list = new List<eventType>();
		if (supportedevents != null)
		{
			supportedeventType[] array = supportedevents;
			foreach (supportedeventType supportedeventType2 in array)
			{
				eventType eventType2 = events.FindByReferenceId(supportedeventType2.eventreferenceid);
				if (eventType2 != null)
				{
					list.Add(eventType2);
				}
			}
		}
		return list.ToArray();
	}
}
