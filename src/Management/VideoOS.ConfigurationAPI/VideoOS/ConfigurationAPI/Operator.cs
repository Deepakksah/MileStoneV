using System.Runtime.Serialization;

namespace VideoOS.ConfigurationAPI;

[DataContract]
public enum Operator
{
	[EnumMember]
	Equals,
	[EnumMember]
	NotEquals,
	[EnumMember]
	LessThan,
	[EnumMember]
	GreaterThan,
	[EnumMember]
	Contains,
	[EnumMember]
	BeginsWith
}
