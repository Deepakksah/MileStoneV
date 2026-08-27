using System;
using System.CodeDom.Compiler;
using System.Xml.Serialization;

namespace VideoOS.Installer.Configuration;

[Serializable]
[GeneratedCode("xsd", "4.7.2053.0")]
[XmlType(Namespace = "urn:milestone-systems")]
public enum TextType
{
	MoveConfig,
	UninstallOld,
	UninstallOldVersionDependent
}
