using NmToolkit;

namespace VideoOS.Toolkit;

public class ConfigurationException : ToolkitException
{
	public unsafe ConfigurationException(ImToolkitError* error)
		: base(error)
	{
	}

	public ConfigurationException(string message)
		: base(message)
	{
	}
}
