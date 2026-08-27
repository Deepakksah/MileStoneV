using System;
using System.CodeDom.Compiler;
using System.Xml.Serialization;

namespace VideoOS.Installer.Core.Arguments;

[Serializable]
[GeneratedCode("xsd", "4.7.2053.0")]
[XmlType(Namespace = "urn:milestone-systems")]
public enum LicenseType
{
	LicenseKey,
	LicenseService,
	LicenseFile,
	Trial,
	None
}
