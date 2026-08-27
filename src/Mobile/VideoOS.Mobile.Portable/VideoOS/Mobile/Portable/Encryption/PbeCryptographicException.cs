using System;

namespace VideoOS.Mobile.Portable.Encryption;

public class PbeCryptographicException : Exception
{
	public PbeCryptographicException(string message)
		: base(message)
	{
	}
}
