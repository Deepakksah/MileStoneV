using System.Runtime.Serialization;

namespace VideoOS.Management.Common;

[DataContract]
public enum PatternType
{
	[EnumMember]
	Calculated = 1,
	[EnumMember]
	Explicit = 0
}
