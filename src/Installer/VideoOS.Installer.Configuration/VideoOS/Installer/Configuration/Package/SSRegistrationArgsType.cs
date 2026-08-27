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
public class SSRegistrationArgsType
{
	private argsType argsField;

	public argsType args
	{
		get
		{
			return argsField;
		}
		set
		{
			argsField = value;
		}
	}
}
