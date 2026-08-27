using System;

namespace VideoOS.Installer.Sequencer.ErrorHandling;

public class DowngradeDisallowedException : InstallerSequencerException
{
	public DowngradeDisallowedException(string message)
		: base(message)
	{
	}

	public DowngradeDisallowedException(string message, Exception innerException)
		: base(message, innerException)
	{
	}
}
