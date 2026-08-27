using System;

namespace VideoOS.IdentityServer.Common.Handlers;

public class IdentityServerTokenException : Exception
{
	public IdentityServerTokenException()
	{
	}

	public IdentityServerTokenException(string message)
		: base(message)
	{
	}

	public IdentityServerTokenException(string message, Exception inner)
		: base(message, inner)
	{
	}
}
