using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.EventServer.Communication.Configuration;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "GetDefaultEventRetentionParams", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
public class GetDefaultEventRetentionParams : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private string CultureField;

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
	public string Culture
	{
		get
		{
			return CultureField;
		}
		set
		{
			CultureField = value;
		}
	}
}
