using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.EventServer.Communication.Map.Client;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "TypeChanged", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Central.Server")]
public class TypeChanged : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private bool ChangedField;

	private DateTime TimestampField;

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
	public bool Changed
	{
		get
		{
			return ChangedField;
		}
		set
		{
			ChangedField = value;
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
}
