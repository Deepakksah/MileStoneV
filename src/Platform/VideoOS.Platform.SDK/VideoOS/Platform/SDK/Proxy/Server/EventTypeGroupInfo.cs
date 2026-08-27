using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Platform.SDK.Proxy.Server;

[Serializable]
[GeneratedCode("wsdl", "4.0.30319.17929")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
[ExcludeFromCodeCoverage]
public class EventTypeGroupInfo : GroupInfo, IEventTypeGroupInfo, IGroupInfo
{
	private EventTypeGroupInfo[] eventTypeGroupsField;

	private Guid[] eventTypesField;

	IEventTypeGroupInfo[] IEventTypeGroupInfo.EventTypeGroups
	{
		get
		{
			return EventTypeGroups;
		}
		set
		{
			EventTypeGroups = (EventTypeGroupInfo[])value;
		}
	}

	public EventTypeGroupInfo[] EventTypeGroups
	{
		get
		{
			return eventTypeGroupsField;
		}
		set
		{
			eventTypeGroupsField = value;
		}
	}

	public Guid[] EventTypes
	{
		get
		{
			return eventTypesField;
		}
		set
		{
			eventTypesField = value;
		}
	}
}
