using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace VideoOS.Common.SharedTimeProfile.AppointmentCalendar;

public class AppointmentRecurrence
{
	private int _patternInterval;

	public Guid Id { get; }

	public TimeSpan OccurrenceDuration { get; set; }

	public DateTime OccurrenceStartTime { get; set; }

	public int RangeMaxOccurrences { get; set; } = 10;

	public RecurrenceRangeLimit RangeLimit{ get; set; }

	public int PatternDayOfMonth { get; set; }

	public RecurrencePatternDaysOfWeek PatternDaysOfWeek{ get; set; }

	public RecurrencePatternFrequency PatternFrequency{ get; set; } = RecurrencePatternFrequency.Weekly;

	public int PatternInterval
	{
		get
		{
			return _patternInterval;
		}
		set
		{
			if (value < 1 || value > 99)
			{
				throw new ArgumentOutOfRangeException("PatternInterval", "Value must be between 1 and 99.");
			}
			_patternInterval = value;
		}
	}

	public int PatternMonthOfYear { get; set; }

	public RecurrencePatternOccurrenceOfDayInMonth PatternOccurrenceOfDayInMonth{ get; set; }

	public RecurrencePatternType PatternType{ get; set; }

	public DateTime RangeStartDate { get; set; }

	public DateTime RangeEndDate { get; set; }

	public string Description => GetRecurrencePatternString() + " " + GetDateRangeString() + " " + GetTimeRangeString();

	public AppointmentRecurrence()
		: this(Guid.NewGuid())
	{
	}

	public AppointmentRecurrence(Guid id)
	{
		_patternInterval = 1;
		Id = id;
	}

	public IReadOnlyCollection<Appointment> GetAppointmentsInRange(DateTime rangeStart, DateTime rangeEnd)
	{
		return GetAppointmentsInRange(rangeStart, rangeEnd, null);
	}

	public IReadOnlyCollection<Appointment> GetAppointmentsInRange(DateTime rangeStart, DateTime rangeEnd, Appointment rootAppointment)
	{
		DateTime firstOccurrence = GetFirstOccurrence();
		DateTime endOfLastOccurrence = GetEndOfLastOccurrence(rangeEnd);
		if (rangeEnd < firstOccurrence || rangeStart > endOfLastOccurrence)
		{
			return new List<Appointment>();
		}
		if (PatternFrequency == RecurrencePatternFrequency.Daily)
		{
			return GetDailyAppointmentsInRange(rangeStart, rangeEnd, rootAppointment);
		}
		if (PatternFrequency == RecurrencePatternFrequency.Weekly)
		{
			return GetWeeklyAppointmentsInRange(rangeStart, rangeEnd, rootAppointment);
		}
		if (PatternFrequency == RecurrencePatternFrequency.Monthly)
		{
			return GetMonthlyAppointmentsInRange(rangeStart, rangeEnd, rootAppointment);
		}
		if (PatternFrequency == RecurrencePatternFrequency.Yearly)
		{
			return GetMonthlyAppointmentsInRange(rangeStart, rangeEnd, rootAppointment);
		}
		return new List<Appointment>();
	}

	private DateTime GetFirstOccurrence()
	{
		switch (PatternFrequency)
		{
		case RecurrencePatternFrequency.Daily:
		case RecurrencePatternFrequency.Weekly:
			return OccurrenceStartTime;
		case RecurrencePatternFrequency.Monthly:
			return GetFirstMonthlyOccurrence();
		case RecurrencePatternFrequency.Yearly:
			return GetFirstYearlyOccurrence();
		default:
			throw new NotImplementedException($"Recurrence frequency '{PatternFrequency}' is not implemented.");
		}
	}

	private DateTime GetFirstMonthlyOccurrence()
	{
		DateTime dateTime = CalculateDateTimeForOccurrenceOfDayInMonth(OccurrenceStartTime.Year, OccurrenceStartTime.Month);
		if (dateTime < OccurrenceStartTime)
		{
			DateTime dateTime2 = new DateTime(OccurrenceStartTime.Year, OccurrenceStartTime.Month, 1).AddMonths(1);
			dateTime = CalculateDateTimeForOccurrenceOfDayInMonth(dateTime2.Year, dateTime2.Month);
		}
		return dateTime;
	}

