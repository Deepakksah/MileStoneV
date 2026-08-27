using System;

namespace VideoOS.Installer.SystemConfigurationPlugin;

public class ProgressPlugin
{
	public delegate void ProgressChangedEventHandler(object sender, ProgressChangedEventArgs e);

	private ProgressChangedEventHandler progressChanged;

	public string LatestMessage { get; set; }

	public int ProgressSteps { get; private set; }

	public int CurrentStep { get; set; }

	public int StepInPercent => 100 / ProgressSteps;

	public int NextStepInPercent => (CurrentStep + 1) * StepInPercent;

	protected int ServerPort { get; private set; }

	public event ProgressChangedEventHandler ProgressChanged
	{
		add
		{
			progressChanged = (ProgressChangedEventHandler)Delegate.Combine(progressChanged, value);
		}
		remove
		{
			progressChanged = (ProgressChangedEventHandler)Delegate.Remove(progressChanged, value);
		}
	}

	protected void OnProgressChanged(int percentage, string procedure)
	{
		progressChanged?.Invoke(this, new ProgressChangedEventArgs(percentage, procedure));
	}

	public ProgressPlugin(int serverPort)
		: this(serverPort, 1)
	{
	}

	public ProgressPlugin(int serverPort, int progressSteps)
	{
		if (progressSteps <= 0)
		{
			throw new ArgumentOutOfRangeException("progressSteps");
		}
		ServerPort = serverPort;
		ProgressSteps = progressSteps;
		CurrentStep = 0;
	}
}
