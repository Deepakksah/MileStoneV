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
public class ShowPageType
{
	private ShowPageTypeByPassIf[] byPassIfField;

	private string nameField;

	[XmlElement("ByPassIf")]
	public ShowPageTypeByPassIf[] ByPassIf
	{
		get
		{
			return byPassIfField;
		}
		set
		{
			byPassIfField = value;
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
}
