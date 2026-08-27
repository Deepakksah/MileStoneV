using System.CodeDom.Compiler;
using System.Runtime.Serialization;

namespace VideoOS.Common.Proxy.Server.WCF;

[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "RetentionOptionType", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Common.Proxy.Server")]
public enum RetentionOptionType
{
	[EnumMember]
	Unused,
	[EnumMember]
	Days,
	[EnumMember]
	Weeks,
	[EnumMember]
	Months,
	[EnumMember]
	Years,
	[EnumMember]
	UserDefined,
	[EnumMember]
	Indefinite
}
