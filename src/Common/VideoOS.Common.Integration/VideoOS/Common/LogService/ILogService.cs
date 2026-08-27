using System.CodeDom.Compiler;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace VideoOS.Common.LogService;

[GeneratedCode("wsdl", "2.0.50727.3038")]
[WebServiceBinding(Name = "LogService", Namespace = "http://videoos.net/2/XProtectLogService")]
public interface ILogService
{
	[WebMethod]
	[SoapDocumentMethod("", RequestNamespace = "http://videoos.net/2/XProtectLogService", OneWay = false, Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	void LogWrite([XmlElement("entries", Form = XmlSchemaForm.Unqualified)] LogEntry[] entries);
}
