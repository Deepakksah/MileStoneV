using System;

namespace VideoOS.GatewayService.InstallerPlugin;

public class ProgressChangedEventArgs : EventArgs
{
	private int _progress;

	public int Progress => _progress;

	public ProgressChangedEventArgs(int progress)
	{
		_progress = progress;
	}
}
