using System;
using System.CodeDom.Compiler;
using System.Xml.Serialization;

namespace VideoOS.Installer.Configuration.Package;

[Serializable]
[GeneratedCode("xsd", "4.7.2053.0")]
[XmlType(Namespace = "urn:milestone-systems")]
public enum InstallerFrameworkType
{
	MSI,
	InstallShield,
	InstallShieldWithMSI,
	WiseInstallMaster,
	InnoSetup,
	NSIS,
	GhostInstaller,
	MSIBootstrapper
}
