using System;
using System.Runtime.Serialization;

namespace VideoOS.Installer.Manager;

[Serializable]
public class PrerequisitesNotInstalledException : Exception
{
	public PrerequisitesNotInstalledException()
	{
	}

	public PrerequisitesNotInstalledException(string message)
		: base(message)
	{
	}

	public PrerequisitesNotInstalledException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected PrerequisitesNotInstalledException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
