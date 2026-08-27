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
public class typeType
{
	private typedefType[] typedefField;

	private displaynameType displaynameField;

	private string nameField;

	private access accessField;

	private bool visibleField;

	private bool visibleFieldSpecified;

	private string referenceidField;

	[XmlElement("typedef")]
	public typedefType[] typedef
	{
		get
		{
			return typedefField;
		}
		set
		{
			typedefField = value;
		}
	}

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
	public access access
	{
		get
		{
			return accessField;
		}
		set
		{
			accessField = value;
		}
	}

	[XmlAttribute]
	public bool visible
	{
		get
		{
			return visibleField;
		}
		set
		{
			visibleField = value;
		}
	}

	[XmlIgnore]
	public bool visibleSpecified
	{
		get
		{
			return visibleFieldSpecified;
		}
		set
		{
			visibleFieldSpecified = value;
		}
	}

	[XmlAttribute]
	public string referenceid
	{
		get
		{
			return referenceidField;
		}
		set
		{
			referenceidField = value;
		}
	}
}
