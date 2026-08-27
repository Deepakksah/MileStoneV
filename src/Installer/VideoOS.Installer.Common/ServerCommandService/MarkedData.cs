using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace ServerCommandService;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "MarkedData", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class MarkedData : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private DateTime CreatedField;

	private string DescriptionField;

	private Guid[] DeviceIdsField;

	private DateTime EndTimeField;

	private string HeaderField;

	private Guid IdField;

	private DateTime LastModifiedField;

	private string ReferenceField;

	private DateTime RetentionExpireField;

	private RetentionOption RetentionOptionField;

	private long RetentionSizeField;

	private DateTime StartTimeField;

	private DateTime TagTimeField;

	private bool UseRetentionField;

	private string UserField;

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
	public DateTime Created
	{
		get
		{
			return CreatedField;
		}
		set
		{
			CreatedField = value;
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
	public Guid[] DeviceIds
	{
		get
		{
			return DeviceIdsField;
		}
		set
		{
			DeviceIdsField = value;
		}
	}

	[DataMember]
	public DateTime EndTime
	{
		get
		{
			return EndTimeField;
		}
		set
		{
			EndTimeField = value;
		}
	}

	[DataMember]
	public string Header
	{
		get
		{
			return HeaderField;
		}
		set
		{
			HeaderField = value;
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
	public DateTime LastModified
	{
		get
		{
			return LastModifiedField;
		}
		set
		{
			LastModifiedField = value;
		}
	}

	[DataMember]
	public string Reference
	{
		get
		{
			return ReferenceField;
		}
		set
		{
			ReferenceField = value;
		}
	}

	[DataMember]
	public DateTime RetentionExpire
	{
		get
		{
			return RetentionExpireField;
		}
		set
		{
			RetentionExpireField = value;
		}
	}

	[DataMember]
	public RetentionOption RetentionOption
	{
		get
		{
			return RetentionOptionField;
		}
		set
		{
			RetentionOptionField = value;
		}
	}

	[DataMember]
	public long RetentionSize
	{
		get
		{
			return RetentionSizeField;
		}
		set
		{
			RetentionSizeField = value;
		}
	}

	[DataMember]
	public DateTime StartTime
	{
		get
		{
			return StartTimeField;
		}
		set
		{
			StartTimeField = value;
		}
	}

	[DataMember]
	public DateTime TagTime
	{
		get
		{
			return TagTimeField;
		}
		set
		{
			TagTimeField = value;
		}
	}

	[DataMember]
	public bool UseRetention
	{
		get
		{
			return UseRetentionField;
		}
		set
		{
			UseRetentionField = value;
		}
	}

	[DataMember]
	public string User
	{
		get
		{
			return UserField;
		}
		set
		{
			UserField = value;
		}
	}
}
