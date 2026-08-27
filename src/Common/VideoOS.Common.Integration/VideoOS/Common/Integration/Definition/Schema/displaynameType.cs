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
public class displaynameType
{
	private string referenceidField;

	private string valueField;

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

	[XmlText]
	public string Value
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

	public string Resolve()
	{
		return DefinitionUtility.ResolveDisplayName(this);
	}
}
