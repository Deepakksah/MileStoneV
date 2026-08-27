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
public class enumValueType
{
	private string nameField;

	private string tagField;

	private displaynameType displaynameField;

	private string referenceidField;

	private double numericvalueField;

	private bool numericvalueFieldSpecified;

	private string selectField;

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

	public string tag
	{
		get
		{
			return tagField;
		}
		set
		{
			tagField = value;
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

	[XmlAttribute]
	public double numericvalue
	{
		get
		{
			return numericvalueField;
		}
		set
		{
			numericvalueField = value;
		}
	}

	[XmlIgnore]
	public bool numericvalueSpecified
	{
		get
		{
			return numericvalueFieldSpecified;
		}
		set
		{
			numericvalueFieldSpecified = value;
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
