using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.EventServer.Communication.Status.Client;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "StatusServiceFault", Namespace = "http://videoos.net/2/MapServerStatusCommand")]
public class StatusServiceFault : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private string ErrorDescriptionField;

	private StatusFaultType ErrorReasonField;

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
	public string ErrorDescription
	{
		get
		{
			return ErrorDescriptionField;
		}
		set
		{
			ErrorDescriptionField = value;
		}
	}

	[DataMember]
	public StatusFaultType ErrorReason
	{
		get
		{
			return ErrorReasonField;
		}
		set
		{
			ErrorReasonField = value;
		}
	}
}
