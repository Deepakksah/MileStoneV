using System;
using System.Globalization;
using System.Text;

namespace NSpring.Logging.Common.Pattern;

public sealed class EventPropertyToken : Token
{
	private static string timeZone = Constants.ISO_8601_TIME_ZONE;

	private static string daylightSavingsTimeZone = Constants.ISO_8601_DAYLIGHT_SAVINGS_TIME_ZONE;

	private static string[] monthNames = Constants.MonthNames;

	private static string[] monthShortNames = Constants.MonthShortNames;

	private static string[] weekdayNames = Constants.WeekdayNames;

	private static string[] weekdayShortNames = Constants.WeekdayShortNames;

	private static string[] yearStrings = Constants.YearStrings;

	private static string[] dateStrings = Constants.DateStrings;

	private static string[] millisecondStrings = Constants.MillisecondStrings;

	private const char COLON_CHAR = ':';

	private const char HYPHEN_CHAR = '-';

	private const char PERIOD_CHAR = '.';

	private const char SPACE_CHAR = ' ';

	private const string BLANK_STRING = "";

	private const char SPACE = ' ';

	private Alignment alignment;

	private Case __case;

	private int minimumWidth;

	private int maximumWidth;

	private EventPropertyType propertyType;

	public EventPropertyType PropertyType => propertyType;

	public Alignment Alignment => alignment;

	public Case Case => __case;

	public int MinimumWidth => minimumWidth;

	public int MaximumWidth => maximumWidth;

	private EventPropertyToken()
	{
	}

	public EventPropertyToken(EventPropertyType _propertyType, string _text, int _minimumWidth, int _maximumWidth, Alignment _alignment, Case _case)
	{
		type = TokenType.EventProperty;
		propertyType = _propertyType;
		text = _text;
		minimumWidth = Math.Max(_minimumWidth, 0);
		maximumWidth = Math.Max(minimumWidth, _maximumWidth);
		alignment = _alignment;
		__case = _case;
	}

