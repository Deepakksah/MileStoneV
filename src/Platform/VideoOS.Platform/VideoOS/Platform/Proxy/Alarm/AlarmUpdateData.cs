using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Platform.Proxy.Alarm;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "AlarmUpdateData", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Platform.Proxy.Alarm")]
public class AlarmUpdateData : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private Guid[] DeletedField;

	private AlarmLine[] InsertedField;

	private AlarmLine[] UpdatedField;

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
	public Guid[] Deleted
	{
		get
		{
			return DeletedField;
		}
		set
		{
			DeletedField = value;
		}
	}

	[DataMember]
	public AlarmLine[] Inserted
	{
		get
		{
			return InsertedField;
		}
		set
		{
			InsertedField = value;
		}
	}

	[DataMember]
	public AlarmLine[] Updated
	{
		get
		{
			return UpdatedField;
		}
		set
		{
			UpdatedField = value;
		}
	}
}
