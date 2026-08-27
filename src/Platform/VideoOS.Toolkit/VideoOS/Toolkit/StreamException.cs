using NmToolkit;

namespace VideoOS.Toolkit;

public class StreamException : ToolkitException
{
	public unsafe StreamException(ImToolkitError* error)
		: base(error)
	{
	}

	public StreamException(string message)
		: base(message)
	{
	}
}
