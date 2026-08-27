using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace ServerCommandService;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "ViewGroupInternalData", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Common.Proxy.Server")]
public class ViewGroupInternalData : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private int DataVersionField;

	private string ViewGroupDataXmlField;

	private int XmlVersionField;

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

	[DataMember]
	public string ViewGroupDataXml
	{
		get
		{
			return ViewGroupDataXmlField;
		}
		set
		{
			ViewGroupDataXmlField = value;
		}
	}

	[DataMember]
	public int XmlVersion
	{
		get
		{
			return XmlVersionField;
		}
		set
		{
			XmlVersionField = value;
		}
	}
}
