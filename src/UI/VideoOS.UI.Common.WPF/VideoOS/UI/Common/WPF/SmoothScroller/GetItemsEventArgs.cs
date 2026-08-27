using System;

namespace VideoOS.UI.Common.WPF.SmoothScroller;

public class GetItemsEventArgs : EventArgs
{
	public int BeginIndex { get; set; }

	public int EndIndex { get; set; }

	public GetItemsEventArgs(int beginIndex, int endIndex)
	{
		BeginIndex = beginIndex;
		EndIndex = endIndex;
	}
}
