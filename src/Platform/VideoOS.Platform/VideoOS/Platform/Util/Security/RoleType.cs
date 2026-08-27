using System;
using System.Runtime.Serialization;

namespace VideoOS.Platform.Util.Security;

[DataContract]
[Flags]
public enum RoleType
{
	[EnumMember]
	Adminstrative = 0,
	[EnumMember]
	UserDefined = 2
}
