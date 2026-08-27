using NmToolkit;

namespace VideoOS.Toolkit;

public class AuthorizationException : ToolkitException
{
	public unsafe AuthorizationException(ImToolkitError* error)
		: base(error)
	{
	}

	public AuthorizationException(string message)
		: base(message)
	{
	}
}
