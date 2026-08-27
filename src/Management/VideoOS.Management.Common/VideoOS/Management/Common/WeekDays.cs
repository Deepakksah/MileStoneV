using System;

namespace VideoOS.Management.Common;

[Flags]
public enum WeekDays
{
	Monday = 1,
	Tuesday = 2,
	Wednesday = 4,
	Thursday = 8,
	Friday = 0x10,
	Saturday = 0x20,
	Sunday = 0x40,
	WeekDays = 0x1F,
	WeekEnds = 0x60,
	EveryDay = 0x7F
}
