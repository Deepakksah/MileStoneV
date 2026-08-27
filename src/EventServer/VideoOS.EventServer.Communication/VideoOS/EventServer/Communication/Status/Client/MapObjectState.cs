using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.EventServer.Communication.Status.Client;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "MapObjectState", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Central.Server")]
public class MapObjectState : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private AlarmObjectState AlarmsField;

	private DisableObjectState DisableField;

	private string IDField;

	private Guid ServerIDField;

	private Guid StateField;

	private SuppressStatusObjectState SuppressStatusField;

	private Guid TypeField;

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
	public AlarmObjectState Alarms
	{
		get
		{
			return AlarmsField;
		}
		set
		{
			AlarmsField = value;
		}
	}

	[DataMember]
	public DisableObjectState Disable
	{
		get
		{
			return DisableField;
		}
		set
		{
			DisableField = value;
		}
	}

	[DataMember]
	public string ID
	{
		get
		{
			return IDField;
		}
		set
		{
			IDField = value;
		}
	}

	[DataMember]
	public Guid ServerID
	{
		get
		{
			return ServerIDField;
		}
		set
		{
			ServerIDField = value;
		}
	}

	[DataMember]
	public Guid State
	{
		get
		{
			return StateField;
		}
		set
		{
			StateField = value;
		}
	}

	[DataMember]
	public SuppressStatusObjectState SuppressStatus
	{
		get
		{
			return SuppressStatusField;
		}
		set
		{
			SuppressStatusField = value;
		}
	}

	[DataMember]
	public Guid Type
	{
		get
		{
			return TypeField;
		}
		set
		{
			TypeField = value;
		}
	}
}
