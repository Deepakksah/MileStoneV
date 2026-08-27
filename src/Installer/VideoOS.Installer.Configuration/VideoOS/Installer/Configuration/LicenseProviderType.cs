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
[XmlType(Namespace = "urn:milestone-systems")]
public class LicenseProviderType
{
	private LicenseInfoType licenseInfoField;

	private LicenseCustomisationsType licenseCustomisationsField;

	private string filenameField;

	private LicenseType typeField;

	public LicenseInfoType LicenseInfo
	{
		get
		{
			return licenseInfoField;
		}
		set
		{
			licenseInfoField = value;
		}
	}

	public LicenseCustomisationsType LicenseCustomisations
	{
		get
		{
			return licenseCustomisationsField;
		}
		set
		{
			licenseCustomisationsField = value;
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
	public LicenseType Type
	{
		get
		{
			return typeField;
		}
		set
		{
			typeField = value;
		}
	}
}
