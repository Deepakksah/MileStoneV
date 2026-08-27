using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Event.Server.WebService.Action.Client;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "ActionServiceFault", Namespace = "http://videoos.net/2/CentralServerActionCommand")]
public class ActionServiceFault : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private string ErrorDescriptionField;

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
}
