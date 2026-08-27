using System;
using System.Windows.Forms;

namespace VideoOS.Common.Controls;

public class UtilsWrapper : IUtilsWrapper
{
	public DialogResult ShowMessage(string caption, Exception ex, MessageBoxButtons buttons, MessageBoxIcon icon, IWin32Window owner)
	{
		return Utils.ShowMessage(caption, ex, buttons, MessageBoxDefaultButton.Button1, icon, owner);
	}

	public DialogResult ShowMessage(string caption, Exception ex, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, MessageBoxIcon icon, IWin32Window owner)
	{
		return Utils.ShowMessage(caption, ex, buttons, defaultButton, icon, owner);
	}

	public DialogResult ShowMessage(string caption, string message, MessageBoxButtons buttons, MessageBoxIcon icon, IWin32Window owner)
	{
		return Utils.ShowMessage(caption, message, buttons, MessageBoxDefaultButton.Button1, icon, owner);
	}

	public DialogResult ShowMessage(string caption, string message, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, MessageBoxIcon icon, IWin32Window owner)
	{
		Exception ex = new Exception(message);
		return Utils.ShowMessage(caption, ex, buttons, defaultButton, icon, owner);
	}

	public DialogResult ShowMessageOKButton(string caption, string message, IWin32Window owner)
	{
		return Utils.ShowMessage(caption, message, MessageBoxButtons.OK, MessageBoxIcon.Asterisk, owner);
	}

	public DialogResult ShowMessageOKButton(string caption, string message, MessageBoxIcon icon, IWin32Window owner)
	{
		return Utils.ShowMessage(caption, message, MessageBoxButtons.OK, icon, owner);
	}

	public DialogResult ShowMessageOKCancelButton(string caption, string message, IWin32Window owner)
	{
		return Utils.ShowMessage(caption, message, MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk, owner);
	}

	public DialogResult ShowMessageOKCancelButton(string caption, string message, MessageBoxIcon icon, IWin32Window owner)
	{
		return Utils.ShowMessage(caption, message, MessageBoxButtons.OKCancel, icon, owner);
	}

	public DialogResult ShowMessageAsError(string caption, Exception ex, IWin32Window owner)
	{
		return Utils.ShowMessage(caption, ex, MessageBoxButtons.OK, MessageBoxIcon.Hand, owner);
	}

	public DialogResult ShowMessageAsError(string caption, string message, Exception ex, IWin32Window owner)
	{
		return Utils.ShowMessage(caption, new MessageOnlyException(message, ex), MessageBoxButtons.OK, MessageBoxIcon.Hand, owner);
	}

	public DialogResult ShowMessageAsError(string caption, string message, IWin32Window owner)
	{
		return Utils.ShowMessage(caption, message, MessageBoxButtons.OK, MessageBoxIcon.Hand, owner);
	}

	public DialogResult ShowMessageAsWarning(string caption, Exception ex, IWin32Window owner)
	{
		return Utils.ShowMessage(caption, ex, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, owner);
	}

	public DialogResult ShowMessageAsWarning(string caption, string message, Exception ex, IWin32Window owner)
	{
		return Utils.ShowMessage(caption, new MessageOnlyException(message, ex), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, owner);
	}

	public DialogResult ShowMessageAsWarning(string caption, string message, IWin32Window owner)
	{
		return Utils.ShowMessage(caption, message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, owner);
	}

	public DialogResult ShowMessageAsQuestion(string caption, string message, IWin32Window owner)
	{
		return Utils.ShowMessage(caption, message, MessageBoxButtons.OK, MessageBoxIcon.Question, owner);
	}

	public DialogResult ShowMessageAsInformation(string caption, string message, IWin32Window owner)
	{
		return Utils.ShowMessage(caption, message, MessageBoxButtons.OK, MessageBoxIcon.Asterisk, owner);
	}

	public DialogResult ShowMessageYesNoButtons(string caption, string message, IWin32Window owner)
	{
		return Utils.ShowMessage(caption, message, MessageBoxButtons.YesNo, MessageBoxIcon.Question, owner);
	}

	public DialogResult ShowMessageYesNoButtons(string caption, string message, MessageBoxIcon icon, IWin32Window owner)
	{
		return Utils.ShowMessage(caption, message, MessageBoxButtons.YesNo, icon, owner);
	}

	public DialogResult ShowMessageYesNoButtons(string caption, string message, MessageBoxDefaultButton defaultButton, IWin32Window owner)
	{
		return Utils.ShowMessage(caption, message, MessageBoxButtons.YesNo, defaultButton, MessageBoxIcon.Question, owner);
	}

	public DialogResult ShowMessageYesNoButtons(string caption, string message, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, IWin32Window owner)
	{
		return Utils.ShowMessage(caption, message, MessageBoxButtons.YesNo, defaultButton, icon, owner);
	}

	public DialogResult ShowMessageYesNoCancelButtons(string caption, string message, IWin32Window owner)
	{
		return Utils.ShowMessage(caption, message, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, owner);
	}

	public DialogResult ShowMessageYesNoCancelButtons(string caption, string message, MessageBoxIcon icon, IWin32Window owner)
	{
		return Utils.ShowMessage(caption, message, MessageBoxButtons.YesNoCancel, icon, owner);
	}

	public int ShowMessage(string caption, string message, string[] buttons, ExceptionDialogDefaultButton defaultButton, MessageBoxIcon icon, IWin32Window owner)
	{
		return Utils.ShowMessage(caption, message, buttons, defaultButton, icon, owner);
	}
}
