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
[XmlType(AnonymousType = true, Namespace = "urn:milestone-systems")]
public class InstallerPluginTypeWizardPage
{
	private InstallerPluginTypeWizardPageByPassIf byPassIfField;

	private string nameField;

	private string idField;

	private bool skipInTypicalField;

	private bool uninstallPageField;

	public InstallerPluginTypeWizardPageByPassIf ByPassIf
	{
		get
		{
			return byPassIfField;
		}
		set
		{
			byPassIfField = value;
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
	public string ID
	{
		get
		{
			return idField;
		}
		set
		{
			idField = value;
		}
	}

	[XmlAttribute]
	[DefaultValue(false)]
	public bool SkipInTypical
	{
		get
		{
			return skipInTypicalField;
		}
		set
		{
			skipInTypicalField = value;
		}
	}

	[XmlAttribute]
	[DefaultValue(false)]
	public bool UninstallPage
	{
		get
		{
			return uninstallPageField;
		}
		set
		{
			uninstallPageField = value;
		}
	}

	public InstallerPluginTypeWizardPage()
	{
		skipInTypicalField = false;
		uninstallPageField = false;
	}
}
