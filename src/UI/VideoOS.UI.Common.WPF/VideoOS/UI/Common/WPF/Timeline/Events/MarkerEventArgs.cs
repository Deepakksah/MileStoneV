using System;

namespace VideoOS.UI.Common.WPF.Timeline.Events;

public class MarkerEventArgs : EventArgs
{
	public object MarkerTag { get; set; }

	public MarkerEventArgs(object markerTag)
	{
		MarkerTag = markerTag;
	}
}
