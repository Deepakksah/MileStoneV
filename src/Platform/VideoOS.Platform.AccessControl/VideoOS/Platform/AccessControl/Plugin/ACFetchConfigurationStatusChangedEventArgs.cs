using System;

namespace VideoOS.Platform.AccessControl.Plugin;

public sealed class ACFetchConfigurationStatusChangedEventArgs : EventArgs
{
	public double Progress { get; private set; }

	public string ProgressMessage { get; private set; }

	public ACConfiguration ACConfiguration { get; private set; }

	public string ErrorMessage { get; private set; }

	public ACFetchConfigurationStatusChangedEventArgs(double progress, string progressMessage)
	{
		Progress = progress;
		ProgressMessage = progressMessage;
	}

	public ACFetchConfigurationStatusChangedEventArgs(ACConfiguration acConfiguration)
	{
		ACConfiguration = acConfiguration;
	}

	public ACFetchConfigurationStatusChangedEventArgs(string errorMessage)
	{
		ErrorMessage = errorMessage;
	}
}
