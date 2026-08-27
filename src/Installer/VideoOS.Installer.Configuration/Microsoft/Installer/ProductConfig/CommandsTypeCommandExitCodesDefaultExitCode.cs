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
public class CommandsTypeCommandExitCodesDefaultExitCode
{
	private object[] itemsField;

	private ExitCodeResultType resultField;

	private bool formatMessageFromSystemField;

	private bool formatMessageFromSystemFieldSpecified;

	private string stringField;

	public object[] Items
	{
		get
		{
			return itemsField;
		}
		set
		{
			itemsField = value;
		}
	}

	[XmlAttribute]
	public ExitCodeResultType Result
	{
		get
		{
			return resultField;
		}
		set
		{
			resultField = value;
		}
	}

	[XmlAttribute]
	public bool FormatMessageFromSystem
	{
		get
		{
			return formatMessageFromSystemField;
		}
		set
		{
			formatMessageFromSystemField = value;
		}
	}

	[XmlIgnore]
	public bool FormatMessageFromSystemSpecified
	{
		get
		{
			return formatMessageFromSystemFieldSpecified;
		}
		set
		{
			formatMessageFromSystemFieldSpecified = value;
		}
	}

	[XmlAttribute]
	public string String
	{
		get
		{
			return stringField;
		}
		set
		{
			stringField = value;
		}
	}
}
