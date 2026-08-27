using System;

namespace VideoOS.Mobile.UI.Interfaces;

public interface IDialogViewModel
{
	event EventHandler<IDialogDismissEventArgs> RequestDismissDialog;

	IDialogDismissEventArgs OnDismiss(bool isCancel);
}
