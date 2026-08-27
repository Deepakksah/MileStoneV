using System;

namespace VideoOS.Installer.Sequencer;

public class ProgressEventArgs : EventArgs
{
	public int Value { get; set; }

	public int Maximum { get; set; }

	public ProgressEventArgs(int value, int maximum)
	{
		Value = value;
		Maximum = maximum;
	}
}
