using System.Runtime.Serialization;

namespace VideoOS.Management.Common;

[DataContract]
public enum RangeLimit
{
	[EnumMember]
	LimitByDate = 2,
	[EnumMember]
	LimitByNumberOfOccurrences = 1,
	[EnumMember]
	NoLimit = 0
}
