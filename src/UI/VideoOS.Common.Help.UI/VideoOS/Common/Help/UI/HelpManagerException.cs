using System;

namespace VideoOS.Common.Help.UI;

public abstract class HelpManagerException : Exception
{
	public HelpManagerException()
	{
	}

	public HelpManagerException(string message)
		: base(message)
	{
	}

	public HelpManagerException(string message, Exception inner)
		: base(message, inner)
	{
	}
}
