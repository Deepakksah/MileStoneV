using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace ServerCommandService;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "SystemEventTypeInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class SystemEventTypeInfo : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private string DescriptionField;

	private EventSource EventSourceField;

	private Guid EventTypeIdField;

	private string NameField;

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
	public string Description
	{
		get
		{
			return DescriptionField;
		}
		set
		{
			DescriptionField = value;
		}
	}

	[DataMember]
	public EventSource EventSource
	{
		get
		{
			return EventSourceField;
		}
		set
		{
			EventSourceField = value;
		}
	}

	[DataMember]
	public Guid EventTypeId
	{
		get
		{
			return EventTypeIdField;
		}
		set
		{
			EventTypeIdField = value;
		}
	}

	[DataMember]
	public string Name
	{
		get
		{
			return NameField;
		}
		set
		{
			NameField = value;
		}
	}
}
