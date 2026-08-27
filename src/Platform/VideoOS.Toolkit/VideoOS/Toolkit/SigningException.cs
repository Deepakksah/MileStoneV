using NmToolkit;

namespace VideoOS.Toolkit;

public class SigningException : ToolkitException
{
	public unsafe SigningException(ImToolkitError* error)
		: base(error)
	{
	}

	public SigningException(string message)
		: base(message)
	{
	}
}
