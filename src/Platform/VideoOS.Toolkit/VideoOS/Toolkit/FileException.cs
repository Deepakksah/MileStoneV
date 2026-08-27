using NmToolkit;

namespace VideoOS.Toolkit;

public class FileException : ToolkitException
{
	public unsafe FileException(ImToolkitError* error)
		: base(error)
	{
	}

	public FileException(string message)
		: base(message)
	{
	}
}
