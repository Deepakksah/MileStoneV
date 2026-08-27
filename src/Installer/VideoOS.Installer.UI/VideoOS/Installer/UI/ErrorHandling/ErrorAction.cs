using System.Diagnostics;

namespace VideoOS.Installer.UI.ErrorHandling;

public abstract class ErrorAction
{
	public string ActionText { get; set; }

	public string ActionCommand { get; set; }

	public string ActionArgs { get; set; }

	public ErrorAction(string actionText, string actionCommand, string actionArgs)
	{
		ActionText = actionText;
		ActionCommand = actionCommand;
		ActionArgs = actionArgs;
	}

	public virtual bool ExecuteAction()
	{
		if (!string.IsNullOrEmpty(ActionCommand))
		{
			Process.Start(ActionCommand, ActionArgs);
			return true;
		}
		return true;
	}
}
