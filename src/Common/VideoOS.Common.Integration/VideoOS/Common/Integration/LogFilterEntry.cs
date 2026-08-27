using System.Runtime.Serialization;

namespace VideoOS.Common.Integration;

[DataContract]
public enum LogFilterEntry
{
	[EnumMember]
	Level,
	[EnumMember]
	SourceName,
	[EnumMember]
	ResourceName,
	[EnumMember]
	ResourceType,
	[EnumMember]
	ResourceHost,
	[EnumMember]
	EventName,
	[EnumMember]
	ServiceName,
	[EnumMember]
	UserName,
	[EnumMember]
	AuditCategory,
	[EnumMember]
	AuditId,
	[EnumMember]
	Permission,
	[EnumMember]
	Location,
	[EnumMember]
	RuleName
}
