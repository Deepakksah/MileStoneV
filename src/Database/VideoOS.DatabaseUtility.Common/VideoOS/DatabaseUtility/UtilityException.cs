using System;

namespace VideoOS.DatabaseUtility;

public class UtilityException : ApplicationException
{
	public UtilityException(string errorMessage, Exception innerException)
		: base(errorMessage, innerException)
	{
	}

	public UtilityException(string errorMessage)
		: base(errorMessage)
	{
	}

	public UtilityException()
	{
	}
}
