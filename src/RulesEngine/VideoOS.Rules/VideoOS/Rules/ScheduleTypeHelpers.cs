using System;
using VideoOS.Rules.Interfaces;

namespace VideoOS.Rules;

public class ScheduleTypeHelpers
{
	public static string GetDayString(int weekday)
	{
		return weekday switch
		{
			0 => "monday", 
			1 => "tuesday", 
			2 => "wednesday", 
			3 => "thursday", 
			4 => "friday", 
			5 => "saturday", 
			6 => "sunday", 
			_ => "undefined", 
		};
	}

	public static string GetFrequencyTypeString(ScheduleTypes.FrequencyType frequencyType)
	{
		return frequencyType switch
		{
			ScheduleTypes.FrequencyType.Daily => "daily", 
			ScheduleTypes.FrequencyType.Weekly => "weekly", 
			ScheduleTypes.FrequencyType.Monthly => "monthly", 
			_ => "undefined", 
		};
	}

	public static string GetMonthlyTypeString(ScheduleTypes.MonthlyType monthlyType)
	{
		return monthlyType switch
		{
			ScheduleTypes.MonthlyType.AbsoluteDay => "absoluteday", 
			ScheduleTypes.MonthlyType.RelativeDay => "relativeday", 
			_ => "undefined", 
		};
	}

	public static string GetSubDayTypeString(ScheduleTypes.SubdayType subdayType)
	{
		return subdayType switch
		{
			ScheduleTypes.SubdayType.Once => "once", 
			ScheduleTypes.SubdayType.Recurring => "recurring", 
			_ => "undefined", 
		};
	}

	public static string GetMonthlyRelativeOffsetString(ScheduleTypes.MonthlyRelativeOffset monthlyRelativeOffset)
	{
		return monthlyRelativeOffset switch
		{
			ScheduleTypes.MonthlyRelativeOffset.First => "first", 
			ScheduleTypes.MonthlyRelativeOffset.Second => "second", 
			ScheduleTypes.MonthlyRelativeOffset.Third => "third", 
			ScheduleTypes.MonthlyRelativeOffset.Fourth => "fourth", 
			ScheduleTypes.MonthlyRelativeOffset.Last => "last", 
			_ => "undefined", 
		};
	}

	public static string GetMonthlyRelativeDayString(ScheduleTypes.MonthlyRelativeDay monthlyRelativeDay)
	{
		return monthlyRelativeDay switch
		{
			ScheduleTypes.MonthlyRelativeDay.Monday => "monday", 
			ScheduleTypes.MonthlyRelativeDay.Tuesday => "tuesday", 
			ScheduleTypes.MonthlyRelativeDay.Wednesday => "wednesday", 
			ScheduleTypes.MonthlyRelativeDay.Thursday => "thursday", 
			ScheduleTypes.MonthlyRelativeDay.Friday => "friday", 
			ScheduleTypes.MonthlyRelativeDay.Saturday => "saturday", 
			ScheduleTypes.MonthlyRelativeDay.Sunday => "sunday", 
			ScheduleTypes.MonthlyRelativeDay.Day => "day", 
			ScheduleTypes.MonthlyRelativeDay.WeekDay => "weekday", 
			ScheduleTypes.MonthlyRelativeDay.WeekendDay => "weekendday", 
			_ => "undefined", 
		};
	}

	public static ScheduleTypes.FrequencyType GetFrequencyTypeEnum(string frequencyType)
	{
		return frequencyType switch
		{
			"daily" => ScheduleTypes.FrequencyType.Daily, 
			"weekly" => ScheduleTypes.FrequencyType.Weekly, 
			"monthly" => ScheduleTypes.FrequencyType.Monthly, 
			_ => ScheduleTypes.FrequencyType.Undefined, 
		};
	}

	public static ScheduleTypes.MonthlyType GetMonthlyTypeEnum(string monthlyType)
	{
		if (monthlyType == "absoluteday")
		{
			return ScheduleTypes.MonthlyType.AbsoluteDay;
		}
		if (monthlyType == "relativeday")
		{
			return ScheduleTypes.MonthlyType.RelativeDay;
		}
		return ScheduleTypes.MonthlyType.Undefined;
	}

	public static ScheduleTypes.SubdayType GetSubDayTypeEnum(string subdayType)
	{
		if (subdayType == "once")
		{
			return ScheduleTypes.SubdayType.Once;
		}
		if (subdayType == "recurring")
		{
			return ScheduleTypes.SubdayType.Recurring;
		}
		return ScheduleTypes.SubdayType.Undefined;
	}

