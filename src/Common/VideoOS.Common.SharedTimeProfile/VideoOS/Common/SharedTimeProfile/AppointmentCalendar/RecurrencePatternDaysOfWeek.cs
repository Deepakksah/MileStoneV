using System;

namespace VideoOS.Common.SharedTimeProfile.AppointmentCalendar;

[Flags]
public enum RecurrencePatternDaysOfWeek
{
	None = 0,
	Sunday = 1,
	Monday = 2,
	Tuesday = 4,
	Wednesday = 8,
	Thursday = 0x10,
	Friday = 0x20,
	Saturday = 0x40,
	AllWeekendDays = 0x41,
	AllWeekdays = 0x3E,
	All = 0x7F
}
