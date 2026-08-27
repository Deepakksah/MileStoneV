using System;

namespace VideoOS.Common.Help.UI;

public class HelpManagerHelpNotAvailableException : HelpManagerException
{
	public string OnlineUri { get; set; }

	public HelpManagerHelpNotAvailableException()
	{
	}

	public HelpManagerHelpNotAvailableException(string message)
		: base(message)
	{
	}

	public HelpManagerHelpNotAvailableException(string message, Exception inner)
		: base(message, inner)
	{
	}
}