	private DateTime GetFirstYearlyOccurrence()
	{
		DateTime dateTime = CalculateDateTimeForOccurrenceOfDayInMonth(OccurrenceStartTime.Year, PatternMonthOfYear);
		if (dateTime < OccurrenceStartTime)
		{
			dateTime = CalculateDateTimeForOccurrenceOfDayInMonth(OccurrenceStartTime.Year + 1, PatternMonthOfYear);
		}
		return dateTime;
	}

	private DateTime GetEndOfLastOccurrence(DateTime defaultDateTime)
	{
		return GetLastOccurrence(defaultDateTime).Add(OccurrenceDuration);
	}

	private DateTime GetLastOccurrence(DateTime defaultDateTime)
	{
		if (RangeLimit == RecurrenceRangeLimit.LimitByNumberOfOccurrences)
		{
			return PatternFrequency switch
			{
				RecurrencePatternFrequency.Daily => GetLastDailyOccurrence(), 
				RecurrencePatternFrequency.Weekly => GetLastWeeklyOccurrence(), 
				RecurrencePatternFrequency.Monthly => GetLastMonthlyOccurrence(), 
				RecurrencePatternFrequency.Yearly => GetLastYearlyOccurrence(), 
				_ => throw new NotImplementedException($"Recurrence frequency '{PatternFrequency}' is not implemented."), 
			};
		}
		if (RangeLimit == RecurrenceRangeLimit.LimitByDate)
		{
			return RangeEndDate + OccurrenceStartTime.TimeOfDay;
		}
		return defaultDateTime;
	}

	private DateTime GetLastDailyOccurrence()
	{
		DateTime result = OccurrenceStartTime;
		int num = 1;
		while (num < RangeMaxOccurrences)
		{
			if (CheckDayOfWeek(result.DayOfWeek))
			{
				num++;
			}
			result = result.AddDays(_patternInterval);
		}
		return result;
	}

	private DateTime GetLastWeeklyOccurrence()
	{
		int patternInterval = _patternInterval;
		int num = 0;
		int num2 = 0;
		DateTime result = OccurrenceStartTime;
		num = (int)OccurrenceStartTime.DayOfWeek;
		int num3 = 0;
		while (num3 < RangeMaxOccurrences)
		{
			if (num2 % patternInterval == 0 && CheckDayOfWeek(result.DayOfWeek))
			{
				num3++;
			}
			num++;
			if (num % 7 == 0)
			{
				num = 0;
				num2++;
			}
			result = result.AddDays(1.0);
		}
		return result;
	}

	private DateTime GetLastMonthlyOccurrence()
	{
		DateTime firstMonthlyOccurrence = GetFirstMonthlyOccurrence();
		int months = _patternInterval * (RangeMaxOccurrences - 1);
		DateTime dateTime = firstMonthlyOccurrence.AddMonths(months);
		return CalculateDateTimeForOccurrenceOfDayInMonth(dateTime.Year, dateTime.Month);
	}

	private DateTime GetLastYearlyOccurrence()
	{
		DateTime dateTime = GetFirstYearlyOccurrence().AddYears(RangeMaxOccurrences - 1);
		return CalculateDateTimeForOccurrenceOfDayInMonth(dateTime.Year, dateTime.Month);
	}

	private DateTime CalculateDateTimeForOccurrenceOfDayInMonth(int year, int month)
	{
		DateTime dateTime = DateTime.MinValue;
		if (PatternType == RecurrencePatternType.Explicit)
		{
			int day = Math.Min(DateTime.DaysInMonth(year, month), PatternDayOfMonth);
			dateTime = new DateTime(year, month, day);
		}
		else if (PatternType == RecurrencePatternType.Calculated)
		{
			if (PatternOccurrenceOfDayInMonth == RecurrencePatternOccurrenceOfDayInMonth.Last)
			{
				dateTime = new DateTime(year, month, DateTime.DaysInMonth(year, month));
				while (!CheckDayOfWeek(dateTime.DayOfWeek))
				{
					dateTime = dateTime.AddDays(-1.0);
				}
			}
			else
			{
				dateTime = new DateTime(year, month, 1);
				int num = 0;
				int patternOccurrenceOfDayInMonth = (int)PatternOccurrenceOfDayInMonth;
				while (true)
				{
					if (CheckDayOfWeek(dateTime.DayOfWeek))
					{
						num++;
					}
					if (num >= patternOccurrenceOfDayInMonth)
					{
						break;
					}
					dateTime = dateTime.AddDays(1.0);
				}
			}
		}
		return dateTime.Add(OccurrenceStartTime.TimeOfDay);
	}

