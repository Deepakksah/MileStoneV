using System;

namespace VideoOS.Common.Controls;

public class SelectionChangedEventArgs : EventArgs
{
	public bool ValidSelection { get; }

	public SelectionChangedEventArgs(bool validSelection)
	{
		ValidSelection = validSelection;
	}
}
