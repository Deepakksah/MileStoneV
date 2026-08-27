using System;
using VideoOS.Mobile.UI.Dialogs;

namespace VideoOS.Mobile.UI.Interfaces;

public interface IDialog
{
	DialogFlags VeiwFlags { get; }

	event EventHandler<IDialogDismissEventArgs> DialogDismissed;

	void ActivateDialog();

	void DismissDialog();
}
