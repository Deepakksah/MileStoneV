using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace ServerCommandService;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "CustomSettingInternalData", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Common.Proxy.Server")]
public class CustomSettingInternalData : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private string CustomSettingDataXmlField;

	private int DataVersionField;

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
	public string CustomSettingDataXml
	{
		get
		{
			return CustomSettingDataXmlField;
		}
		set
		{
			CustomSettingDataXmlField = value;
		}
	}

	[DataMember]
	public int DataVersion
	{
		get
		{
			return DataVersionField;
		}
		set
		{
			DataVersionField = value;
		}
	}
}
