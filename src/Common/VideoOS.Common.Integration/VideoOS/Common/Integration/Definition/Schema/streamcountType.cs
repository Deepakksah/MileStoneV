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
public class streamcountType
{
	private string streamtypeField;

	private string valueField;

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

	[XmlAttribute(DataType = "integer")]
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
}
