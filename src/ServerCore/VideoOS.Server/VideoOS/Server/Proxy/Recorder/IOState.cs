using System;
using System.CodeDom.Compiler;
using System.Xml.Serialization;

namespace VideoOS.Server.Proxy.Recorder;

[Serializable]
[GeneratedCode("wsdl", "2.0.50727.42")]
[XmlType(Namespace = "http://videoos.net/2/XProtectCSRecorderCommand")]
public enum IOState
{
	Unknown,
	Activated,
	Deactivated
}
