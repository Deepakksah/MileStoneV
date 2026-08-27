using System;
using System.Runtime.Serialization;

namespace VideoOS.Installer.Sequencer.ErrorHandling;

public class InstallerSequencerException : Exception
{
	public InstallerSequencerException()
	{
	}

	public InstallerSequencerException(string message)
		: base(message)
	{
	}

	public InstallerSequencerException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected InstallerSequencerException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
