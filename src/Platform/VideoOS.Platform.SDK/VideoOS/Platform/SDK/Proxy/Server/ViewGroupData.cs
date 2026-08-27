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
public class ViewGroupData : IViewGroupData
{
	private int dataVersionField;

	private int xmlVersionField;

	private XmlNode viewGroupDataXmlField;

	XmlNode IViewGroupData.ViewGroupDataXml
	{
		get
		{
			return ViewGroupDataXml;
		}
		set
		{
			ViewGroupDataXml = value;
		}
	}

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

	public int XmlVersion
	{
		get
		{
			return xmlVersionField;
		}
		set
		{
			xmlVersionField = value;
		}
	}

	public XmlNode ViewGroupDataXml
	{
		get
		{
			return viewGroupDataXmlField;
		}
		set
		{
			viewGroupDataXmlField = value;
		}
	}
}
