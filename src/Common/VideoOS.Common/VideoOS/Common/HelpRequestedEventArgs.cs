using System;
using System.Windows.Controls;

namespace VideoOS.Common;

public class HelpRequestedEventArgs : EventArgs
{
	public int TagID { get; private set; }

	public Control RequestingControl { get; private set; }

	public string LangDir { get; private set; }

	public HelpRequestedEventArgs(int tagID, Control requestingControl, string langDir)
	{
		TagID = tagID;
		RequestingControl = requestingControl;
		LangDir = langDir;
	}

	public HelpRequestedEventArgs(int tagID, string langDir)
	{
		TagID = tagID;
		LangDir = langDir;
	}
}
