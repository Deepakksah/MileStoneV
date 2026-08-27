using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace VideoOS.Installer.Core.Arguments;

[Serializable]
[GeneratedCode("xsd", "4.7.2053.0")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "urn:milestone-systems")]
[XmlRoot(Namespace = "urn:milestone-systems", IsNullable = false)]
public class ArgumentsXmlV2
{
	private ArgumentType[] argumentsField;

	[XmlArrayItem("Argument", IsNullable = false)]
	public ArgumentType[] Arguments
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
}
