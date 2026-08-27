using System.Runtime.Serialization;

namespace VideoOS.Management.Common;

[DataContract]
public enum PatternOccurrenceOfDayInMonthType
{
	[EnumMember]
	None,
	[EnumMember]
	First,
	[EnumMember]
	Second,
	[EnumMember]
	Third,
	[EnumMember]
	Fourth,
	[EnumMember]
	Last
}
