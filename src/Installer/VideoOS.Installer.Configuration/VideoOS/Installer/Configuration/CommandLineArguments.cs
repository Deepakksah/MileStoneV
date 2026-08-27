using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace VideoOS.Installer.Configuration;

[Serializable]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "urn:milestone-systems")]
[XmlRoot(Namespace = "urn:milestone-systems", IsNullable = false)]
public class CommandLineArguments
{
	private InstallEnvironment _installEnvironment;

	public InstallEnvironment InstallEnvironment
	{
		get
		{
			return _installEnvironment;
		}
		set
		{
			_installEnvironment = value;
		}
	}
}
