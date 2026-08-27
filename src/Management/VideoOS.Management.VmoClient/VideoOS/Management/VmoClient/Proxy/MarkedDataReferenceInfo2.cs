using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient.Proxy;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "MarkedDataReferenceInfo2", Namespace = "http://schemas.datacontract.org/2008/06/VideoOS.Management.Objects")]
public class MarkedDataReferenceInfo2 : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private string[] DevicesField;

	private string ReferenceField;

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
	public string[] Devices
	{
		get
		{
			return DevicesField;
		}
		set
		{
			DevicesField = value;
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
