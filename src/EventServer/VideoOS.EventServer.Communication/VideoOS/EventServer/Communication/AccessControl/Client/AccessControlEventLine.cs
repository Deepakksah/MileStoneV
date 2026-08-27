using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;
using VideoOS.Platform.Data;

namespace VideoOS.EventServer.Communication.AccessControl.Client;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "AccessControlEventLine", Namespace = "http://videoos.net/2/EventServerAccessControlCommand")]
public class AccessControlEventLine : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private string CardholderNameField;

	private string EventTypeNameField;

	private AccessControlEvent FullEventField;

	private int LocalIdField;

	private string SourceNameField;

	private string SystemNameField;

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
	public string CardholderName
	{
		get
		{
			return CardholderNameField;
		}
		set
		{
			CardholderNameField = value;
		}
	}

	[DataMember]
	public string EventTypeName
	{
		get
		{
			return EventTypeNameField;
		}
		set
		{
			EventTypeNameField = value;
		}
	}

	[DataMember]
	public AccessControlEvent FullEvent
	{
		get
		{
			return FullEventField;
		}
		set
		{
			FullEventField = value;
		}
	}

	[DataMember]
	public int LocalId
	{
		get
		{
			return LocalIdField;
		}
		set
		{
			LocalIdField = value;
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
	public string SystemName
	{
		get
		{
			return SystemNameField;
		}
		set
		{
			SystemNameField = value;
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
