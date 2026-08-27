using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Controls;

[Serializable]
public class HelpException : Exception
{
	public HelpException()
	{
	}

	public HelpException(string message)
		: base(message)
	{
	}

	public HelpException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected HelpException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
