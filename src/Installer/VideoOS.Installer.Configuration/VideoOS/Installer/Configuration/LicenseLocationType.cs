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
public class LicenseLocationType
{
	private string pathField;

	private string nameField;

	private LicenseLocationTypeType typeField;

	[XmlAttribute]
	public string Path
	{
		get
		{
			return pathField;
		}
		set
		{
			pathField = value;
		}
	}

	[XmlAttribute]
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

	[XmlAttribute]
	public LicenseLocationTypeType Type
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
