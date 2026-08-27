using System;
using System.CodeDom.Compiler;
using System.Xml.Serialization;

namespace VideoOS.Common.LogService;

[Serializable]
[GeneratedCode("wsdl", "2.0.50727.3038")]
[XmlType(Namespace = "http://videoos.net/2/XProtectLogService")]
public enum Severity
{
	Error,
	Warning,
	Info,
	Debug
}
