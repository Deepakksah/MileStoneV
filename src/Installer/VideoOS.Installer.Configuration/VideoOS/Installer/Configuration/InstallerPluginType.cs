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
public class InstallerPluginType
{
	private InstallerPluginTypePluginAssembly pluginAssemblyField;

	private string[] supportedLanguagesField;

	private ParameterType[] parametersField;

	private InstallerPluginTypeWizardPage[] wizardPagesField;

	private InstallerPluginTypeFlowStep[] pageFlowField;

	private string nameField;

	private string filenameField;

	public InstallerPluginTypePluginAssembly PluginAssembly
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

	[XmlArrayItem("Language", IsNullable = false)]
	public string[] SupportedLanguages
	{
		get
		{
			return supportedLanguagesField;
		}
		set
		{
			supportedLanguagesField = value;
		}
	}

	[XmlArrayItem("Parameter", IsNullable = false)]
	public ParameterType[] Parameters
	{
		get
		{
			return parametersField;
		}
		set
		{
			parametersField = value;
		}
	}

	[XmlArrayItem("WizardPage", IsNullable = false)]
	public InstallerPluginTypeWizardPage[] WizardPages
	{
		get
		{
			return wizardPagesField;
		}
		set
		{
			wizardPagesField = value;
		}
	}

	[XmlArrayItem("FlowStep", IsNullable = false)]
	public InstallerPluginTypeFlowStep[] PageFlow
	{
		get
		{
			return pageFlowField;
		}
		set
		{
			pageFlowField = value;
		}
	}

	[XmlAttribute]
	public string Name
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

	[XmlAttribute]
	public string Filename
	{
		get
		{
			return filenameField;
		}
		set
		{
			filenameField = value;
		}
	}
}
