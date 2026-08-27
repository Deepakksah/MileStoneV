using System;
using System.Collections.Generic;

namespace VideoOS.UI.Common.WPF.IO;

public class NodeExpandedEventArgs : EventArgs
{
	public bool Handled { get; set; }

	public DisplayInfo Info { get; private set; }

	public IList<DisplayInfo> Children { get; private set; }

	public NodeExpandedEventArgs(DisplayInfo info)
	{
		Handled = false;
		Info = info;
		Children = new List<DisplayInfo>();
	}
}