	private List<Appointment> GetDailyAppointmentsInRange(DateTime rangeStart, DateTime rangeEnd, Appointment rootAppointment)
	{
		List<Appointment> list = new List<Appointment>();
		int patternInterval = _patternInterval;
		DateTime endOfLastOccurrence = GetEndOfLastOccurrence(rangeEnd);
		DateTime dateTime = OccurrenceStartTime;
		DateTime dateTime2 = dateTime + OccurrenceDuration;
		while (dateTime2 < rangeStart && dateTime <= endOfLastOccurrence)
		{
			dateTime = dateTime.AddDays(patternInterval);
			dateTime2 = dateTime + OccurrenceDuration;
		}
		while (dateTime2 >= rangeStart && dateTime <= endOfLastOccurrence && dateTime <= rangeEnd)
		{
			if (CheckDayOfWeek(dateTime.DayOfWeek))
			{
				Appointment item = new Appointment(dateTime, dateTime2)
				{
					Subject = rootAppointment?.Subject,
					AllDayEvent = false
				};
				list.Add(item);
			}
			dateTime = dateTime.AddDays(patternInterval);
			dateTime2 = dateTime2.AddDays(patternInterval);
		}
		return list;
	}

	private List<Appointment> GetWeeklyAppointmentsInRange(DateTime rangeStart, DateTime rangeEnd, Appointment rootAppointment)
	{
		List<Appointment> list = new List<Appointment>();
		int patternInterval = _patternInterval;
		DateTime endOfLastOccurrence = GetEndOfLastOccurrence(rangeEnd);
		int days = (rangeStart.Date - OccurrenceStartTime.Date).Days;
		int num = (int)rangeStart.Date.DayOfWeek;
		int num2 = (days + num) / 7;
		DateTime dateTime = OccurrenceStartTime.AddDays(days);
		DateTime dateTime2 = dateTime + OccurrenceDuration;
		while (dateTime2 >= rangeStart && dateTime <= endOfLastOccurrence && dateTime <= rangeEnd)
		{
			if (num2 % patternInterval == 0 && CheckDayOfWeek(dateTime.DayOfWeek))
			{
				Appointment item = new Appointment(dateTime, dateTime2)
				{
					Subject = rootAppointment?.Subject,
					AllDayEvent = false
				};
				list.Add(item);
			}
			num++;
			if (num % 7 == 0)
			{
				num = 0;
				num2++;
			}
			dateTime = dateTime.AddDays(1.0);
			dateTime2 = dateTime2.AddDays(1.0);
		}
		return list;
	}

	private List<Appointment> GetMonthlyAppointmentsInRange(DateTime rangeStart, DateTime rangeEnd, Appointment rootAppointment)
	{
		List<Appointment> list = new List<Appointment>();
		int months = ((PatternFrequency == RecurrencePatternFrequency.Yearly) ? 12 : _patternInterval);
		DateTime endOfLastOccurrence = GetEndOfLastOccurrence(rangeEnd);
		DateTime dateTime = GetFirstOccurrence();
		DateTime dateTime2 = dateTime + OccurrenceDuration;
		while (dateTime2 < rangeStart && dateTime <= endOfLastOccurrence)
		{
			DateTime dateTime3 = dateTime.AddMonths(months);
			dateTime = CalculateDateTimeForOccurrenceOfDayInMonth(dateTime3.Year, dateTime3.Month);
			dateTime2 = dateTime + OccurrenceDuration;
		}
		while (dateTime2 >= rangeStart && dateTime <= endOfLastOccurrence && dateTime <= rangeEnd)
		{
			Appointment item = new Appointment(dateTime, dateTime2)
			{
				Subject = rootAppointment?.Subject,
				AllDayEvent = false
			};
			list.Add(item);
			DateTime dateTime4 = dateTime.AddMonths(months);
			dateTime = CalculateDateTimeForOccurrenceOfDayInMonth(dateTime4.Year, dateTime4.Month);
			dateTime2 = dateTime + OccurrenceDuration;
		}
		return list;
	}

