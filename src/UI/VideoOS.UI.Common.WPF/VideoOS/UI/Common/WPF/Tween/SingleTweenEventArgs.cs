using System;

namespace VideoOS.UI.Common.WPF.Tween;

public class SingleTweenEventArgs : EventArgs
{
	public double NewValue { get; set; }

	public double OldValue { get; set; }

	public SingleTweenEventArgs(double newValue, double oldValue)
	{
		NewValue = newValue;
		OldValue = oldValue;
	}
}
