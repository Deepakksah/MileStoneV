using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.EventServer.Communication.Status.Client;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "DisableAlarmItem", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
public class DisableAlarmItem : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private EventType[] AvailableEventTypesField;

	private string CommentField;

	private Guid DisableAlarmItemIdField;

	private EventType[] EventTypesField;

	private Guid SourceIdField;

	private string SourceNameField;

	private DateTime TimeField;

	public ExtensionDataObject ExtensionData
	{
		get
		{
			return extensionDataField;
		}
		set
		{
			extensionDataField = value;
		}
	}

	[DataMember]
	public EventType[] AvailableEventTypes
	{
		get
		{
			return AvailableEventTypesField;
		}
		set
		{
			AvailableEventTypesField = value;
		}
	}

	[DataMember]
	public string Comment
	{
		get
		{
			return CommentField;
		}
		set
		{
			CommentField = value;
		}
	}

	[DataMember]
	public Guid DisableAlarmItemId
	{
		get
		{
			return DisableAlarmItemIdField;
		}
		set
		{
			DisableAlarmItemIdField = value;
		}
	}

	[DataMember]
	public EventType[] EventTypes
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

	[DataMember]
	public Guid SourceId
	{
		get
		{
			return SourceIdField;
		}
		set
		{
			SourceIdField = value;
		}
	}

	[DataMember]
	public string SourceName
	{
		get
		{
			return SourceNameField;
		}
		set
		{
			SourceNameField = value;
		}
	}

	[DataMember]
	public DateTime Time
	{
		get
		{
			return TimeField;
		}
		set
		{
			TimeField = value;
		}
	}
}
