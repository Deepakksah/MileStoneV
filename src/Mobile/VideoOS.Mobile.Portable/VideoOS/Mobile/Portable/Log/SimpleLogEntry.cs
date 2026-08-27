using System;
using System.Collections.Generic;

namespace VideoOS.Mobile.Portable.Log;

public class SimpleLogEntry : ILogEntry
{
	private const int _sanitizedStringLenght = 512;

	private LogContent _logLevel;

	public string Text;

	public DateTime DateTime;

	public string Origin;

	public LogContent LogLevel
	{
		get
		{
			return _logLevel;
		}
		set
		{
			_logLevel = value;
		}
	}

	public SimpleLogEntry(LogContent loglevel, string text, DateTime dateTime, string origin, IEnumerable<string> multipleMessages = null, bool sanitized = true)
	{
		LogLevel = loglevel;
		Text = text;
		if (multipleMessages != null)
		{
			foreach (string multipleMessage in multipleMessages)
			{
				Text = (Text.Equals("") ? (Text + multipleMessage) : (Text + Log.Separator + multipleMessage));
			}
		}
		DateTime = dateTime;
		if (sanitized)
		{
			string text2 = Text;
			Text = ((text2 == null || text2.Length <= 512) ? Text : Text?.Substring(0, 512));
			Origin = ((origin == null || origin.Length <= 512) ? origin : origin?.Substring(0, 512));
		}
		else
		{
			Origin = origin;
		}
	}

	public string GetLogEntry()
	{
		return $"{Log.LogDateTime(DateTime)}{Log.Separator}{_logLevel}{Log.Separator}{Origin}{Log.Separator}{Text}";
	}
}
