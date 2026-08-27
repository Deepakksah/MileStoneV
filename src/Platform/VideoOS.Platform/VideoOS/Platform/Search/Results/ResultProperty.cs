using System;
using System.Diagnostics;
using System.Globalization;

namespace VideoOS.Platform.Search.Results;

[DebuggerDisplay("{Title}:{Value}")]
public sealed class ResultProperty
{
	public string Title { get; }

	public string Value { get; }

	public DateTime DateTimeValue { get; }

	public ResultProperty(string title, string value)
	{
		if (string.IsNullOrEmpty(title))
		{
			throw new ArgumentNullMIPException("title");
		}
		Title = title;
		Value = value ?? string.Empty;
	}

	public ResultProperty(string title, DateTime value)
	{
		if (string.IsNullOrEmpty(title))
		{
			throw new ArgumentNullMIPException("title");
		}
		if (value == default(DateTime) || value.Kind != DateTimeKind.Utc)
		{
			throw new ArgumentMIPException("DateTime value cannot be empty and must be Utc.");
		}
		Title = title;
		DateTimeValue = value;
		Value = TimeZoneInfo.ConvertTime(DateTimeValue, ClientControl.Instance.SelectedTimeZoneInfo).ToString("G", CultureInfo.CurrentCulture);
	}
}
