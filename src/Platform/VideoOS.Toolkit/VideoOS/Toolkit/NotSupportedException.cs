using NmToolkit;

namespace VideoOS.Toolkit;

public class NotSupportedException : ToolkitException
{
	public unsafe NotSupportedException(ImToolkitError* error)
		: base(error)
	{
	}

	public NotSupportedException(string message)
		: base(message)
	{
	}
}
