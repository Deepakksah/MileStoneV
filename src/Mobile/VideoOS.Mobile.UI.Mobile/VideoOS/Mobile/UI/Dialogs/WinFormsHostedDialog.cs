using System.Windows.Forms;
using System.Windows.Interop;
using VideoOS.Mobile.UI.Interfaces;

namespace VideoOS.Mobile.UI.Dialogs;

public abstract class WinFormsHostedDialog : DialogBase
{
	protected WinFormsHostedDialog(IDialogViewModel viewModel, DialogFlags dialogFlags = DialogFlags.None)
		: base(viewModel, dialogFlags)
	{
	}

	public static ContainerControl FindActiveWinFormsContainerControl(ContainerControl control)
	{
		ContainerControl containerControl = control;
		ContainerControl containerControl2 = containerControl;
		while (containerControl != null)
		{
			containerControl = containerControl.ActiveControl as ContainerControl;
			containerControl2 = containerControl ?? containerControl2;
		}
		return containerControl2;
	}

	protected override void ShowDialogView()
	{
		if (Application.OpenForms.Count > 0)
		{
			new WindowInteropHelper(this).Owner = Application.OpenForms[0].Handle;
		}
		base.ShowDialogView();
	}
}
