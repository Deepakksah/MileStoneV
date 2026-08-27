namespace VideoOS.Installer.Interfaces;

public class ClMsiHandlerDelegates
{
	public delegate void UpdateAction(string txt);

	public delegate void UpdateActionStart(string txt);

	public delegate void UpdateActionData(string txt);

	public delegate void UpdateProgress(string txt);
}
