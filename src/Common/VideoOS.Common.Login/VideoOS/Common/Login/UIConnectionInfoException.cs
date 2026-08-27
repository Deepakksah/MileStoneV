using System;

namespace VideoOS.Common.Login;

public class UIConnectionInfoException : Exception
{
	public UIConnectionInfoException(string message)
		: base(message)
	{
	}
}
