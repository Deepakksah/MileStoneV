using System;

namespace VideoOS.Mobile.Portable.Utilities;

public class DateTimeAndDiffs
{
	public DateTime DateTime { get; set; }

	public TimeSpan TimeSpan { get; set; }

	public DateTimeAndDiffs(DateTime dateTime, TimeSpan timeSpan)
	{
		DateTime = dateTime;
		TimeSpan = timeSpan;
	}

	public DateTimeAndDiffs()
	{
		DateTime = DateTime.MinValue;
		TimeSpan = TimeSpan.Zero;
	}
}
