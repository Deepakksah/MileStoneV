using NmToolkit;

namespace VideoOS.Toolkit;

public class DecoderException : ToolkitException
{
	public unsafe DecoderException(ImToolkitError* error)
		: base(error)
	{
	}

	public DecoderException(string message)
		: base(message)
	{
	}
}
