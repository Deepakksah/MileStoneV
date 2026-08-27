using System;
using System.CodeDom.Compiler;
using System.Xml.Serialization;

namespace VideoOS.Installer.Configuration;

[Serializable]
[GeneratedCode("xsd", "4.7.2053.0")]
[XmlType(AnonymousType = true, Namespace = "urn:milestone-systems")]
public enum WinVersionTypeVersionsToExcludeProductType
{
	Server,
	Workstation,
	All
}
