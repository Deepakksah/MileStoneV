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
[XmlType(Namespace = "http://VideoOS.net/Surveillance/XmlSchema")]
public class enumerationType
{
	private string defaultvalueField;

	private InformationType[] valueField;

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
	public InformationType[] value
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
