using System;

namespace VideoOS.Platform.AccessControl.Plugin;

public sealed class ACFetchPersonalizedConfigurationStatusChangedEventArgs : EventArgs
{
	public double Progress { get; private set; }

	public string ProgressMessage { get; private set; }

	public ACConfiguration ACConfiguration { get; private set; }

	public string ErrorMessage { get; private set; }

	public string Username { get; private set; }

	public string ConfigVersion { get; private set; }

	public ACFetchPersonalizedConfigurationStatusChangedEventArgs(double progress, string progressMessage, string username)
	{
		Progress = progress;
		ProgressMessage = progressMessage;
		Username = username;
	}

	public ACFetchPersonalizedConfigurationStatusChangedEventArgs(ACConfiguration acConfiguration, string username, string configVersion = null)
	{
		ACConfiguration = acConfiguration;
		Username = username;
		ConfigVersion = configVersion;
	}

	public ACFetchPersonalizedConfigurationStatusChangedEventArgs(string errorMessage, string username)
	{
		ErrorMessage = errorMessage;
		Username = username;
	}
}
