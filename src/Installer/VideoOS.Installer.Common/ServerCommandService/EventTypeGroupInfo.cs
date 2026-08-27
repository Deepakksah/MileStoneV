using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace ServerCommandService;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "EventTypeGroupInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class EventTypeGroupInfo : GroupInfo
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
}
