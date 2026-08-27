using System;

namespace VideoOS.UI.Common.WPF.Timeline.Events;

public class DateTimeEventArgs : EventArgs
{
	public DateTime DateTime { get; set; }

	public DateTimeEventArgs(DateTime dateTime)
	{
		DateTime = dateTime;
	}
}
