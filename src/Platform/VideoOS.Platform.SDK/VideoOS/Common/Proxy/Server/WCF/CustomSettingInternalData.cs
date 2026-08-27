using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using System.Xml;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Common.Proxy.Server.WCF;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "CustomSettingInternalData", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Common.Proxy.Server")]
[ExcludeFromCodeCoverage]
public class CustomSettingInternalData : IExtensibleDataObject, ICustomSettingData
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

	XmlNode ICustomSettingData.CustomSettingDataXml
	{
		get
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(CustomSettingDataXml);
			return xmlDocument.FirstChild;
		}
		set
		{
			if (value != null)
			{
				if (value.Attributes != null && value.Attributes["xmlns"] == null && value is XmlElement xmlElement)
				{
					xmlElement.SetAttribute("xmlns", "");
				}
				CustomSettingDataXml = value.OuterXml;
			}
		}
	}
}
