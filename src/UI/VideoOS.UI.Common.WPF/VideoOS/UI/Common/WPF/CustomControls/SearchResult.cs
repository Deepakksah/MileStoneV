using System;
using System.Collections.Generic;

namespace VideoOS.UI.Common.WPF.CustomControls;

public class SearchResult : EventArgs
{
	public Guid SearchId { get; set; }

	public bool OpenPopup { get; set; }

	public List<SearchItem> Items { get; set; }

	public bool SearchErrorOccurred { get; set; }
}
