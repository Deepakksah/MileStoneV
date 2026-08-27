using System;

namespace VideoOS.IdentityServer.Repositories;

[Serializable]
public class ScopesFormatInvalidException : Exception
{
	public ScopesFormatInvalidException()
	{
	}

	public ScopesFormatInvalidException(string message)
		: base(message)
	{
	}

	public ScopesFormatInvalidException(string message, Exception inner)
		: base(message, inner)
	{
	}
}
