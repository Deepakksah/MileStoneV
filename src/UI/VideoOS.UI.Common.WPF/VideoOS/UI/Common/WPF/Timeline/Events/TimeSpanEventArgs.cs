using System;

namespace VideoOS.UI.Common.WPF.Timeline.Events;

public class TimeSpanEventArgs : EventArgs
{
	public TimeSpan TimeSpan { get; set; }

	public TimeSpanEventArgs(TimeSpan timeSpan)
	{
		TimeSpan = timeSpan;
	}
}
