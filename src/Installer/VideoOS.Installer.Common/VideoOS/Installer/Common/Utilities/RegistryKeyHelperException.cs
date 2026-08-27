using System;
using System.Runtime.Serialization;

namespace VideoOS.Installer.Common.Utilities;

[Serializable]
public class RegistryKeyHelperException : Exception
{
	public RegistryKeyHelperException()
	{
	}

	public RegistryKeyHelperException(string message)
		: base(message)
	{
	}

	public RegistryKeyHelperException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected RegistryKeyHelperException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
