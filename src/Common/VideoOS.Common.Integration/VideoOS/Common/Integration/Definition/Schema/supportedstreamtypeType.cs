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
public class supportedstreamtypeType
{
	private string nameField;

	private displaynameType displaynameField;

	private streamcountType[] allowedconcurrentField;

	private string referenceidField;

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

	[XmlArrayItem("streamcount", IsNullable = false)]
	public streamcountType[] allowedconcurrent
	{
		get
		{
			return allowedconcurrentField;
		}
		set
		{
			allowedconcurrentField = value;
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
