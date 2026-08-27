using System;

namespace VideoOS.Installer.UI.Framework;

public class PageEventArgsException : Exception
{
	public PageEventArgsException(string message)
		: base(message)
	{
	}
}
