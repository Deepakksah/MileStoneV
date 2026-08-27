using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace VideoOS.Installer.Configuration;

[Serializable]
[GeneratedCode("xsd", "4.7.2053.0")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "urn:milestone-systems")]
public class InstallerPackageTypeProductManifest64
{
	private string filenameField;

	private FormatType formatField;

	private string versionField;

	private bool mainPackageField;

	private string platformField;

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
	[DefaultValue("x64")]
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

	public InstallerPackageTypeProductManifest64()
	{
		mainPackageField = false;
		platformField = "x64";
	}
}
