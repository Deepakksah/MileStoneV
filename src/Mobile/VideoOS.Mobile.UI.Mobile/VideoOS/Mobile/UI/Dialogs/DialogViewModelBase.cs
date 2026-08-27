using System;
using Prism.Commands;
using VideoOS.Mobile.UI.Interfaces;
using VideoOS.Mobile.UI.ViewModel.MVVM;

namespace VideoOS.Mobile.UI.Dialogs;

public class DialogViewModelBase : ViewModelBase, IDialogViewModel
{
	public DelegateCommand DismissCommand { get; private set; }

	public event EventHandler<IDialogDismissEventArgs> RequestDismissDialog;

	public DialogViewModelBase()
	{
		InitCommands();
	}

	public virtual IDialogDismissEventArgs OnDismiss(bool isCancel)
	{
		return new DialogDismissEventArgs
		{
			DialogResult = !isCancel
		};
	}

	private void InitCommands()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		DismissCommand = new DelegateCommand((Action)Dismiss);
	}

	protected void OnRequestDismissDialog(bool isCancel)
	{
		if (this.RequestDismissDialog != null)
		{
			IDialogDismissEventArgs e = OnDismiss(isCancel);
			this.RequestDismissDialog(this, e);
		}
	}

	private void Dismiss()
	{
		OnRequestDismissDialog(isCancel: false);
	}
}
