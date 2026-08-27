using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Platform.Proxy.Alarm;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "EventLine", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Platform.Proxy.Alarm")]
public class EventLine : IExtensibleDataObject
{
	private Guid MessageIdField;

	private ExtensionDataObject extensionDataField;

	private Guid CameraIdField;

	private string CustomTagField;

	private Guid IdField;

	private int LocalIdField;

	private string MessageField;

	private string NameField;

	private string ObjectValueField;

	private string ObjectDataField;

	private string RuleTypeField;

	private Guid SourceIdField;

	private string SourceNameField;

	private DateTime TimestampField;

	private string TypeField;

	private string VendorNameField;

	[DataMember]
	public Guid MessageId
	{
		get
		{
			return MessageIdField;
		}
		set
		{
			MessageIdField = value;
		}
	}

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
	public Guid CameraId
	{
		get
		{
			return CameraIdField;
		}
		set
		{
			CameraIdField = value;
		}
	}

	[DataMember]
	public string CustomTag
	{
		get
		{
			return CustomTagField;
		}
		set
		{
			CustomTagField = value;
		}
	}

	[DataMember]
	public Guid Id
	{
		get
		{
			return IdField;
		}
		set
		{
			IdField = value;
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
	public string Message
	{
		get
		{
			return MessageField;
		}
		set
		{
			MessageField = value;
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

	[DataMember]
	public string ObjectValue
	{
		get
		{
			return ObjectValueField;
		}
		set
		{
			ObjectValueField = value;
		}
	}

	[DataMember]
	public string RuleType
	{
		get
		{
			return RuleTypeField;
		}
		set
		{
			RuleTypeField = value;
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
	public DateTime Timestamp
	{
		get
		{
			return TimestampField;
		}
		set
		{
			TimestampField = value;
		}
	}

	[DataMember]
	public string Type
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

	[DataMember]
	public string VendorName
	{
		get
		{
			return VendorNameField;
		}
		set
		{
			VendorNameField = value;
		}
	}

	[DataMember]
	public string ObjectData
	{
		get
		{
			return ObjectDataField;
		}
		set
		{
			ObjectDataField = value;
		}
	}
}
