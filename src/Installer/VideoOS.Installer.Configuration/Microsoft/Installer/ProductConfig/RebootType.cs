using System;
using System.CodeDom.Compiler;
using System.Xml.Serialization;

namespace Microsoft.Installer.ProductConfig;

[Serializable]
[GeneratedCode("xsd", "4.7.2053.0")]
[XmlType(Namespace = "http://schemas.microsoft.com/developer/2004/01/bootstrapper")]
public enum RebootType
{
	Defer,
	Immediate,
	Force,
	None
}
