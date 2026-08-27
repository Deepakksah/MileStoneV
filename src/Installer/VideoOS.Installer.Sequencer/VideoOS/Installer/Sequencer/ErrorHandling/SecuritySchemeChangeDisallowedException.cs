using System;
using System.Runtime.Serialization;

namespace VideoOS.Installer.Sequencer.ErrorHandling;

public class SecuritySchemeChangeDisallowedException : InstallerSequencerException
{
	public SecuritySchemeChangeDisallowedException()
	{
	}

	public SecuritySchemeChangeDisallowedException(string message)
		: base(message)
	{
	}

	public SecuritySchemeChangeDisallowedException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected SecuritySchemeChangeDisallowedException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
