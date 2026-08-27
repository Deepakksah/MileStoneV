using System.Runtime.Serialization;

namespace VideoOS.Common.Integration;

[DataContract]
public enum LogFilterCondition
{
	[EnumMember]
	Equal,
	[EnumMember]
	GreaterThan,
	[EnumMember]
	GreaterThanOrEqual,
	[EnumMember]
	LessThan,
	[EnumMember]
	LessThanOrEqual,
	[EnumMember]
	NotEqual,
	[EnumMember]
	Like
}
