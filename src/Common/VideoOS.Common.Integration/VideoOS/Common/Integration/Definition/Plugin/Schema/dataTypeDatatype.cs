using System;
using System.CodeDom.Compiler;
using System.Xml.Serialization;

namespace VideoOS.Common.Integration.Definition.Plugin.Schema;

[Serializable]
[GeneratedCode("xsd", "4.6.1055.0")]
[XmlType(AnonymousType = true, Namespace = "http://VideoOS.net/Surveillance/XmlSchema")]
public enum dataTypeDatatype
{
	boolean,
	@int,
	dateTime,
	@float,
	@double,
	@string,
	base64binary
}
