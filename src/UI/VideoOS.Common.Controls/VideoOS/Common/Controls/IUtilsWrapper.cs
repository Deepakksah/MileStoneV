using System;
using System.Windows.Forms;

namespace VideoOS.Common.Controls;

public interface IUtilsWrapper
{
	DialogResult ShowMessage(string caption, Exception ex, MessageBoxButtons buttons, MessageBoxIcon icon, IWin32Window owner);

	DialogResult ShowMessage(string caption, Exception ex, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, MessageBoxIcon icon, IWin32Window owner);

	DialogResult ShowMessage(string caption, string message, MessageBoxButtons buttons, MessageBoxIcon icon, IWin32Window owner);

	DialogResult ShowMessage(string caption, string message, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton, MessageBoxIcon icon, IWin32Window owner);

	DialogResult ShowMessageOKButton(string caption, string message, IWin32Window owner);

	DialogResult ShowMessageOKButton(string caption, string message, MessageBoxIcon icon, IWin32Window owner);

	DialogResult ShowMessageOKCancelButton(string caption, string message, IWin32Window owner);

	DialogResult ShowMessageOKCancelButton(string caption, string message, MessageBoxIcon icon, IWin32Window owner);

	DialogResult ShowMessageAsError(string caption, Exception ex, IWin32Window owner);

	DialogResult ShowMessageAsError(string caption, string message, Exception ex, IWin32Window owner);

	DialogResult ShowMessageAsError(string caption, string message, IWin32Window owner);

	DialogResult ShowMessageAsWarning(string caption, Exception ex, IWin32Window owner);

	DialogResult ShowMessageAsWarning(string caption, string message, Exception ex, IWin32Window owner);

	DialogResult ShowMessageAsWarning(string caption, string message, IWin32Window owner);

	DialogResult ShowMessageAsQuestion(string caption, string message, IWin32Window owner);

	DialogResult ShowMessageAsInformation(string caption, string message, IWin32Window owner);

	DialogResult ShowMessageYesNoButtons(string caption, string message, IWin32Window owner);

	DialogResult ShowMessageYesNoButtons(string caption, string message, MessageBoxIcon icon, IWin32Window owner);

	DialogResult ShowMessageYesNoButtons(string caption, string message, MessageBoxDefaultButton defaultButton, IWin32Window owner);

	DialogResult ShowMessageYesNoButtons(string caption, string message, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, IWin32Window owner);

	DialogResult ShowMessageYesNoCancelButtons(string caption, string message, IWin32Window owner);

	DialogResult ShowMessageYesNoCancelButtons(string caption, string message, MessageBoxIcon icon, IWin32Window owner);

	int ShowMessage(string caption, string message, string[] buttons, ExceptionDialogDefaultButton defaultButton, MessageBoxIcon icon, IWin32Window owner);
}
