using System;

namespace VideoOS.UI.Common.WPF.IO;

public class FolderSelectionChangedEventArgs : EventArgs
{
	public DisplayInfo Info { get; private set; }

	public string ParentPath { get; private set; }

	public FolderSelectionChangedEventArgs(DisplayInfo info, string parentPath)
	{
		Info = info;
		ParentPath = parentPath;
	}
}