	public static ScheduleTypes.MonthlyRelativeOffset GetMonthlyRelativeOffsetEnum(string monthlyRelativeOffset)
	{
		return monthlyRelativeOffset switch
		{
			"first" => ScheduleTypes.MonthlyRelativeOffset.First, 
			"second" => ScheduleTypes.MonthlyRelativeOffset.Second, 
			"third" => ScheduleTypes.MonthlyRelativeOffset.Third, 
			"fourth" => ScheduleTypes.MonthlyRelativeOffset.Fourth, 
			"last" => ScheduleTypes.MonthlyRelativeOffset.Last, 
			_ => ScheduleTypes.MonthlyRelativeOffset.Undefined, 
		};
	}

	public static ScheduleTypes.MonthlyRelativeDay GetMonthlyRelativeDayEnum(string monthlyRelativeDay)
	{
		return monthlyRelativeDay switch
		{
			"monday" => ScheduleTypes.MonthlyRelativeDay.Monday, 
			"tuesday" => ScheduleTypes.MonthlyRelativeDay.Tuesday, 
			"wednesday" => ScheduleTypes.MonthlyRelativeDay.Wednesday, 
			"thursday" => ScheduleTypes.MonthlyRelativeDay.Thursday, 
			"friday" => ScheduleTypes.MonthlyRelativeDay.Friday, 
			"saturday" => ScheduleTypes.MonthlyRelativeDay.Saturday, 
			"sunday" => ScheduleTypes.MonthlyRelativeDay.Sunday, 
			"day" => ScheduleTypes.MonthlyRelativeDay.Day, 
			"weekday" => ScheduleTypes.MonthlyRelativeDay.WeekDay, 
			"weekendday" => ScheduleTypes.MonthlyRelativeDay.WeekendDay, 
			_ => ScheduleTypes.MonthlyRelativeDay.Undefined, 
		};
	}

	public static bool IsDayOfType(int weekday, ScheduleTypes.MonthlyRelativeDay relativeDay)
	{
		switch (relativeDay)
		{
		case ScheduleTypes.MonthlyRelativeDay.Monday:
			return weekday == 0;
		case ScheduleTypes.MonthlyRelativeDay.Tuesday:
			return weekday == 1;
		case ScheduleTypes.MonthlyRelativeDay.Wednesday:
			return weekday == 2;
		case ScheduleTypes.MonthlyRelativeDay.Thursday:
			return weekday == 3;
		case ScheduleTypes.MonthlyRelativeDay.Friday:
			return weekday == 4;
		case ScheduleTypes.MonthlyRelativeDay.Saturday:
			return weekday == 5;
		case ScheduleTypes.MonthlyRelativeDay.Sunday:
			return weekday == 6;
		case ScheduleTypes.MonthlyRelativeDay.Day:
			return true;
		case ScheduleTypes.MonthlyRelativeDay.WeekDay:
			if (weekday >= 0)
			{
				return weekday <= 4;
			}
			return false;
		case ScheduleTypes.MonthlyRelativeDay.WeekendDay:
			if (weekday != 5)
			{
				return weekday == 6;
			}
			return true;
		default:
			return false;
		}
	}

	public static int GetWeekDay(DateTime time)
	{
		int num = (int)(time.DayOfWeek - 1);
		if (num < 0)
		{
			num = 6;
		}
		return num;
	}

	public static DateTime SetTimeOfDayAndAccountForDST(DateTime time, TimeSpan timeOfDay)
	{
		DateTime result = new DateTime(time.Year, time.Month, time.Day);
		result += timeOfDay;
		int hours = timeOfDay.Hours;
		int hour = result.Hour;
		if (hours != hour)
		{
			if (hours >= 2 && hours <= 3)
			{
				result = new DateTime(time.Year, time.Month, time.Day, 3, 0, 0);
			}
			else if ((hours + 1) % 24 == hour)
			{
				result -= TimeSpan.FromHours(1.0);
			}
			else
			{
				result += TimeSpan.FromHours(1.0);
			}
		}
		return result;
	}

	public static DateTime AddDaysAndAccountForDST(DateTime time, int days)
	{
		DateTime result = new DateTime(time.Year, time.Month, time.Day);
		if (days >= 0)
		{
			result += TimeSpan.FromDays(days);
		}
		else
		{
			result -= TimeSpan.FromDays(-days);
		}
		switch (result.Hour)
		{
		case 23:
			result += TimeSpan.FromHours(1.0);
			break;
		case 1:
			result -= TimeSpan.FromHours(1.0);
			break;
		}
		return result;
	}

	public static DateTime AddMonths(DateTime time, int months)
	{
		int num = time.Year + months / 12;
		int num2 = time.Month + months % 12;
		if (num2 > 12)
		{
			num2 -= 12;
			num++;
		}
		else if (num2 <= 0)
		{
			num2 += 12;
			num--;
		}
		return new DateTime(num, num2, 1);
	}
}
