using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Common.Proxy.Server.WCF.Net;

[DebuggerStepThrough]
[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DataContract(Name = "EventTypeGroupInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class EventTypeGroupInfo : GroupInfo, IEventTypeGroupInfo, IGroupInfo
{
	private EventTypeGroupInfo[] EventTypeGroupsField;

	private Guid[] EventTypesField;

	[DataMember]
	public EventTypeGroupInfo[] EventTypeGroups
	{
		get
		{
			return EventTypeGroupsField;
		}
		set
		{
			EventTypeGroupsField = value;
		}
	}

	[DataMember]
	public Guid[] EventTypes
	{
		get
		{
			return EventTypesField;
		}
		set
		{
			EventTypesField = value;
		}
	}

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
}
