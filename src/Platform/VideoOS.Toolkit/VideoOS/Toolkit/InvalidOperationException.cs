using NmToolkit;

namespace VideoOS.Toolkit;

public class InvalidOperationException : ToolkitException
{
	public unsafe InvalidOperationException(ImToolkitError* error)
		: base(error)
	{
	}

	public InvalidOperationException(string message)
		: base(message)
	{
	}
}
