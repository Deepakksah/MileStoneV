using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Server.ServiceChannel;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "3.0.0.0")]
[DataContract(Name = "ErrorItem", Namespace = "urn:videoos:contracts:servicechannel:main:common:1")]
public class ErrorItem : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private string ErrorDescriptionField;

	private int ErrorNumberField;

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
	public int ErrorNumber
	{
		get
		{
			return ErrorNumberField;
		}
		set
		{
			ErrorNumberField = value;
		}
	}
}
