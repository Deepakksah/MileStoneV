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
public class InstallerPackageTypePreInstallActions
{
	private bool uninstallField;

	private UninstallTypeType uninstallTypeField;

	private bool confirmInstallField;

	private bool allowDowngradeField;

	[XmlAttribute]
	public bool Uninstall
	{
		get
		{
			return uninstallField;
		}
		set
		{
			uninstallField = value;
		}
	}

	[XmlAttribute]
	[DefaultValue(UninstallTypeType.MSI)]
	public UninstallTypeType UninstallType
	{
		get
		{
			return uninstallTypeField;
		}
		set
		{
			uninstallTypeField = value;
		}
	}

	[XmlAttribute]
	[DefaultValue(false)]
	public bool ConfirmInstall
	{
		get
		{
			return confirmInstallField;
		}
		set
		{
			confirmInstallField = value;
		}
	}

	[XmlAttribute]
	[DefaultValue(false)]
	public bool AllowDowngrade
	{
		get
		{
			return allowDowngradeField;
		}
		set
		{
			allowDowngradeField = value;
		}
	}

	public InstallerPackageTypePreInstallActions()
	{
		uninstallTypeField = UninstallTypeType.MSI;
		confirmInstallField = false;
		allowDowngradeField = false;
	}
}
