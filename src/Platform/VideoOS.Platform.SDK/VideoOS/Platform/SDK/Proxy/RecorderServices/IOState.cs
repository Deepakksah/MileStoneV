using System;
using System.CodeDom.Compiler;
using System.Xml.Serialization;

namespace VideoOS.Platform.SDK.Proxy.RecorderServices;

[Serializable]
[GeneratedCode("wsdl", "4.0.30319.33440")]
[XmlType(Namespace = "http://videoos.net/2/XProtectCSRecorderCommand")]
public enum IOState
{
	Unknown,
	Activated,
	Deactivated
}
