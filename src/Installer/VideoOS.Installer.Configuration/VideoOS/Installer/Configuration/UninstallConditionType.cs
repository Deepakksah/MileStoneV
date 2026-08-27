using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace VideoOS.Installer.Configuration;

[Serializable]
[GeneratedCode("xsd", "4.7.2053.0")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "urn:milestone-systems")]
public class UninstallConditionType
{
	private UninstallConditionTypeByPassIf[] byPassIfField;

	private bool allowOtherSourceField;

	[XmlElement("ByPassIf")]
	public UninstallConditionTypeByPassIf[] ByPassIf
	{
		get
		{
			return byPassIfField;
		}
		set
		{
			byPassIfField = value;
		}
	}

	[XmlAttribute]
	[DefaultValue(true)]
	public bool AllowOtherSource
	{
		get
		{
			return allowOtherSourceField;
		}
		set
		{
			allowOtherSourceField = value;
		}
	}

	public UninstallConditionType()
	{
		allowOtherSourceField = true;
	}
}
