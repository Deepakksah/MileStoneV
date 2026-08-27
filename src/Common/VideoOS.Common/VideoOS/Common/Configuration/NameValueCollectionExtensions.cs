using System;
using System.Collections.Specialized;
using System.Globalization;

namespace VideoOS.Common.Configuration;

public static class NameValueCollectionExtensions
{
	public static int GetIntValueOrDefault(this NameValueCollection col, string key, int defaultValue = 0)
	{
		if (!int.TryParse(col[key], NumberStyles.Integer, CultureInfo.InvariantCulture, out var result))
		{
			return defaultValue;
		}
		return result;
	}

	public static double GetDoubleValueOrDefault(this NameValueCollection col, string key, double defaultValue = 0.0)
	{
		if (!double.TryParse(col[key], NumberStyles.Number, CultureInfo.InvariantCulture, out var result))
		{
			return defaultValue;
		}
		return result;
	}

	public static TimeSpan GetTimeSpanValueOrDefault(this NameValueCollection col, string key, TimeSpan defaultValue)
	{
		if (!TimeSpan.TryParse(col[key], CultureInfo.InvariantCulture, out var result))
		{
			return defaultValue;
		}
		return result;
	}

	public static string GetStringValueOrDefault(this NameValueCollection col, string key, string defaultValue = "")
	{
		return col[key] ?? defaultValue;
	}

	public static bool GetBoolValueOrDefault(this NameValueCollection col, string key, bool defaultValue = false)
	{
		string value = col[key];
		if (!string.IsNullOrWhiteSpace(value) && bool.TryParse(value, out var result))
		{
			return result;
		}
		return defaultValue;
	}

	public static bool TryGetIntValue(this NameValueCollection col, string key, out int value)
	{
		value = 0;
		string text = col[key];
		if (string.IsNullOrEmpty(text))
		{
			return false;
		}
		return int.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out value);
	}
}
