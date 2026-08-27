using System.Runtime.Serialization;

namespace VideoOS.WebService.Common.ServerCommandService;

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
