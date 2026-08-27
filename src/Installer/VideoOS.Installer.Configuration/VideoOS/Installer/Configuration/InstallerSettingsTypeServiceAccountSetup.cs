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
public class InstallerSettingsTypeServiceAccountSetup
{
	private bool disableAllPreferedAccountField;

	private string disablePreferedAccountsField;

	private string defaultPredefAccountField;

	private bool disableAllAccountField;

	private string onParameterNameField;

	private string onParameterValueField;

	[XmlAttribute]
	[DefaultValue(false)]
	public bool DisableAllPreferedAccount
	{
		get
		{
			return disableAllPreferedAccountField;
		}
		set
		{
			disableAllPreferedAccountField = value;
		}
	}

	[XmlAttribute]
	[DefaultValue("")]
	public string DisablePreferedAccounts
	{
		get
		{
			return disablePreferedAccountsField;
		}
		set
		{
			disablePreferedAccountsField = value;
		}
	}

	[XmlAttribute]
	public string DefaultPredefAccount
	{
		get
		{
			return defaultPredefAccountField;
		}
		set
		{
			defaultPredefAccountField = value;
		}
	}

	[XmlAttribute]
	[DefaultValue(false)]
	public bool DisableAllAccount
	{
		get
		{
			return disableAllAccountField;
		}
		set
		{
			disableAllAccountField = value;
		}
	}

	[XmlAttribute]
	public string OnParameterName
	{
		get
		{
			return onParameterNameField;
		}
		set
		{
			onParameterNameField = value;
		}
	}

	[XmlAttribute]
	public string OnParameterValue
	{
		get
		{
			return onParameterValueField;
		}
		set
		{
			onParameterValueField = value;
		}
	}

	public InstallerSettingsTypeServiceAccountSetup()
	{
		disableAllPreferedAccountField = false;
		disablePreferedAccountsField = "";
		disableAllAccountField = false;
	}
}
