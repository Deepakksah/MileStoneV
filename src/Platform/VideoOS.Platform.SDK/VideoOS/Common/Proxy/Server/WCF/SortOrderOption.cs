using System.CodeDom.Compiler;
using System.Runtime.Serialization;

namespace VideoOS.Common.Proxy.Server.WCF;

[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "SortOrderOption", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Common.Proxy.Server")]
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
