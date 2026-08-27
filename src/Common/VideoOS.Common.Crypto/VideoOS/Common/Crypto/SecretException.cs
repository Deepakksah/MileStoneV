using System;

namespace VideoOS.Common.Crypto;

public class SecretException : Exception
{
	private const string DefaultMessage = "An error occurred during handling of secret";

	public SecretException()
		: base("An error occurred during handling of secret")
	{
	}

	public SecretException(string message)
		: base(message)
	{
	}

	public SecretException(Exception innerException)
		: base("An error occurred during handling of secret", innerException)
	{
	}

	public SecretException(string message, Exception innerException)
		: base(message, innerException)
	{
	}
}
