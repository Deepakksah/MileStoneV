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
public class CustomPageSetupType
{
	private CustomPageSetupTypePluginAssembly pluginAssemblyField;

	public CustomPageSetupTypePluginAssembly PluginAssembly
	{
		get
		{
			return pluginAssemblyField;
		}
		set
		{
			pluginAssemblyField = value;
		}
	}
}
