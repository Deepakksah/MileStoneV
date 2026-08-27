using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Management.Server;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "3.0.0.0")]
[DataContract(Name = "CreateMessagesMsgOut", Namespace = "urn:videoos:contracts:servicechannel:main:messagecreator:1")]
public class CreateMessagesMsgOut : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private OperationStatus CurrentStatusField;

	private int NoofSessionMessagesCreatedField;

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
	public OperationStatus CurrentStatus
	{
		get
		{
			return CurrentStatusField;
		}
		set
		{
			CurrentStatusField = value;
		}
	}

	[DataMember]
	public int NoofSessionMessagesCreated
	{
		get
		{
			return NoofSessionMessagesCreatedField;
		}
		set
		{
			NoofSessionMessagesCreatedField = value;
		}
	}
}
