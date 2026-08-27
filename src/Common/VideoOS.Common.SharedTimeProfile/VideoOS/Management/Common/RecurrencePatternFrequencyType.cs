using System.Runtime.Serialization;

namespace VideoOS.Management.Common;

[DataContract]
public enum RecurrencePatternFrequencyType
{
	[EnumMember]
	Daily,
	[EnumMember]
	Weekly,
	[EnumMember]
	Monthly,
	[EnumMember]
	Yearly
}
