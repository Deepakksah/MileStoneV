using System;
using VideoOS.Mobile.UI.Interfaces;

namespace VideoOS.Mobile.UI.Dialogs;

public class DialogDismissEventArgs : EventArgs, IDialogDismissEventArgs
{
	public bool? DialogResult { get; set; }

	public DialogDismissEventArgs(bool? dialogResult = null)
	{
		DialogResult = dialogResult;
	}
}
