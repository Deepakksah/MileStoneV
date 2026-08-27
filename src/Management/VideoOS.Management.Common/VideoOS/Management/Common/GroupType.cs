using System.Runtime.Serialization;

namespace VideoOS.Management.Common;

[DataContract]
public enum GroupType
{
	[EnumMember]
	FailoverGroupDefault,
	[EnumMember]
	FailoverGroupUserDefined
}
