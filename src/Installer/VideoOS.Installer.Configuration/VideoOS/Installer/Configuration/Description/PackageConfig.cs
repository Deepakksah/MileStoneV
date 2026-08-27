using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace VideoOS.Installer.Configuration.Description;

[Serializable]
[GeneratedCode("xsd", "4.7.2053.0")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "urn:milestone-systems")]
[XmlRoot(Namespace = "urn:milestone-systems", IsNullable = false)]
public class PackageConfig
{
	private string nameField;

	private PackageDescriptionType[] packageDescriptionsField;

	public string Name
	{
		get
		{
			return nameField;
		}
		set
		{
			nameField = value;
		}
	}

	[XmlArrayItem("PackageDescription", IsNullable = false)]
	public PackageDescriptionType[] PackageDescriptions
	{
		get
		{
			return packageDescriptionsField;
		}
		set
		{
			packageDescriptionsField = value;
		}
	}
}
