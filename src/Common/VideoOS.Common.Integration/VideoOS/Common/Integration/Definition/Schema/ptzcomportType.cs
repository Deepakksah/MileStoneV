using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace VideoOS.Common.Integration.Definition.Schema;

[Serializable]
[GeneratedCode("xsd", "4.7.2558.0")]
[DebuggerStepThrough]
[DesignerCategory("code")]
public class ptzcomportType
{
	private displaynameType displaynameField;

	private string nameField;

	private string valueField;

	private bool multipleprotocolsField;

	public displaynameType displayname
	{
		get
		{
			return displaynameField;
		}
		set
		{
			displaynameField = value;
		}
	}

	[XmlAttribute]
	public string name
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
	public string value
	{
		get
		{
			return valueField;
		}
		set
		{
			valueField = value;
		}
	}

	[XmlAttribute]
	public bool multipleprotocols
	{
		get
		{
			return multipleprotocolsField;
		}
		set
		{
			multipleprotocolsField = value;
		}
	}
}
