using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Platform.Proxy.Alarm;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "AlarmLine", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Platform.Proxy.Alarm")]
public class AlarmLine : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private string AssignedToField;

	private Guid CameraIdField;

	private string CustomTagField;

	private string DescriptionField;

	private Guid IdField;

	private int LocalIdField;

	private string LocationField;

	private string MessageField;

	private string NameField;

	private string ObjectValueField;

	private ushort PriorityField;

	private string PriorityNameField;

	private string RuleTypeField;

	private Guid SourceIdField;

	private string SourceNameField;

	private ushort StateField;

	private string StateNameField;

	private ushort CategoryField;

	private string CategoryNameField;

	private DateTime TimestampField;

	private string TypeField;

	private string VendorNameField;

	private DateTime ModifiedField;

	private bool FilterMatchField;

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
	public string AssignedTo
	{
		get
		{
			return AssignedToField;
		}
		set
		{
			AssignedToField = value;
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
	public string Location
	{
		get
		{
			return LocationField;
		}
		set
		{
			LocationField = value;
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

	[DataMember]
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
	public ushort State
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
	public string StateName
	{
		get
		{
			return StateNameField;
		}
		set
		{
			StateNameField = value;
		}
	}

	[DataMember]
	public ushort Category
	{
		get
		{
			return CategoryField;
		}
		set
		{
			CategoryField = value;
		}
	}

	[DataMember]
	public string CategoryName
	{
		get
		{
			return CategoryNameField;
		}
		set
		{
			CategoryNameField = value;
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
	public DateTime Modified
	{
		get
		{
			return ModifiedField;
		}
		set
		{
			ModifiedField = value;
		}
	}

	[DataMember]
	public bool FilterMatch
	{
		get
		{
			return FilterMatchField;
		}
		set
		{
			FilterMatchField = value;
		}
	}
}
