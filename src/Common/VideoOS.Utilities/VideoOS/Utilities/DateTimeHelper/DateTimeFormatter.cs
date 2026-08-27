using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;

namespace VideoOS.Utilities.DateTimeHelper;

public class DateTimeFormatter
{
	private static readonly string[] _fractionOfSecondSearchPatterns;

	private static readonly string _fractionOfSecondInjectPattern;

	private static readonly string[] _utcOffsetSearchPatterns;

	private static readonly string _utcOffsetInjectPattern;

	private static Dictionary<string, string> _formatPatternsMap;

	public static string TimestampPrefixForTesting { get; set; }

	static DateTimeFormatter()
	{
		_fractionOfSecondSearchPatterns = new string[1] { "s" };
		_fractionOfSecondInjectPattern = ".fff";
		_utcOffsetSearchPatterns = new string[4] { "tt", "f", "s", "m" };
		_utcOffsetInjectPattern = " ('UTC'zzz)";
		_formatPatternsMap = new Dictionary<string, string>();
		TimestampPrefixForTesting = ConfigurationManager.AppSettings["DateTimeFormatter.DisplayFormatPrefix"] ?? "";
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

	public static string ConvertAndFormat(string formatPattern, TimeZoneInfo timeZone, DateTime input)
	{
		return ConvertAndFormat(formatPattern, timeZone, input, injectMilliseconds: false, injectOffset: false);
	}

	public static string ConvertAndFormat(string formatPattern, TimeZoneInfo timeZone, DateTime input, bool injectMilliseconds, bool injectOffset)
	{
		if (input.ToUniversalTime() == DateTime.MinValue || input.ToUniversalTime() == DateTime.MaxValue)
		{
			return string.Empty;
		}
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

	public static string Format(string formatPattern, DateTime input)
	{
		return Format(formatPattern, input, injectMilliseconds: false, injectOffset: false);
	}

	public static string Format(string formatPattern, DateTime input, bool injectMilliseconds, bool injectOffset)
	{
		string formatPattern2 = GetFormatPattern(formatPattern, injectMilliseconds, injectOffset);
		CultureInfo currentCulture = CultureInfo.CurrentCulture;
		string arg = input.ToString(formatPattern2, currentCulture);
		return string.Format(currentCulture, "{0}{1}", TimestampPrefixForTesting, arg);
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
