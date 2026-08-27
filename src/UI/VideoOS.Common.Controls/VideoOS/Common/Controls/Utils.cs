using System;
using System.Windows.Forms;

namespace VideoOS.Common.Controls;

public static class Utils
{
	public static DialogResult ShowMessage(string caption, Exception ex, MessageBoxButtons buttons, MessageBoxIcon icon, IWin32Window owner)
	{
		return ShowMessage(caption, ex, buttons, MessageBoxDefaultButton.Button1, icon, owner, makeSounds: true);
	}

	public static DialogResult ShowMessage(string caption, Exception ex, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, MessageBoxIcon icon, IWin32Window owner)
	{
		return ShowMessage(caption, ex, buttons, defaultButton, icon, owner, makeSounds: true);
	}

	public static DialogResult ShowMessage(string caption, Exception ex, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, MessageBoxIcon icon, IWin32Window owner, bool makeSounds)
	{
		return ShowMessage(caption, ex, string.Empty, 0, 0, buttons, defaultButton, icon, owner, makeSounds);
	}

	public static DialogResult ShowMessage(string caption, Exception ex, string link, int linkPosition, int linkLength, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, MessageBoxIcon icon, IWin32Window owner, bool makeSounds)
	{
		ExceptionDialogSymbol symbol = ExceptionDialogSymbol.Information;
		switch (icon)
		{
		case MessageBoxIcon.Question:
			symbol = ExceptionDialogSymbol.Question;
			break;
		case MessageBoxIcon.Exclamation:
			symbol = ExceptionDialogSymbol.Warning;
			break;
		case MessageBoxIcon.Asterisk:
			symbol = ExceptionDialogSymbol.Information;
			break;
		case MessageBoxIcon.None:
			symbol = ExceptionDialogSymbol.None;
			break;
		case MessageBoxIcon.Hand:
			symbol = ExceptionDialogSymbol.Stop;
			break;
		}
		ExceptionDialogButtons buttons2 = ExceptionDialogButtons.OK;
		switch (buttons)
		{
		case MessageBoxButtons.OKCancel:
			buttons2 = ExceptionDialogButtons.OKCancel;
			break;
		case MessageBoxButtons.AbortRetryIgnore:
			buttons2 = ExceptionDialogButtons.AbortRetryIgnore;
			break;
		case MessageBoxButtons.YesNoCancel:
			buttons2 = ExceptionDialogButtons.YesNoCancel;
			break;
		case MessageBoxButtons.YesNo:
			buttons2 = ExceptionDialogButtons.YesNo;
			break;
		case MessageBoxButtons.RetryCancel:
			buttons2 = ExceptionDialogButtons.RetryCancel;
			break;
		}
		ExceptionDialog exceptionDialog = new ExceptionDialog(ex, buttons2, symbol)
		{
			Caption = caption,
			Beep = makeSounds,
			MessageLevelDefault = 1,
			Link = link,
			LinkPosition = linkPosition,
			LinkLength = linkLength
		};
		if (buttons == MessageBoxButtons.YesNo)
		{
			switch (defaultButton)
			{
			case MessageBoxDefaultButton.Button2:
				exceptionDialog.DefaultButton = ExceptionDialogDefaultButton.Button2;
				break;
			default:
				exceptionDialog.DefaultButton = ExceptionDialogDefaultButton.Button1;
				break;
			}
		}
		return exceptionDialog.Show(owner);
	}

	public static DialogResult ShowMessage(string caption, string message, MessageBoxButtons buttons, MessageBoxIcon icon, IWin32Window owner)
	{
		return ShowMessage(caption, message, buttons, icon, owner, makeSounds: true);
	}

	public static DialogResult ShowMessage(string caption, string message, MessageBoxButtons buttons, MessageBoxIcon icon, IWin32Window owner, bool makeSounds)
	{
		return ShowMessage(caption, message, buttons, MessageBoxDefaultButton.Button1, icon, owner, makeSounds);
	}

	public static DialogResult ShowMessage(string caption, string message, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, MessageBoxIcon icon, IWin32Window owner)
	{
		return ShowMessage(caption, message, buttons, defaultButton, icon, owner, makeSounds: true);
	}

	public static DialogResult ShowMessage(string caption, string message, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, MessageBoxIcon icon, IWin32Window owner, bool makeSounds)
	{
		return ShowMessage(caption, new Exception(message), buttons, defaultButton, icon, owner, makeSounds);
	}

	public static DialogResult ShowMessageOKButton(string caption, string message, IWin32Window owner)
	{
		return ShowMessage(caption, message, MessageBoxButtons.OK, MessageBoxIcon.Asterisk, owner);
	}

	public static DialogResult ShowMessageOKButton(string caption, string message, MessageBoxIcon icon, IWin32Window owner)
	{
		return ShowMessage(caption, message, MessageBoxButtons.OK, icon, owner);
	}

	public static DialogResult ShowMessageOKCancelButton(string caption, string message, IWin32Window owner)
	{
		return ShowMessage(caption, message, MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk, owner);
	}

	public static DialogResult ShowMessageOKCancelButton(string caption, string message, MessageBoxIcon icon, IWin32Window owner)
	{
		return ShowMessage(caption, message, MessageBoxButtons.OKCancel, icon, owner);
	}

	public static DialogResult ShowMessageAsError(string caption, Exception ex, IWin32Window owner)
	{
		return ShowMessage(caption, ex, MessageBoxButtons.OK, MessageBoxIcon.Hand, owner);
	}

