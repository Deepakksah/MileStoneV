using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.ConfigurationApi.ClientService;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "ValidateResult", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.ConfigurationAPI")]
public class ValidateResult : IExtensibleDataObject
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	private ErrorResult[] ErrorResultsField;

	private ConfigurationItem ResultItemField;

	private bool ValidatedOkField;

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
	public ErrorResult[] ErrorResults
	{
		get
		{
			return ErrorResultsField;
		}
		set
		{
			ErrorResultsField = value;
		}
	}

	[DataMember]
	public ConfigurationItem ResultItem
	{
		get
		{
			return ResultItemField;
		}
		set
		{
			ResultItemField = value;
		}
	}

	[DataMember]
	public bool ValidatedOk
	{
		get
		{
			return ValidatedOkField;
		}
		set
		{
			ValidatedOkField = value;
		}
	}
}
