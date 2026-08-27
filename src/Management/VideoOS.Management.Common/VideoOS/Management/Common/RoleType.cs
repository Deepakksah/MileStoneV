using System.Runtime.Serialization;

namespace VideoOS.Management.Common;

[DataContract]
public enum RoleType
{
	[EnumMember]
	Adminstrative = 0,
	[EnumMember]
	UserDefined = 2
}
