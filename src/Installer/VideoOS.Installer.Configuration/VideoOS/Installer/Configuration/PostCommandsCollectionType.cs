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
public class PostCommandsCollectionType
{
	private CommandType[] commandsField;

	private ServiceActionOnType[] serviceActionsOnField;

	[XmlArrayItem("Command", IsNullable = false)]
	public CommandType[] Commands
	{
		get
		{
			return commandsField;
		}
		set
		{
			commandsField = value;
		}
	}

	[XmlArrayItem("ServiceAction", IsNullable = false)]
	public ServiceActionOnType[] ServiceActionsOn
	{
		get
		{
			return serviceActionsOnField;
		}
		set
		{
			serviceActionsOnField = value;
		}
	}
}
