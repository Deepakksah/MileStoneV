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
public class InstallerPackageTypePostInstallActions
{
	private bool performSSRegistrationField;

	private bool pendingFileRenameOperationsField;

	[XmlAttribute]
	[DefaultValue(false)]
	public bool PerformSSRegistration
	{
		get
		{
			return performSSRegistrationField;
		}
		set
		{
			performSSRegistrationField = value;
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

	public InstallerPackageTypePostInstallActions()
	{
		performSSRegistrationField = false;
		pendingFileRenameOperationsField = false;
	}
}
