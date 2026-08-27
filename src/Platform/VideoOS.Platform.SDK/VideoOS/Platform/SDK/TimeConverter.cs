using System;
using System.Collections.Generic;
using System.Globalization;

namespace VideoOS.Platform.SDK;

public static class TimeConverter
{
	private static readonly DateTime Dt1970Utc;

	private static Dictionary<string, string> _formatPatternsMap;

	private static readonly string[] _fractionOfSecondSearchPatterns;

	private static readonly string _fractionOfSecondInjectPattern;

	private static readonly string[] _utcOffsetSearchPatterns;

	private static readonly string _utcOffsetInjectPattern;

	public static string TimestampPrefixForTesting { get; set; }

	static TimeConverter()
	{
		Dt1970Utc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
		_formatPatternsMap = new Dictionary<string, string>();
		_fractionOfSecondSearchPatterns = new string[1] { "s" };
		_fractionOfSecondInjectPattern = ".fff";
		_utcOffsetSearchPatterns = new string[4] { "tt", "f", "s", "m" };
		_utcOffsetInjectPattern = " ('UTC'zzz)";
		TimestampPrefixForTesting = "";
	}

	public static DateTime FromDouble(double millisec)
	{
		return (Dt1970Utc + new TimeSpan((long)millisec * 10000)).ToLocalTime();
	}

	public static DateTime FromInt(int millisec)
	{
		return (Dt1970Utc + new TimeSpan((long)millisec * 10000L)).ToLocalTime();
	}

	public static DateTime FromString(string millisec)
	{
		if (long.TryParse(millisec, out var _))
		{
			return (Dt1970Utc + new TimeSpan(long.Parse(millisec, NumberStyles.Integer) * 10000)).ToLocalTime();
		}
		if (DateTime.TryParse(millisec, out var result2))
		{
			return result2;
		}
		return DateTime.MaxValue;
	}

	public static double ToDouble(DateTime time)
	{
		DateTime dateTime = time;
		if (time.Kind != DateTimeKind.Utc)
		{
			dateTime = time.ToUniversalTime();
		}
		return (dateTime - Dt1970Utc).Ticks / 10000;
	}

	public static int ToInt(DateTime time)
	{
		DateTime dateTime = time;
		if (time.Kind != DateTimeKind.Utc)
		{
			dateTime = time.ToUniversalTime();
		}
		long num = (dateTime - Dt1970Utc).Ticks / 10000;
		if (num > int.MaxValue)
		{
			EnvironmentManager.Instance.Log(error: true, "TimeConverter", "ToInt() loosing significant value: Consider to use ToLong()");
		}
		return (int)num;
	}

	public static long ToLong(DateTime time)
	{
		DateTime dateTime = time;
		if (time.Kind != DateTimeKind.Utc)
		{
			dateTime = time.ToUniversalTime();
		}
		return (dateTime - Dt1970Utc).Ticks / 10000;
	}

	public static string ToMillisecString(DateTime time)
	{
		DateTime time2 = time;
		if (time.Kind != DateTimeKind.Utc)
		{
			time2 = time.ToUniversalTime();
		}
		return ToLong(time2).ToString();
	}

	public static string ToMilisecString(DateTime time)
	{
		return ToMillisecString(time);
	}

	public static string ToStringWithTandZ(DateTime datetime)
	{
		string text = "yyyy-MM-ddTHH:mm:ss.fffZ";
		return datetime.ToString(text, CultureInfo.InvariantCulture);
	}

	public static string ConvertAndFormat(string formatPattern, TimeZoneInfo timeZone, DateTime input)
	{
		return ConvertAndFormat(formatPattern, timeZone, input, injectMilliseconds: false, injectOffset: false);
	}

	public static string ConvertAndFormat(string formatPattern, TimeZoneInfo timeZone, DateTime input, bool injectMilliseconds, bool injectOffset)
	{
		string formatPattern2 = GetFormatPattern(formatPattern, injectMilliseconds, injectOffset);
		CultureInfo currentCulture = CultureInfo.CurrentCulture;
		string arg = ConvertToTimeZone(timeZone, input).ToString(formatPattern2, currentCulture);
		return string.Format(currentCulture, "{0}{1}", TimestampPrefixForTesting, arg);
	}

	public static DateTimeOffset ConvertToTimeZone(TimeZoneInfo timeZone, DateTime input)
	{
		DateTime dateTime = TimeZoneInfo.ConvertTime(input, timeZone);
		TimeSpan utcOffset = timeZone.GetUtcOffset(input.ToUniversalTime());
		DateTimeOffset result;
		try
		{
			return new DateTimeOffset(dateTime, utcOffset);
		}
		catch (ArgumentException)
		{
			result = new DateTimeOffset(input, new TimeSpan(0L));
		}
		return result;
	}

	private static string GetFormatPattern(string inputPattern, bool withMilliseconds, bool withOffset)
	{
		string key = string.Format(CultureInfo.InvariantCulture, "[{0}][{1}][{2}]", inputPattern, withMilliseconds, withOffset);
		if (!_formatPatternsMap.ContainsKey(key))
		{
			_formatPatternsMap[key] = Inject(inputPattern, withMilliseconds, withOffset);
		}
		return _formatPatternsMap[key];
	}

	private static string Inject(string inputFormatPattern, bool injectMilliseconds, bool injectTimeZoneOffset)
	{
		if (!injectMilliseconds && !injectTimeZoneOffset)
		{
			return inputFormatPattern;
		}
		string text = ExpandPattern(inputFormatPattern);
		if (injectMilliseconds)
		{
			text = InjectFractionOfSeconds(text);
		}
		if (injectTimeZoneOffset)
		{
			text = InjectTimeZoneOffset(text);
		}
		return text;
	}

	private static string ExpandPattern(string inputPattern)
	{
		if (inputPattern.Length != 1)
		{
			return inputPattern;
		}
		string[] allDateTimePatterns = CultureInfo.CurrentCulture.DateTimeFormat.GetAllDateTimePatterns(inputPattern[0]);
		if (allDateTimePatterns.Length == 0)
		{
			return inputPattern;
		}
		return allDateTimePatterns[0].ToString();
	}

	private static string InjectFractionOfSeconds(string inputFormat)
	{
		return Inject(inputFormat, _fractionOfSecondSearchPatterns, _fractionOfSecondInjectPattern);
	}

	private static string InjectTimeZoneOffset(string inputFormat)
	{
		return Inject(inputFormat, _utcOffsetSearchPatterns, _utcOffsetInjectPattern);
	}

	private static string Inject(string input, string[] searchPatterns, string inject)
	{
		foreach (string text in searchPatterns)
		{
			int num = input.LastIndexOf(text, StringComparison.Ordinal);
			if (num >= 0)
			{
				return input.Insert(num + text.Length, inject);
			}
		}
		return input;
	}
}
