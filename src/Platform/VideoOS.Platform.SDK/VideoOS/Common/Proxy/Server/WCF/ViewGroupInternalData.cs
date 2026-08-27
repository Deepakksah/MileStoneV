using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using System.Xml;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Common.Proxy.Server.WCF;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "ViewGroupInternalData", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Common.Proxy.Server")]
[ExcludeFromCodeCoverage]
public class ViewGroupInternalData : IExtensibleDataObject, IViewGroupData
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

	XmlNode IViewGroupData.ViewGroupDataXml
	{
		get
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(ViewGroupDataXml);
			return xmlDocument;
		}
		set
		{
			if (value != null && value.Attributes["xmlns"] == null && value is XmlElement xmlElement)
			{
				xmlElement.SetAttribute("xmlns", "");
			}
			ViewGroupDataXml = value.OuterXml;
		}
	}
}
