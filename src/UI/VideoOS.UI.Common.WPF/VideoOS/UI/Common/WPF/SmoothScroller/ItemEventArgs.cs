using System;

namespace VideoOS.UI.Common.WPF.SmoothScroller;

public class ItemEventArgs : EventArgs
{
	public object Item { get; set; }

	public ItemEventArgs(object item)
	{
		Item = item;
	}
}
