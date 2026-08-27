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
public class ActionStartServiceType
{
	private StringType nameField;

	public StringType Name
	{
		get
		{
			return nameField;
		}
		set
		{
			nameField = value;
		}
	}
}
