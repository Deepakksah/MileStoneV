using VideoOS.Installer.UI.Properties;

namespace VideoOS.Installer.UI.ErrorHandling;

public class ViewLogAction : ErrorAction
{
	public ViewLogAction(string logFile)
		: base(Resources.ErrorMessage_ViewLogButton, "notepad.exe", logFile)
	{
	}
}
