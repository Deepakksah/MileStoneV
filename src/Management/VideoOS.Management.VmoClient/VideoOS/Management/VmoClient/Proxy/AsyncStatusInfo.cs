using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient.Proxy;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "AsyncStatusInfo", Namespace = "http://schemas.datacontract.org/2008/06/VideoOS.Management.Objects")]
public class AsyncStatusInfo : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private ExceptionStatusInfo ExceptionStatusField;

	private string LastExceptionField;

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
	public ExceptionStatusInfo ExceptionStatus
	{
		get
		{
			return ExceptionStatusField;
		}
		set
		{
			ExceptionStatusField = value;
		}
	}

	[DataMember]
	public string LastException
	{
		get
		{
			return LastExceptionField;
		}
		set
		{
			LastExceptionField = value;
		}
	}
}
