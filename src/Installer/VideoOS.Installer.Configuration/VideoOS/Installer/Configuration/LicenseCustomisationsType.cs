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
public class LicenseCustomisationsType
{
	private LicenseCustomisationsTypeLicenseCustom[] licenseCustomField;

	private bool byLicenseField;

	[XmlElement("LicenseCustom")]
	public LicenseCustomisationsTypeLicenseCustom[] LicenseCustom
	{
		get
		{
			return licenseCustomField;
		}
		set
		{
			licenseCustomField = value;
		}
	}

	[XmlAttribute]
	[DefaultValue(false)]
	public bool ByLicense
	{
		get
		{
			return byLicenseField;
		}
		set
		{
			byLicenseField = value;
		}
	}

	public LicenseCustomisationsType()
	{
		byLicenseField = false;
	}
}
