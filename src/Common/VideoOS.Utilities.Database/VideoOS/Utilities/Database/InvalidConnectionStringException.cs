using System;

namespace VideoOS.Utilities.Database;

public sealed class InvalidConnectionStringException : Exception
{
	public InvalidConnectionStringException(string errorMessage)
		: base(errorMessage)
	{
	}
}
