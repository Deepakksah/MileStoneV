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
public class LicenseInfoType
{
	private LicenseLocationType licenseLocationField;

	private LicenseInfoTypeLicUpgrade licUpgradeField;

	public LicenseLocationType LicenseLocation
	{
		get
		{
			return licenseLocationField;
		}
		set
		{
			licenseLocationField = value;
		}
	}

	public LicenseInfoTypeLicUpgrade LicUpgrade
	{
		get
		{
			return licUpgradeField;
		}
		set
		{
			licUpgradeField = value;
		}
	}
}
