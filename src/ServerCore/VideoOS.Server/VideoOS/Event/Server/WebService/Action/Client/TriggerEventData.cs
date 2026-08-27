using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Event.Server.WebService.Action.Client;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "TriggerEventData", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.EventServer.WebService.Action")]
public class TriggerEventData : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private string MetadataField;

	private Guid[] RelatedDevicesField;

	private string TriggerEventNameField;

	private DateTime TriggerEventTimeField;

	private Guid TriggerEventTypeField;

	private Guid TriggerSourceField;

	private string TriggerSourceDescriptionField;

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
	public string Metadata
	{
		get
		{
			return MetadataField;
		}
		set
		{
			MetadataField = value;
		}
	}

	[DataMember]
	public Guid[] RelatedDevices
	{
		get
		{
			return RelatedDevicesField;
		}
		set
		{
			RelatedDevicesField = value;
		}
	}

	[DataMember]
	public string TriggerEventName
	{
		get
		{
			return TriggerEventNameField;
		}
		set
		{
			TriggerEventNameField = value;
		}
	}

	[DataMember]
	public DateTime TriggerEventTime
	{
		get
		{
			return TriggerEventTimeField;
		}
		set
		{
			TriggerEventTimeField = value;
		}
	}

	[DataMember]
	public Guid TriggerEventType
	{
		get
		{
			return TriggerEventTypeField;
		}
		set
		{
			TriggerEventTypeField = value;
		}
	}

	[DataMember]
	public Guid TriggerSource
	{
		get
		{
			return TriggerSourceField;
		}
		set
		{
			TriggerSourceField = value;
		}
	}

	[DataMember]
	public string TriggerSourceDescription
	{
		get
		{
			return TriggerSourceDescriptionField;
		}
		set
		{
			TriggerSourceDescriptionField = value;
		}
	}
}
