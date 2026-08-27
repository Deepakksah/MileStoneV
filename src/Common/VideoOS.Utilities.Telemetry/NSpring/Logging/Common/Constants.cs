using System;

namespace NSpring.Logging.Common;

public sealed class Constants
{
	public static readonly string ISO_8601_TIME_ZONE;

	public static readonly string ISO_8601_DAYLIGHT_SAVINGS_TIME_ZONE;

	public static readonly Version VERSION;

	private static string[] dateStrings;

	private static string[] millisecondStrings;

	private static string[] yearStrings;

	private static string[] monthNames;

	private static string[] monthShortNames;

	private static string[] weekdayNames;

	private static string[] weekdayShortNames;

	public static string[] MonthNames
	{
		get
		{
			string[] array = monthNames;
			string[] array2 = new string[array.Length];
			Array.Copy(array, 0, array2, 0, array2.Length);
			return array2;
		}
	}

	public static string[] MonthShortNames
	{
		get
		{
			string[] array = monthShortNames;
			string[] array2 = new string[array.Length];
			Array.Copy(array, 0, array2, 0, array2.Length);
			return array2;
		}
	}

	public static string[] WeekdayNames
	{
		get
		{
			string[] array = weekdayNames;
			string[] array2 = new string[array.Length];
			Array.Copy(array, 0, array2, 0, array2.Length);
			return array2;
		}
	}

	public static string[] WeekdayShortNames
	{
		get
		{
			string[] array = weekdayShortNames;
			string[] array2 = new string[array.Length];
			Array.Copy(array, 0, array2, 0, array2.Length);
			return array2;
		}
	}

	public static string[] DateStrings => dateStrings;

	public static string[] MillisecondStrings => millisecondStrings;

	public static string[] YearStrings => yearStrings;

	static Constants()
	{
		ISO_8601_TIME_ZONE = new DateTime(2000, 7, 1).ToString("zzz");
		ISO_8601_DAYLIGHT_SAVINGS_TIME_ZONE = new DateTime(2000, 1, 1).ToString("zzz");
		VERSION = new Version(1, 1);
		dateStrings = new string[61];
		millisecondStrings = new string[1000];
		yearStrings = new string[2101];
		monthNames = new string[13]
		{
			"", "January", "February", "March", "April", "May", "June", "July", "August", "September",
			"October", "November", "December"
		};
		monthShortNames = new string[13]
		{
			"", "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep",
			"Oct", "Nov", "Dec"
		};
		weekdayNames = new string[7] { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
		weekdayShortNames = new string[7] { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };
		string text = "00";
		for (int i = 0; i < 10; i++)
		{
			millisecondStrings[i] = text + i;
		}
		text = "0";
		for (int i = 10; i < 100; i++)
		{
			millisecondStrings[i] = text + i;
		}
		for (int i = 100; i < 1000; i++)
		{
			millisecondStrings[i] = i.ToString();
		}
		text = "0";
		for (int i = 0; i < 10; i++)
		{
			dateStrings[i] = text + i;
		}
		for (int i = 10; i <= 60; i++)
		{
			dateStrings[i] = i.ToString();
		}
		for (int i = 1900; i <= 2100; i++)
		{
			yearStrings[i] = i.ToString();
		}
	}

	private Constants()
	{
	}
}
