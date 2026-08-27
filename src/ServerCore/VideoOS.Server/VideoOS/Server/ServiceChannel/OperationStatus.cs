using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Server.ServiceChannel;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "3.0.0.0")]
[DataContract(Name = "OperationStatus", Namespace = "urn:videoos:contracts:servicechannel:main:common:1")]
public class OperationStatus : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private ErrorItem[] ErrorsField;

	private OperationStatusEnum StatusField;

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
	public ErrorItem[] Errors
	{
		get
		{
			return ErrorsField;
		}
		set
		{
			ErrorsField = value;
		}
	}

	[DataMember]
	public OperationStatusEnum Status
	{
		get
		{
			return StatusField;
		}
		set
		{
			StatusField = value;
		}
	}
}
