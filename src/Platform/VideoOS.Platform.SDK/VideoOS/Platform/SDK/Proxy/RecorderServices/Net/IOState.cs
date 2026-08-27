using System.CodeDom.Compiler;
using System.Xml.Serialization;

namespace VideoOS.Platform.SDK.Proxy.RecorderServices.Net;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[XmlType(Namespace = "http://videoos.net/2/XProtectCSRecorderCommand")]
public enum IOState
{
	Unknown,
	Activated,
	Deactivated
}
