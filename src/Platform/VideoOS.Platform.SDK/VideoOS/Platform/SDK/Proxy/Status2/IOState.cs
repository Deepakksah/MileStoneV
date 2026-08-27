using System;
using System.CodeDom.Compiler;
using System.Xml.Serialization;

namespace VideoOS.Platform.SDK.Proxy.Status2;

[Serializable]
[GeneratedCode("wsdl", "4.0.30319.17929")]
[XmlType(Namespace = "http://videoos.net/2/XProtectCSRecorderStatus2")]
public enum IOState
{
	Activated,
	Deactivated,
	NotAvailable
}
