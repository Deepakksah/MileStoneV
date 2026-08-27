using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Xml;
using System.Xml.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Platform.SDK.Proxy.Server;

[Serializable]
[GeneratedCode("wsdl", "4.0.30319.17929")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
[ExcludeFromCodeCoverage]
public class CustomSettingData : ICustomSettingData
{
	private int dataVersionField;

	private XmlNode customSettingDataXmlField;

	public int DataVersion
	{
		get
		{
			return dataVersionField;
		}
		set
		{
			dataVersionField = value;
		}
	}

	public XmlNode CustomSettingDataXml
	{
		get
		{
			return customSettingDataXmlField;
		}
		set
		{
			customSettingDataXmlField = value;
		}
	}
}
