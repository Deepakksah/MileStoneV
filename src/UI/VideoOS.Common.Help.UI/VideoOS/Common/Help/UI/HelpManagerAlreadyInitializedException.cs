using System;

namespace VideoOS.Common.Help.UI;

public class HelpManagerAlreadyInitializedException : HelpManagerException
{
	public HelpManagerAlreadyInitializedException()
	{
	}

	public HelpManagerAlreadyInitializedException(string message)
		: base(message)
	{
	}

	public HelpManagerAlreadyInitializedException(string message, Exception inner)
		: base(message, inner)
	{
	}
}
