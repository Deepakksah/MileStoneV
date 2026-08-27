using System.Runtime.Serialization;

namespace VideoOS.WebService.Common.ServerCommandService;

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