	public override string Format(Event e)
	{
		string text = null;
		switch (propertyType)
		{
		case EventPropertyType.LevelID:
			text = e.Level.IDString;
			break;
		case EventPropertyType.LevelName:
			text = e.Level.Name;
			break;
		case EventPropertyType.LevelPriority:
			text = e.Level.PriorityString;
			break;
		case EventPropertyType.ID:
			text = e.ID;
			break;
		case EventPropertyType.Category:
			text = e.Category;
			break;
		case EventPropertyType.Code:
			text = e.Code;
			break;
		case EventPropertyType.Year:
		{
			int year3 = e.Timestamp.Year;
			if (year3 < yearStrings.Length)
			{
				return yearStrings[year3];
			}
			return year3.ToString();
		}
		case EventPropertyType.Month:
			return dateStrings[e.Timestamp.Month];
		case EventPropertyType.Day:
			return dateStrings[e.Timestamp.Day];
		case EventPropertyType.Hour:
			return dateStrings[e.Timestamp.Hour];
		case EventPropertyType.Minute:
			return dateStrings[e.Timestamp.Minute];
		case EventPropertyType.Second:
			return dateStrings[e.Timestamp.Second];
		case EventPropertyType.Millisecond:
			return millisecondStrings[e.Timestamp.Millisecond];
		case EventPropertyType.Date:
		{
			StringBuilder stringBuilder3 = new StringBuilder(10);
			int year2 = e.Timestamp.Year;
			if (year2 < yearStrings.Length)
			{
				stringBuilder3.Append(yearStrings[year2]).Append('-');
			}
			else
			{
				stringBuilder3.Append(year2.ToString()).Append('-');
			}
			stringBuilder3.Append(dateStrings[e.Timestamp.Month]).Append('-');
			stringBuilder3.Append(dateStrings[e.Timestamp.Day]);
			return stringBuilder3.ToString();
		}
		case EventPropertyType.Time:
		{
			StringBuilder stringBuilder2 = new StringBuilder(12);
			stringBuilder2.Append(dateStrings[e.Timestamp.Hour]).Append(':');
			stringBuilder2.Append(dateStrings[e.Timestamp.Minute]).Append(':');
			stringBuilder2.Append(dateStrings[e.Timestamp.Second]).Append('.');
			stringBuilder2.Append(millisecondStrings[e.Timestamp.Millisecond]);
			return stringBuilder2.ToString();
		}
		case EventPropertyType.Timestamp:
		{
			StringBuilder stringBuilder = new StringBuilder(29);
			int year = e.Timestamp.Year;
			if (year < yearStrings.Length)
			{
				stringBuilder.Append(yearStrings[year]).Append('-');
			}
			else
			{
				stringBuilder.Append(year.ToString()).Append('-');
			}
			stringBuilder.Append(dateStrings[e.Timestamp.Month]).Append('-');
			stringBuilder.Append(dateStrings[e.Timestamp.Day]).Append(' ');
			stringBuilder.Append(dateStrings[e.Timestamp.Hour]).Append(':');
			stringBuilder.Append(dateStrings[e.Timestamp.Minute]).Append(':');
			stringBuilder.Append(dateStrings[e.Timestamp.Second]).Append('.');
			stringBuilder.Append(millisecondStrings[e.Timestamp.Millisecond]);
			stringBuilder.AppendFormat(CultureInfo.CurrentCulture, "{0:zzz}", e.Timestamp);
			return stringBuilder.ToString();
		}
		case EventPropertyType.TimeZone:
			switch (e.Timestamp.Month)
			{
			case 1:
			case 2:
			case 3:
				return daylightSavingsTimeZone;
			case 4:
				if (e.Timestamp.Day > 2)
				{
					return timeZone;
				}
				return daylightSavingsTimeZone;
			case 5:
			case 6:
			case 7:
			case 8:
			case 9:
				return timeZone;
			case 10:
				if (e.Timestamp.Day < 30)
				{
					return timeZone;
				}
				return daylightSavingsTimeZone;
			case 11:
			case 12:
				return daylightSavingsTimeZone;
			default:
				return timeZone;
			}
		case EventPropertyType.MonthName:
			text = monthNames[e.Timestamp.Month];
			break;
		case EventPropertyType.MonthShortName:
			text = monthShortNames[e.Timestamp.Month];
			break;
		case EventPropertyType.Weekday:
			return ((int)(e.Timestamp.DayOfWeek + 1)).ToString();
		case EventPropertyType.WeekdayName:
			text = weekdayNames[(int)e.Timestamp.DayOfWeek];
			break;
		case EventPropertyType.WeekdayShortName:
			text = weekdayShortNames[(int)e.Timestamp.DayOfWeek];
			break;
		case EventPropertyType.DayOfYear:
			return millisecondStrings[e.Timestamp.DayOfYear];
		case EventPropertyType.WeekOfMonth:
			return (e.Timestamp.Day / 7 + 1).ToString();
		case EventPropertyType.WeekOfYear:
			return dateStrings[e.Timestamp.DayOfYear / 7 + 1];
		}
		if (text == null)
		{
			text = ((minimumWidth != 0) ? "".PadLeft(minimumWidth, ' ') : "");
		}
		else if (text.Length < minimumWidth)
		{
			switch (alignment)
			{
			case Alignment.Center:
			{
				int num = (minimumWidth - text.Length) / 2;
				if (num > 0)
				{
					text = text.PadLeft(text.Length + num, ' ');
				}
				text = text.PadRight(minimumWidth, ' ');
				break;
			}
			case Alignment.Left:
				text = text.PadRight(minimumWidth, ' ');
				break;
			case Alignment.Right:
				text = text.PadLeft(minimumWidth, ' ');
				break;
			case Alignment.Default:
				text = text.PadRight(minimumWidth, ' ');
				break;
			}
		}
		else if (text.Length > maximumWidth)
		{
			text = text.Substring(0, maximumWidth);
		}
		switch (__case)
		{
		case Case.Lower:
			text = text.ToLower();
			break;
		case Case.Upper:
			text = text.ToUpper();
			break;
		}
		return text;
	}