	private bool CheckDayOfWeek(DayOfWeek dayOfWeek)
	{
		if (PatternDaysOfWeek == RecurrencePatternDaysOfWeek.None)
		{
			return false;
		}
		return ((uint)PatternDaysOfWeek & (uint)(1 << (int)dayOfWeek)) != 0;
	}

	private string GetRecurrencePatternString()
	{
		switch (PatternFrequency)
		{
		case RecurrencePatternFrequency.Daily:
			if (PatternDaysOfWeek == RecurrencePatternDaysOfWeek.All)
			{
				return $"Occurs every {PatternInterval} day(s)";
			}
			return "Occurs every " + GetDaysOfWeekString(PatternDaysOfWeek);
		case RecurrencePatternFrequency.Weekly:
			return $"Occurs every {PatternInterval} week(s) on {GetDaysOfWeekString(PatternDaysOfWeek)}";
		case RecurrencePatternFrequency.Monthly:
			if (PatternType == RecurrencePatternType.Explicit)
			{
				return $"Occurs day {PatternDayOfMonth} of every {PatternInterval} month(s)";
			}
			return $"Occurs the {PatternOccurrenceOfDayInMonth} {GetDaysOfWeekString(PatternDaysOfWeek)} of every {PatternInterval} month(s)";
		default:
		{
			string text = DateTimeFormatInfo.InvariantInfo.MonthNames[PatternMonthOfYear - 1];
			if (PatternType == RecurrencePatternType.Explicit)
			{
				return $"Occurs every {text} {PatternDayOfMonth}";
			}
			return $"Occurs the {PatternOccurrenceOfDayInMonth} {GetDaysOfWeekString(PatternDaysOfWeek)} of {text}";
		}
		}
	}

	private string GetDateRangeString()
	{
		return RangeLimit switch
		{
			RecurrenceRangeLimit.LimitByNumberOfOccurrences => $"effective {RangeStartDate:d} for {RangeMaxOccurrences} occurrences", 
			RecurrenceRangeLimit.LimitByDate => $"effective {RangeStartDate:d} until {RangeEndDate:d}", 
			_ => $"effective {RangeStartDate:d}", 
		};
	}

	private string GetTimeRangeString()
	{
		if (OccurrenceDuration < TimeSpan.FromDays(1.0))
		{
			return $"from {OccurrenceStartTime:t} to {OccurrenceStartTime + OccurrenceDuration:t}";
		}
		if (OccurrenceDuration.Minutes != 0)
		{
			return $"from {OccurrenceStartTime:t} for {OccurrenceDuration.TotalMinutes} minutes";
		}
		if (OccurrenceDuration.Hours != 0)
		{
			return $"from {OccurrenceStartTime:t} for {OccurrenceDuration.TotalHours} hours";
		}
		return $"from {OccurrenceStartTime:t} for {OccurrenceDuration.TotalDays} days";
	}

	private static string GetDaysOfWeekString(RecurrencePatternDaysOfWeek patternDaysOfWeek)
	{
		switch (patternDaysOfWeek)
		{
		case RecurrencePatternDaysOfWeek.None:
			return "no days";
		case RecurrencePatternDaysOfWeek.All:
			return "all days";
		case RecurrencePatternDaysOfWeek.AllWeekdays:
			return "weekdays";
		default:
		{
			RecurrencePatternDaysOfWeek[] array = new RecurrencePatternDaysOfWeek[7]
			{
				RecurrencePatternDaysOfWeek.Monday,
				RecurrencePatternDaysOfWeek.Tuesday,
				RecurrencePatternDaysOfWeek.Wednesday,
				RecurrencePatternDaysOfWeek.Thursday,
				RecurrencePatternDaysOfWeek.Friday,
				RecurrencePatternDaysOfWeek.Saturday,
				RecurrencePatternDaysOfWeek.Sunday
			}.Where((RecurrencePatternDaysOfWeek day) => patternDaysOfWeek.HasFlag(day)).ToArray();
			if (array.Length != 1)
			{
				return string.Format("{0} and {1}", string.Join(", ", array.Take(array.Length - 1)), array.Last());
			}
			return $"{array[0]}";
		}
		}
	}
}
