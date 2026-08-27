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
public class LicenseInfoTypeLicUpgrade
{
	private string upgradeCodeField;

	private string supInfoLinkField;

	[XmlAttribute]
	public string UpgradeCode
	{
		get
		{
			return upgradeCodeField;
		}
		set
		{
			upgradeCodeField = value;
		}
	}

	[XmlAttribute]
	public string SupInfoLink
	{
		get
		{
			return supInfoLinkField;
		}
		set
		{
			supInfoLinkField = value;
		}
	}
}
