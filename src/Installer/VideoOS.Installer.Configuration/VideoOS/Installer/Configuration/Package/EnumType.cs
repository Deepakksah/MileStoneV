using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace VideoOS.Installer.Configuration.Package;

[Serializable]
[GeneratedCode("xsd", "4.7.2053.0")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "urn:milestone-systems")]
public class EnumType
{
	private string indexField;

	private EnumStringType[] valuesField;

	[XmlElement(DataType = "integer")]
	public string Index
	{
		get
		{
			return indexField;
		}
		set
		{
			indexField = value;
		}
	}

	[XmlArrayItem("Value", IsNullable = false)]
	public EnumStringType[] Values
	{
		get
		{
			return valuesField;
		}
		set
		{
			valuesField = value;
		}
	}
}
