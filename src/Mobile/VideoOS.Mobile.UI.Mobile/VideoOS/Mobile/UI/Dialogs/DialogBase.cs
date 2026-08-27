using System;
using System.ComponentModel;
using System.Windows;
using VideoOS.Mobile.UI.Interfaces;

namespace VideoOS.Mobile.UI.Dialogs;

public abstract class DialogBase : Window, IDialog
{
	private bool _isViewActivated;

	private bool _isBeingDismissed;

	protected IDialogViewModel ViewModel
	{
		get
		{
			return base.DataContext as IDialogViewModel;
		}
		set
		{
			base.DataContext = value;
		}
	}

	public DialogFlags VeiwFlags { get; private set; }

	public event EventHandler<IDialogDismissEventArgs> DialogDismissed;

	protected DialogBase(IDialogViewModel viewModel, DialogFlags dialogFlags = DialogFlags.None)
	{
		ViewModel = viewModel;
		VeiwFlags = dialogFlags;
		SubscribeViewModelEvents();
	}

	public void ActivateDialog()
	{
		if (_isViewActivated)
		{
			throw new InvalidOperationException("Unable to reactivate dialog view.");
		}
		_isViewActivated = true;
		base.Dispatcher.BeginInvoke(new Action(ShowDialogView));
	}

	protected void OnDialogDismissed(IDialogDismissEventArgs dismissArgs = null)
	{
		if (this.DialogDismissed != null)
		{
			IDialogDismissEventArgs e = dismissArgs ?? ViewModel.OnDismiss(isCancel: true);
			this.DialogDismissed(this, e);
		}
	}

	protected override void OnClosing(CancelEventArgs e)
	{
		if (!_isBeingDismissed)
		{
			DismissDialog(null, closeWindow: false);
		}
		base.OnClosing(e);
	}

	protected virtual void ShowDialogView()
	{
		if (VeiwFlags != DialogFlags.None)
		{
			base.ResizeMode = (((VeiwFlags & DialogFlags.CanResize) == DialogFlags.CanResize) ? ResizeMode.CanResize : ResizeMode.NoResize);
		}
		if (VeiwFlags == DialogFlags.None || (VeiwFlags & DialogFlags.Modal) == DialogFlags.Modal)
		{
			ShowDialog();
		}
		else
		{
			Show();
		}
	}

	public void DismissDialog()
	{
		DismissDialog(null);
	}

	private void DismissDialog(IDialogDismissEventArgs dismissArgs, bool closeWindow = true)
	{
		if (!_isBeingDismissed)
		{
			_isBeingDismissed = true;
			UnSubscribeViewModelEvents();
			OnDialogDismissed(dismissArgs);
			if (closeWindow)
			{
				Close();
			}
			_isBeingDismissed = false;
		}
	}

	private void UnSubscribeViewModelEvents()
	{
		ViewModel.RequestDismissDialog -= ViewModelRequestDismissDialog;
	}

	private void SubscribeViewModelEvents()
	{
		UnSubscribeViewModelEvents();
		ViewModel.RequestDismissDialog += ViewModelRequestDismissDialog;
	}

	private void ViewModelRequestDismissDialog(object sender, IDialogDismissEventArgs dismissArgs)
	{
		DismissDialog(dismissArgs);
	}
}
