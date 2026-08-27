using System;

namespace VideoOS.Platform.AccessControl.Plugin;

public sealed class ACConfigurationException : Exception
{
	public ACConfigurationException(string message)
		: base(message)
	{
	}
}
