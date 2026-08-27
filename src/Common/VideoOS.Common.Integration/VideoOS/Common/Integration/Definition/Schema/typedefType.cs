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
public class typedefType
{
	private typedefcontentType[] itemsField;

	private typename typenameField;

	private datatype datatypeField;

	private string selectField;

	[XmlElement("data", typeof(datacontentType))]
	[XmlElement("enumeration", typeof(enumerationType))]
	public typedefcontentType[] Items
	{
		get
		{
			return itemsField;
		}
		set
		{
			itemsField = value;
		}
	}

	[XmlAttribute]
	public typename typename
	{
		get
		{
			return typenameField;
		}
		set
		{
			typenameField = value;
		}
	}

	[XmlAttribute]
	public datatype datatype
	{
		get
		{
			return datatypeField;
		}
		set
		{
			datatypeField = value;
		}
	}

	[XmlAttribute]
	public string select
	{
		get
		{
			return selectField;
		}
		set
		{
			selectField = value;
		}
	}
}
