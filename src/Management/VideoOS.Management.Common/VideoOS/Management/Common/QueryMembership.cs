using System.Runtime.Serialization;

namespace VideoOS.Management.Common;

[DataContract]
public enum QueryMembership
{
	[EnumMember]
	None,
	[EnumMember]
	Direct,
	[EnumMember]
	Expanded
}
