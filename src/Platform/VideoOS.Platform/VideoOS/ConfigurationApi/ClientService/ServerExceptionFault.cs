using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.ConfigurationApi.ClientService;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "ServerExceptionFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.ConfigurationAPI.ConfigurationFaultException")]
public class ServerExceptionFault : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private string MessageField;

	private string TranslationIdField;

	private string TypeField;

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
	public string Message
	{
		get
		{
			return MessageField;
		}
		set
		{
			MessageField = value;
		}
	}

	[DataMember]
	public string TranslationId
	{
		get
		{
			return TranslationIdField;
		}
		set
		{
			TranslationIdField = value;
		}
	}

	[DataMember]
	public string Type
	{
		get
		{
			return TypeField;
		}
		set
		{
			TypeField = value;
		}
	}
}
