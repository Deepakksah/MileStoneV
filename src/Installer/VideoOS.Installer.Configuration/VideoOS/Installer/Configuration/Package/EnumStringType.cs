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
public class EnumStringType
{
	private string actualValueField;

	private StringType displayNameField;

	public string ActualValue
	{
		get
		{
			return actualValueField;
		}
		set
		{
			actualValueField = value;
		}
	}

	public StringType DisplayName
	{
		get
		{
			return displayNameField;
		}
		set
		{
			displayNameField = value;
		}
	}
}
