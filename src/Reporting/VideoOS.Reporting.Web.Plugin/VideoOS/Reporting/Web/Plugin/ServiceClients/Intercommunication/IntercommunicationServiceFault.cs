using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Reporting.Web.Plugin.ServiceClients.Intercommunication;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "IntercommunicationServiceFault", Namespace = "http://videoos.net/2/CentralServerInterCommunicationCommand")]
public class IntercommunicationServiceFault : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private string ErrorDescriptionField;

	private IntercommunicationFaultType ErrorReasonField;

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
	public IntercommunicationFaultType ErrorReason
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
