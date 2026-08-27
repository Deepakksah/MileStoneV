using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace VideoOS.Common.Integration.Definition.Plugin.Schema;

[Serializable]
[GeneratedCode("xsd", "4.6.1055.0")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://VideoOS.net/Surveillance/XmlSchema")]
public class dataTypeMaxvaluedisplayname
{
	private string langField;

	private string valueField;

	[XmlAttribute(DataType = "language")]
	public string lang
	{
		get
		{
			return langField;
		}
		set
		{
			langField = value;
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
}
