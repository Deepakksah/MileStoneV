using NmToolkit;

namespace VideoOS.Toolkit;

public class InvalidArgumentException : ToolkitException
{
	public unsafe InvalidArgumentException(ImToolkitError* error)
		: base(error)
	{
	}

	public InvalidArgumentException(string message)
		: base(message)
	{
	}
}
