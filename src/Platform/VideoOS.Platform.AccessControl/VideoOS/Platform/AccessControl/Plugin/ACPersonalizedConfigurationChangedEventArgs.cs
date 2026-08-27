using System;

namespace VideoOS.Platform.AccessControl.Plugin;

public sealed class ACPersonalizedConfigurationChangedEventArgs : EventArgs
{
	public string Username { get; private set; }

	public ACPersonalizedConfigurationChangedEventArgs(string username)
	{
		Username = username;
	}
}
