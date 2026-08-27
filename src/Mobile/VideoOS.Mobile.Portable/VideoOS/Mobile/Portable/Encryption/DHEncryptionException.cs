using System;

namespace VideoOS.Mobile.Portable.Encryption;

public class DHEncryptionException : Exception
{
	public DHEncryptionException(string message = "")
		: base(message)
	{
	}
}
