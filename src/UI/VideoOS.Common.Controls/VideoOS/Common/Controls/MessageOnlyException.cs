using System;

namespace VideoOS.Common.Controls;

public class MessageOnlyException : Exception
{
	public MessageOnlyException(string message, Exception inner)
		: base(message, inner)
	{
	}
}
