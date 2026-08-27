using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Mobile.CorporateBasicLogin;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "EventTypeGroupInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class EventTypeGroupInfo : GroupInfo
{
	[OptionalField]
	private EventTypeGroupInfo[] EventTypeGroupsField;

	[OptionalField]
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
			if (EventTypeGroupsField != value)
			{
				EventTypeGroupsField = value;
				RaisePropertyChanged("EventTypeGroups");
			}
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
			if (EventTypesField != value)
			{
				EventTypesField = value;
				RaisePropertyChanged("EventTypes");
			}
		}
	}
}
