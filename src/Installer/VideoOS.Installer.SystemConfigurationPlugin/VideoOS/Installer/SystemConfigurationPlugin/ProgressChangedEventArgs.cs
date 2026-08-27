using System;

namespace VideoOS.Installer.SystemConfigurationPlugin;

public class ProgressChangedEventArgs : EventArgs
{
	private int _progress;

	private string _message;

	public int Progress => _progress;

	public string Message => _message;

	public ProgressChangedEventArgs(int progress, string message)
	{
		_progress = progress;
		_message = message;
	}
}
