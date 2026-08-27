using System;

namespace VideoOS.Common.Crypto;

public class RemoveFileSecretException : SecretException
{
	public RemoveFileSecretException()
		: base("Unable to remove secret file")
	{
	}

	public RemoveFileSecretException(string message)
		: base(message)
	{
	}

	public RemoveFileSecretException(string message, Exception innerException)
		: base(message, innerException)
	{
	}
}
