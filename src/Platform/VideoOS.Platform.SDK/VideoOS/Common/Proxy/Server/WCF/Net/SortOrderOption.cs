using System.CodeDom.Compiler;
using System.Runtime.Serialization;

namespace VideoOS.Common.Proxy.Server.WCF.Net;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DataContract(Name = "SortOrderOption", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.WebService.Common.ServerCommandService")]
public enum SortOrderOption
{
	[EnumMember]
	Header,
	[EnumMember]
	Description,
	[EnumMember]
	StartTime,
	[EnumMember]
	TagTime,
	[EnumMember]
	EndTime,
	[EnumMember]
	CreateTime,
	[EnumMember]
	RetentionExpireTime,
	[EnumMember]
	UserName,
	[EnumMember]
	Size
}