	public static DialogResult ShowMessageAsError(string caption, string message, Exception ex, IWin32Window owner)
	{
		return ShowMessage(caption, new MessageOnlyException(message, ex), MessageBoxButtons.OK, MessageBoxIcon.Hand, owner);
	}

	public static DialogResult ShowMessageAsError(string caption, string message, IWin32Window owner)
	{
		return ShowMessage(caption, message, MessageBoxButtons.OK, MessageBoxIcon.Hand, owner);
	}

	public static DialogResult ShowMessageAsWarning(string caption, Exception ex, IWin32Window owner)
	{
		return ShowMessage(caption, ex, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, owner);
	}

	public static DialogResult ShowWarningOkCancelButton(string caption, string message, IWin32Window owner)
	{
		return ShowMessage(caption, message, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation, owner);
	}

	public static DialogResult ShowMessageAsWarning(string caption, string message, Exception ex, IWin32Window owner)
	{
		return ShowMessage(caption, new MessageOnlyException(message, ex), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, owner);
	}

	public static DialogResult ShowMessageAsWarning(string caption, string message, IWin32Window owner)
	{
		return ShowMessage(caption, message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, owner);
	}

	public static DialogResult ShowMessageAsQuestion(string caption, string message, IWin32Window owner)
	{
		return ShowMessage(caption, message, MessageBoxButtons.OK, MessageBoxIcon.Question, owner);
	}

	public static DialogResult ShowMessageAsInformation(string caption, string message, IWin32Window owner)
	{
		return ShowMessage(caption, message, MessageBoxButtons.OK, MessageBoxIcon.Asterisk, owner);
	}

	public static DialogResult ShowMessageYesNoButtons(string caption, string message, IWin32Window owner)
	{
		return ShowMessage(caption, message, MessageBoxButtons.YesNo, MessageBoxIcon.Question, owner);
	}

	public static DialogResult ShowMessageYesNoButtons(string caption, string message, MessageBoxIcon icon, IWin32Window owner)
	{
		return ShowMessage(caption, message, MessageBoxButtons.YesNo, icon, owner);
	}

	public static DialogResult ShowMessageYesNoButtons(string caption, string message, string link, int linkPosition, int linkLen, MessageBoxIcon icon, IWin32Window owner)
	{
		return ShowMessage(caption, new Exception(message), link, linkPosition, linkLen, MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button1, icon, owner, makeSounds: true);
	}

	public static DialogResult ShowMessageYesNoButtons(string caption, string message, MessageBoxDefaultButton defaultButton, IWin32Window owner)
	{
		return ShowMessage(caption, message, MessageBoxButtons.YesNo, defaultButton, MessageBoxIcon.Question, owner);
	}

	public static DialogResult ShowMessageYesNoButtons(string caption, string message, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, IWin32Window owner)
	{
		return ShowMessage(caption, message, MessageBoxButtons.YesNo, defaultButton, icon, owner);
	}

	public static DialogResult ShowMessageYesNoCancelButtons(string caption, string message, IWin32Window owner)
	{
		return ShowMessage(caption, message, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, owner);
	}

	public static DialogResult ShowMessageYesNoCancelButtons(string caption, string message, MessageBoxIcon icon, IWin32Window owner)
	{
		return ShowMessage(caption, message, MessageBoxButtons.YesNoCancel, icon, owner);
	}

	public static int ShowMessage(string caption, string message, string[] buttons, ExceptionDialogDefaultButton defaultButton, MessageBoxIcon icon, IWin32Window owner)
	{
		if (buttons == null)
		{
			throw new ArgumentNullException("buttons");
		}
		if (buttons.Length < 1 || buttons.Length > 5)
		{
			throw new ArgumentException("Must contain between 1 and 5 strings.", "buttons");
		}
		ExceptionDialogSymbol symbol = ExceptionDialogSymbol.Information;
		switch (icon)
		{
		case MessageBoxIcon.Question:
			symbol = ExceptionDialogSymbol.Question;
			break;
		case MessageBoxIcon.Exclamation:
			symbol = ExceptionDialogSymbol.Warning;
			break;
		case MessageBoxIcon.Asterisk:
			symbol = ExceptionDialogSymbol.Information;
			break;
		case MessageBoxIcon.None:
			symbol = ExceptionDialogSymbol.None;
			break;
		case MessageBoxIcon.Hand:
			symbol = ExceptionDialogSymbol.Stop;
			break;
		}
		ExceptionDialog exceptionDialog = new ExceptionDialog(message, caption, ExceptionDialogButtons.Custom, symbol);
		switch (buttons.Length)
		{
		case 1:
			exceptionDialog.SetButtonText(buttons[0]);
			break;
		case 2:
			exceptionDialog.SetButtonText(buttons[0], buttons[1]);
			break;
		case 3:
			exceptionDialog.SetButtonText(buttons[0], buttons[1], buttons[2]);
			break;
		case 4:
			exceptionDialog.SetButtonText(buttons[0], buttons[1], buttons[2], buttons[3]);
			break;
		case 5:
			exceptionDialog.SetButtonText(buttons[0], buttons[1], buttons[2], buttons[3], buttons[4]);
			break;
		}
		exceptionDialog.DefaultButton = defaultButton;
		exceptionDialog.Show(owner);
		return exceptionDialog.CustomDialogResult switch
		{
			ExceptionDialogResult.Button1 => 0, 
			ExceptionDialogResult.Button2 => 1, 
			ExceptionDialogResult.Button3 => 2, 
			ExceptionDialogResult.Button4 => 3, 
			ExceptionDialogResult.Button5 => 4, 
			_ => -1, 
		};
	}
}
