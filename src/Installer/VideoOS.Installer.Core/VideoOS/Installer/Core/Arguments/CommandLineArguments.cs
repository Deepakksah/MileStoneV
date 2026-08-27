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
public class CommandLineArguments
{
	private InstallEnvironment installEnvironmentField;

	public InstallEnvironment InstallEnvironment
	{
		get
		{
			return installEnvironmentField;
		}
		set
		{
			installEnvironmentField = value;
		}
	}
}
