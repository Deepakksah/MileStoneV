using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.EventServer.Communication.Map.Client;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "MapServiceFault2", Namespace = "http://videoos.net/2/EventServerMapCommand")]
public class MapServiceFault2 : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private string ErrorDescriptionField;

	private string ErrorReasonField;

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
	public string ErrorReason
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
