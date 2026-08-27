using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Microsoft.Installer.ProductConfig;

[Serializable]
[GeneratedCode("xsd", "4.7.2053.0")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/2004/01/bootstrapper")]
public class CommandsTypeCommandBypassIf
{
	private string propertyField;

	private string valueField;

	private string scheduleField;

	private CompareType compareField;

	[XmlAttribute]
	public string Property
	{
		get
		{
			return propertyField;
		}
		set
		{
			propertyField = value;
		}
	}

	[XmlAttribute]
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

	[XmlAttribute]
	public string Schedule
	{
		get
		{
			return scheduleField;
		}
		set
		{
			scheduleField = value;
		}
	}

	[XmlAttribute]
	public CompareType Compare
	{
		get
		{
			return compareField;
		}
		set
		{
			compareField = value;
		}
	}
}
