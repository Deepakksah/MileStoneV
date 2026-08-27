using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Platform.Data;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "EventHeader", Namespace = "urn:milestone-systems")]
public class EventHeader : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private Guid IDField;

	private DateTime TimestampField;

	private string ClassField;

	private string TypeField;

	private string VersionField = "1.0";

	private ushort PriorityField;

	private string PriorityNameField;

	private string NameField;

	private string MessageField;

	private string CustomTagField;

	private EventSource SourceField;

	private DateTime ExpireTimestampField = DateTime.MinValue;

	private Guid MessageIdField;

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

	[DataMember(IsRequired = true)]
	public Guid ID
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

	[DataMember(EmitDefaultValue = false, IsRequired = true)]
	public DateTime Timestamp
	{
		get
		{
			return TimestampField;
		}
		set
		{
			if (value.Kind == DateTimeKind.Unspecified)
			{
				value = DateTime.SpecifyKind(value, DateTimeKind.Utc);
			}
			TimestampField = value;
		}
	}

	[DataMember(EmitDefaultValue = false, Order = 2)]
	public string Class
	{
		get
		{
			return ClassField;
		}
		set
		{
			ClassField = value;
		}
	}

	[DataMember(EmitDefaultValue = false, Order = 3)]
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

	[DataMember(EmitDefaultValue = false, Order = 4)]
	public string Version
	{
		get
		{
			return VersionField;
		}
		set
		{
			VersionField = value;
		}
	}

	[DataMember(EmitDefaultValue = false, Order = 5)]
	public ushort Priority
	{
		get
		{
			return PriorityField;
		}
		set
		{
			PriorityField = value;
		}
	}

	[DataMember(EmitDefaultValue = false, Order = 6)]
	public string PriorityName
	{
		get
		{
			return PriorityNameField;
		}
		set
		{
			PriorityNameField = value;
		}
	}

	[DataMember(EmitDefaultValue = false, Order = 7)]
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

	[DataMember(EmitDefaultValue = false, Order = 8)]
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

	[DataMember(EmitDefaultValue = false, Order = 9)]
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

	[DataMember(EmitDefaultValue = false, Order = 10)]
	public EventSource Source
	{
		get
		{
			return SourceField;
		}
		set
		{
			SourceField = value;
		}
	}

	[DataMember(EmitDefaultValue = false, Order = 11)]
	public DateTime ExpireTimestamp
	{
		get
		{
			return ExpireTimestampField;
		}
		set
		{
			if (value.Kind == DateTimeKind.Unspecified)
			{
				value = DateTime.SpecifyKind(value, DateTimeKind.Utc);
			}
			ExpireTimestampField = value;
		}
	}

	[DataMember(EmitDefaultValue = false, Order = 12)]
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
}
