using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Installer.ProductConfig;

namespace VideoOS.Installer.Configuration.Package;

[Serializable]
[GeneratedCode("xsd", "4.7.2053.0")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "urn:milestone-systems")]
public class ProductInfoTypeProductManifest
{
	private XmlElement manifestXmlField;

	private string filenameField;

	private FormatType formatField;

	private string versionField;

	private object _manifestData;

	public XmlElement ManifestXml
	{
		get
		{
			return manifestXmlField;
		}
		set
		{
			manifestXmlField = value;
		}
	}

	[XmlAttribute]
	public string Filename
	{
		get
		{
			return filenameField;
		}
		set
		{
			filenameField = value;
		}
	}

	[XmlAttribute]
	public FormatType Format
	{
		get
		{
			return formatField;
		}
		set
		{
			formatField = value;
		}
	}

	[XmlAttribute]
	public string Version
	{
		get
		{
			return versionField;
		}
		set
		{
			versionField = value;
		}
	}

	public object ManifestData
	{
		get
		{
			return _manifestData;
		}
		set
		{
			_manifestData = value;
		}
	}

	public Product ProductData => (Product)_manifestData;

	public PackageType PackageData => (PackageType)_manifestData;
}
