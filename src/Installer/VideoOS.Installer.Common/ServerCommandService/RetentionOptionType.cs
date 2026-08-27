using System.CodeDom.Compiler;
using System.Runtime.Serialization;

namespace ServerCommandService;

[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "RetentionOptionType", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.WebService.Common.ServerCommandService")]
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
