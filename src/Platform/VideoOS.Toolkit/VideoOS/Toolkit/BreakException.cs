using NmToolkit;

namespace VideoOS.Toolkit;

public class BreakException : ToolkitException
{
	public unsafe BreakException(ImToolkitError* error)
		: base(error)
	{
	}

	public BreakException(string message)
		: base(message)
	{
	}
}
