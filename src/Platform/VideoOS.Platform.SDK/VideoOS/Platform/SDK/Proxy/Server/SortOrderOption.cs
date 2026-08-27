using System;
using System.CodeDom.Compiler;
using System.Xml.Serialization;

namespace VideoOS.Platform.SDK.Proxy.Server;

[Serializable]
[GeneratedCode("wsdl", "4.0.30319.17929")]
[XmlType(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public enum SortOrderOption
{
	Header,
	Description,
	StartTime,
	TagTime,
	EndTime,
	CreateTime,
	RetentionExpireTime,
	UserName,
	Size
}
