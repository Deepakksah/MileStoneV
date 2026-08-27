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
public class enumerationType : typedefcontentType
{
	private string defaultvalueField;

	private enumValueType[] valueField;

	public string defaultvalue
	{
		get
		{
			return defaultvalueField;
		}
		set
		{
			defaultvalueField = value;
		}
	}

	[XmlElement("value")]
	public enumValueType[] value
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
}
