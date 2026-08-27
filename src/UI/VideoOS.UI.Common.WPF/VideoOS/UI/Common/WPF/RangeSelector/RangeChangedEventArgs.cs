using System;

namespace VideoOS.UI.Common.WPF.RangeSelector;

public class RangeChangedEventArgs : EventArgs
{
	public double Low { get; private set; }

	public double High { get; private set; }

	public RangeChangedEventArgs(double low, double high)
	{
		Low = low;
		High = high;
	}
}
