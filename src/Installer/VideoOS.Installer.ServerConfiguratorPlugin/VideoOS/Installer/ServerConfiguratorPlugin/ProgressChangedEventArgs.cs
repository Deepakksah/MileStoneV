using System;

namespace VideoOS.Installer.ServerConfiguratorPlugin;

public class ProgressChangedEventArgs : EventArgs
{
	private int _progress;

	public int Progress => _progress;

	public ProgressChangedEventArgs(int progress)
	{
		_progress = progress;
	}
}
