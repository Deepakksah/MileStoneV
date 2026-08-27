using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.ConfigurationApi.ClientService;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "ErrorResult", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.ConfigurationAPI")]
public class ErrorResult : IExtensibleDataObject
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	private string ErrorPropertyField;

	private string ErrorTextField;

	private string ErrorTextIdField;

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
	public string ErrorProperty
	{
		get
		{
			return ErrorPropertyField;
		}
		set
		{
			ErrorPropertyField = value;
		}
	}

	[DataMember]
	public string ErrorText
	{
		get
		{
			return ErrorTextField;
		}
		set
		{
			ErrorTextField = value;
		}
	}

	[DataMember]
	public string ErrorTextId
	{
		get
		{
			return ErrorTextIdField;
		}
		set
		{
			ErrorTextIdField = value;
		}
	}
}
