using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Installer.ProductConfig;
using VideoOS.Installer.Configuration.Description;
using VideoOS.Installer.Configuration.Package;

namespace VideoOS.Installer.Configuration;

[Serializable]
[GeneratedCode("xsd", "4.7.2053.0")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "urn:milestone-systems")]
public class InstallerPackageTypeProductManifest
{
	private XmlElement manifestXmlField;

	private string filenameField;

	private FormatType formatField;

	private string versionField;

	private bool mainPackageField;

	private string platformField;

	private object _manifestData;

	private PackageConfig _packageConfig;

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

	[XmlAttribute]
	[DefaultValue(false)]
	public bool MainPackage
	{
		get
		{
			return mainPackageField;
		}
		set
		{
			mainPackageField = value;
		}
	}

	[XmlAttribute]
	[DefaultValue("x86")]
	public string Platform
	{
		get
		{
			return platformField;
		}
		set
		{
			platformField = value;
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

	public PackageConfig PackageConfig
	{
		get
		{
			return _packageConfig;
		}
		set
		{
			_packageConfig = value;
		}
	}

	public InstallerPackageTypeProductManifest()
	{
		mainPackageField = false;
		platformField = "x86";
	}
}
