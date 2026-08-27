using NmToolkit;

namespace VideoOS.Toolkit;

public class EncoderException : ToolkitException
{
	public unsafe EncoderException(ImToolkitError* error)
		: base(error)
	{
	}

	public EncoderException(string message)
		: base(message)
	{
	}
}