	public override void Format(Event e, StringBuilder sb)
	{
		string text = null;
		switch (propertyType)
		{
		case EventPropertyType.LevelID:
			text = e.Level.IDString;
			break;
		case EventPropertyType.LevelName:
			text = e.Level.Name;
			break;
		case EventPropertyType.LevelPriority:
			text = e.Level.PriorityString;
			break;
		case EventPropertyType.ID:
			text = e.ID;
			break;
		case EventPropertyType.Category:
			text = e.Category;
			break;
		case EventPropertyType.Code:
			text = e.Code;
			break;
		case EventPropertyType.Year:
			sb.Append(yearStrings[e.Timestamp.Year]);
			return;
		case EventPropertyType.Month:
			sb.Append(dateStrings[e.Timestamp.Month]);
			return;
		case EventPropertyType.Day:
			sb.Append(dateStrings[e.Timestamp.Day]);
			return;
		case EventPropertyType.Hour:
			sb.Append(dateStrings[e.Timestamp.Hour]);
			return;
		case EventPropertyType.Minute:
			sb.Append(dateStrings[e.Timestamp.Minute]);
			return;
		case EventPropertyType.Second:
			sb.Append(dateStrings[e.Timestamp.Second]);
			return;
		case EventPropertyType.Millisecond:
			sb.Append(millisecondStrings[e.Timestamp.Millisecond]);
			return;
		case EventPropertyType.Date:
			sb.Append(yearStrings[e.Timestamp.Year]).Append('-');
			sb.Append(dateStrings[e.Timestamp.Month]).Append('-');
			sb.Append(dateStrings[e.Timestamp.Day]);
			return;
		case EventPropertyType.Time:
			sb.Append(dateStrings[e.Timestamp.Hour]).Append(':');
			sb.Append(dateStrings[e.Timestamp.Minute]).Append(':');
			sb.Append(dateStrings[e.Timestamp.Second]).Append('.');
			sb.Append(millisecondStrings[e.Timestamp.Millisecond]);
			return;
		case EventPropertyType.Timestamp:
			sb.Append(yearStrings[e.Timestamp.Year]).Append('-');
			sb.Append(dateStrings[e.Timestamp.Month]).Append('-');
			sb.Append(dateStrings[e.Timestamp.Day]).Append(' ');
			sb.Append(dateStrings[e.Timestamp.Hour]).Append(':');
			sb.Append(dateStrings[e.Timestamp.Minute]).Append(':');
			sb.Append(dateStrings[e.Timestamp.Second]).Append('.');
			sb.Append(millisecondStrings[e.Timestamp.Millisecond]);
			sb.AppendFormat(CultureInfo.CurrentCulture, "{0:zzz}", e.Timestamp);
			return;
		case EventPropertyType.TimeZone:
			switch (e.Timestamp.Month)
			{
			case 1:
			case 2:
			case 3:
				sb.Append(daylightSavingsTimeZone);
				break;
			case 4:
				if (e.Timestamp.Day > 2)
				{
					sb.Append(timeZone);
				}
				else
				{
					sb.Append(daylightSavingsTimeZone);
				}
				break;
			case 5:
			case 6:
			case 7:
			case 8:
			case 9:
				sb.Append(timeZone);
				break;
			case 10:
				if (e.Timestamp.Day < 30)
				{
					sb.Append(timeZone);
				}
				else
				{
					sb.Append(daylightSavingsTimeZone);
				}
				break;
			case 11:
			case 12:
				sb.Append(daylightSavingsTimeZone);
				break;
			default:
				sb.Append(timeZone);
				break;
			}
			return;
		case EventPropertyType.MonthName:
			text = monthNames[e.Timestamp.Month];
			break;
		case EventPropertyType.MonthShortName:
			text = monthShortNames[e.Timestamp.Month];
			break;
		case EventPropertyType.Weekday:
			sb.Append((int)(e.Timestamp.DayOfWeek + 1));
			return;
		case EventPropertyType.WeekdayName:
			text = weekdayNames[(int)e.Timestamp.DayOfWeek];
			break;
		case EventPropertyType.WeekdayShortName:
			text = weekdayShortNames[(int)e.Timestamp.DayOfWeek];
			break;
		case EventPropertyType.DayOfYear:
			sb.Append(millisecondStrings[e.Timestamp.DayOfYear]);
			return;
		case EventPropertyType.WeekOfMonth:
			sb.Append(e.Timestamp.Day / 7 + 1);
			return;
		case EventPropertyType.WeekOfYear:
			sb.Append(dateStrings[e.Timestamp.DayOfYear / 7 + 1]);
			return;
		}
		if (text == null)
		{
			if (minimumWidth != 0)
			{
				sb.Append("".PadLeft(minimumWidth, ' '));
			}
			return;
		}
		int length = text.Length;
		if (length < minimumWidth)
		{
			switch (alignment)
			{
			case Alignment.Center:
			{
				int num = (minimumWidth - length) / 2;
				if (num > 0)
				{
					text = text.PadLeft(length + num, ' ');
				}
				text = text.PadRight(minimumWidth, ' ');
				break;
			}
			case Alignment.Left:
				text = text.PadRight(minimumWidth, ' ');
				break;
			case Alignment.Right:
				text = text.PadLeft(minimumWidth, ' ');
				break;
			case Alignment.Default:
				text = text.PadRight(minimumWidth, ' ');
				break;
			}
		}
		else if (length > maximumWidth)
		{
			text = text.Substring(0, maximumWidth);
		}
		switch (__case)
		{
		case Case.Lower:
			sb.Append(text.ToLower());
			break;
		case Case.Upper:
			sb.Append(text.ToUpper());
			break;
		case Case.Default:
			sb.Append(text);
			break;
		}
	}

	private static string GetLocalTimeZone(int month, int day)
	{
		switch (month)
		{
		case 1:
		case 2:
		case 3:
			return daylightSavingsTimeZone;
		case 4:
			if (day > 2)
			{
				return timeZone;
			}
			return daylightSavingsTimeZone;
		case 5:
		case 6:
		case 7:
		case 8:
		case 9:
			return timeZone;
		case 10:
			if (day < 30)
			{
				return timeZone;
			}
			return daylightSavingsTimeZone;
		case 11:
		case 12:
			return daylightSavingsTimeZone;
		default:
			return timeZone;
		}
	}
}
