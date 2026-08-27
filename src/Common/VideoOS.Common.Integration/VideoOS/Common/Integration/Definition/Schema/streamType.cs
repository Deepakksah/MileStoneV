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
public class streamType
{
	private string nameField;

	private displaynameType displaynameField;

	private capabilitiesType capabilitiesField;

	private string selectField;

	private string referenceidField;

	private string streamtypeField;

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

	public capabilitiesType capabilities
	{
		get
		{
			return capabilitiesField;
		}
		set
		{
			capabilitiesField = value;
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
	public string streamtype
	{
		get
		{
			return streamtypeField;
		}
		set
		{
			streamtypeField = value;
		}
	}
}
