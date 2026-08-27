using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.Common;

[DataContract]
public class AppointmentRecurrenceDefinition
{
	[DataMember]
	public Guid RecurrenceId { get; set; }

	[DataMember]
	public string Description { get; set; }

	[DataMember]
	public TimeSpan OccurrenceDuration { get; set; }

	[DataMember]
	public DateTime OccurrenceStartTime { get; set; }

	[DataMember]
	public int PatternDayOfMonth { get; set; }

	[DataMember]
	public int PatternDaysOfWeek { get; set; }

	[DataMember]
	public RecurrencePatternFrequencyType PatternFrequency { get; set; }

	[DataMember]
	public int PatternMonthOfYear { get; set; }

	[DataMember]
	public PatternOccurrenceOfDayInMonthType PatternOccurrenceOfDayInMonth { get; set; }

	[DataMember]
	public int PatternInterval { get; set; }

	[DataMember]
	public PatternType PatternType { get; set; }

	[DataMember]
	public DateTime RangeEndDate { get; set; }

	[DataMember]
	public DateTime RangeStartDate { get; set; }

	[DataMember]
	public RangeLimit RangeLimit { get; set; }

	[DataMember]
	public int RangeMaxOccurrences { get; set; }

	[DataMember]
	public TimeSpan Duration { get; set; }
}
