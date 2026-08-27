using System;

namespace VideoOS.Installer.Common;

public class ActionEventArgs : EventArgs
{
	public string Action { get; set; }

	public ActionEventArgs(string action)
	{
		Action = action;
	}
}
