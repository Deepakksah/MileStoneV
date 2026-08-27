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
public class CommandType
{
	private string executableField;

	private string argumentsField;

	private CommandCondition conditionField;

	[XmlAttribute]
	public string Executable
	{
		get
		{
			return executableField;
		}
		set
		{
			executableField = value;
		}
	}

	[XmlAttribute]
	public string Arguments
	{
		get
		{
			return argumentsField;
		}
		set
		{
			argumentsField = value;
		}
	}

	[XmlAttribute]
	[DefaultValue(CommandCondition.Never)]
	public CommandCondition Condition
	{
		get
		{
			return conditionField;
		}
		set
		{
			conditionField = value;
		}
	}

	public CommandType()
	{
		conditionField = CommandCondition.Never;
	}
}
