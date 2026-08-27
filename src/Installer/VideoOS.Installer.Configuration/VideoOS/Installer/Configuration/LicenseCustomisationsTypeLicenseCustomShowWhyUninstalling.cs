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
public class LicenseCustomisationsTypeLicenseCustomShowWhyUninstalling
{
	private string linkField;

	[XmlAttribute]
	public string Link
	{
		get
		{
			return linkField;
		}
		set
		{
			linkField = value;
		}
	}
}
