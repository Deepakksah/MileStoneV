using System;
using System.CodeDom.Compiler;
using System.Xml.Serialization;

namespace VideoOS.Platform.SDK.Proxy.Status;

[Serializable]
[GeneratedCode("wsdl", "2.0.50727.1432")]
[XmlType(Namespace = "http://videoos.net/2/XProtectCSRecorderStatus")]
public enum IOState
{
	Activated,
	Deactivated,
	NotAvailable
}
