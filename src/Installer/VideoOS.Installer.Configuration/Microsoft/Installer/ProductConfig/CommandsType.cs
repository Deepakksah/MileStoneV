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
[XmlType(Namespace = "http://schemas.microsoft.com/developer/2004/01/bootstrapper")]
public class CommandsType
{
	private CommandsTypeCommand[] commandField;

	private RebootType rebootField;

	[XmlElement("Command")]
	public CommandsTypeCommand[] Command
	{
		get
		{
			return commandField;
		}
		set
		{
			commandField = value;
		}
	}

	[XmlAttribute]
	[DefaultValue(RebootType.Immediate)]
	public RebootType Reboot
	{
		get
		{
			return rebootField;
		}
		set
		{
			rebootField = value;
		}
	}

	public CommandsType()
	{
		rebootField = RebootType.Immediate;
	}
}
