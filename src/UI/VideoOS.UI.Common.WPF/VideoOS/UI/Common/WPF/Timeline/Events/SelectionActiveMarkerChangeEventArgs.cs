using System;

namespace VideoOS.UI.Common.WPF.Timeline.Events;

public class SelectionActiveMarkerChangeEventArgs : EventArgs
{
	public SelectionMarker NewSelectionMarker { get; set; }

	public SelectionMarker OldSelectionMarker { get; set; }

	public SelectionActiveMarkerChangeEventArgs(SelectionMarker newSelectionMarker, SelectionMarker oldSelectionMarker)
	{
		NewSelectionMarker = newSelectionMarker;
		OldSelectionMarker = oldSelectionMarker;
	}
}
