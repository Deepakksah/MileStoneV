using NmToolkit;

namespace VideoOS.Toolkit;

public class NotConnectedException : ToolkitException
{
	public unsafe NotConnectedException(ImToolkitError* error)
		: base(error)
	{
	}

	public NotConnectedException(string message)
		: base(message)
	{
	}
}
