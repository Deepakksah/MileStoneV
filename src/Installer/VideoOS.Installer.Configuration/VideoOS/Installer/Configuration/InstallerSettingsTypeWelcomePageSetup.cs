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
public class InstallerSettingsTypeWelcomePageSetup
{
	private bool disableTrialModeField;

	[XmlAttribute]
	[DefaultValue(false)]
	public bool DisableTrialMode
	{
		get
		{
			return disableTrialModeField;
		}
		set
		{
			disableTrialModeField = value;
		}
	}

	public InstallerSettingsTypeWelcomePageSetup()
	{
		disableTrialModeField = false;
	}
}
