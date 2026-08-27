using System;

namespace VideoOS.UI.Common.WPF.Timeline.Control.Data;

public class TimelineSpan
{
	public TimeSpan TimeSpan { get; set; }

	public string Text { get; set; }

	public TimelineSpan(TimeSpan timeSpan, string text)
	{
		TimeSpan = timeSpan;
		Text = text;
	}

	public override bool Equals(object obj)
	{
		return TimeSpan.Equals(((TimelineSpan)obj).TimeSpan);
	}

	public override int GetHashCode()
	{
		return TimeSpan.GetHashCode();
	}
}
