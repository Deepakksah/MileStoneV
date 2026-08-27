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
public class InstallerPackageTypePostUninstallActions
{
	private string productCodeField;

	private string upgradeCodeField;

	private bool pendingFileRenameOperationsField;

	[XmlAttribute]
	public string ProductCode
	{
		get
		{
			return productCodeField;
		}
		set
		{
			productCodeField = value;
		}
	}

	[XmlAttribute]
	public string UpgradeCode
	{
		get
		{
			return upgradeCodeField;
		}
		set
		{
			upgradeCodeField = value;
		}
	}

	[XmlAttribute]
	[DefaultValue(false)]
	public bool PendingFileRenameOperations
	{
		get
		{
			return pendingFileRenameOperationsField;
		}
		set
		{
			pendingFileRenameOperationsField = value;
		}
	}

	public InstallerPackageTypePostUninstallActions()
	{
		pendingFileRenameOperationsField = false;
	}
}
