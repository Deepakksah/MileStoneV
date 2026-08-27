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
public class CommandsTypeCommand
{
	private CommandsTypeCommandExitCodes exitCodesField;

	private object[] installConditionsField;

	private string packageFileField;

	private string argumentsField;

	private string estimatedInstallSecondsField;

	private string estimatedInstalledBytesField;

	private string estimatedTempBytesField;

	private string logField;

	public CommandsTypeCommandExitCodes ExitCodes
	{
		get
		{
			return exitCodesField;
		}
		set
		{
			exitCodesField = value;
		}
	}

	[XmlArrayItem("BypassIf", typeof(CommandsTypeCommandBypassIf), IsNullable = false)]
	[XmlArrayItem("FailIf", typeof(CommandsTypeCommandFailIf), IsNullable = false)]
	public object[] InstallConditions
	{
		get
		{
			return installConditionsField;
		}
		set
		{
			installConditionsField = value;
		}
	}

	[XmlAttribute]
	public string PackageFile
	{
		get
		{
			return packageFileField;
		}
		set
		{
			packageFileField = value;
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

	[XmlAttribute(DataType = "nonNegativeInteger")]
	[DefaultValue("0")]
	public string EstimatedInstallSeconds
	{
		get
		{
			return estimatedInstallSecondsField;
		}
		set
		{
			estimatedInstallSecondsField = value;
		}
	}

	[XmlAttribute(DataType = "nonNegativeInteger")]
	[DefaultValue("0")]
	public string EstimatedInstalledBytes
	{
		get
		{
			return estimatedInstalledBytesField;
		}
		set
		{
			estimatedInstalledBytesField = value;
		}
	}

	[XmlAttribute(DataType = "nonNegativeInteger")]
	[DefaultValue("0")]
	public string EstimatedTempBytes
	{
		get
		{
			return estimatedTempBytesField;
		}
		set
		{
			estimatedTempBytesField = value;
		}
	}

	[XmlAttribute]
	public string Log
	{
		get
		{
			return logField;
		}
		set
		{
			logField = value;
		}
	}

	public CommandsTypeCommand()
	{
		estimatedInstallSecondsField = "0";
		estimatedInstalledBytesField = "0";
		estimatedTempBytesField = "0";
	}
}
