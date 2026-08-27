using System;

namespace VideoOS.UI.Common.WPF.HyperlinkListView;

public class HyperlinkClickedArgs : EventArgs
{
	private HyperlinkListViewData _clickedLink;

	public HyperlinkListViewData ClickedHyperLink
	{
		get
		{
			return _clickedLink;
		}
		set
		{
			_clickedLink = value;
		}
	}

	public HyperlinkClickedArgs(HyperlinkListViewData clickedHyperlink)
	{
		_clickedLink = clickedHyperlink;
	}
}
