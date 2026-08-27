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
public class CommandsTypeCommandExitCodes
{
	private CommandsTypeCommandExitCodesExitCode[] exitCodeField;

	private CommandsTypeCommandExitCodesDefaultExitCode defaultExitCodeField;

	[XmlElement("ExitCode")]
	public CommandsTypeCommandExitCodesExitCode[] ExitCode
	{
		get
		{
			return exitCodeField;
		}
		set
		{
			exitCodeField = value;
		}
	}

	public CommandsTypeCommandExitCodesDefaultExitCode DefaultExitCode
	{
		get
		{
			return defaultExitCodeField;
		}
		set
		{
			defaultExitCodeField = value;
		}
	}
}
