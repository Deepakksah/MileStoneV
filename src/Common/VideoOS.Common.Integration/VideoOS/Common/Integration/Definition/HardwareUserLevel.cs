using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.Definition;

[Serializable]
public enum HardwareUserLevel
{
	[EnumMember]
	Unknown = -1,
	[EnumMember]
	Administrator,
	[EnumMember]
	Operator,
	[EnumMember]
	User,
	[EnumMember]
	Anonymous
}
