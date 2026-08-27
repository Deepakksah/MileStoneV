using System;

namespace VideoOS.Common.Crypto;

public class SecretNotFoundException : SecretException
{
	private const string DefaultMessage = "The secret was not found";

	public SecretNotFoundException()
		: base("The secret was not found")
	{
	}

	public SecretNotFoundException(string message)
		: base(message)
	{
	}

	public SecretNotFoundException(Exception innerException)
		: base("The secret was not found", innerException)
	{
	}

	public SecretNotFoundException(string message, Exception innerException)
		: base(message, innerException)
	{
	}
}
