using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.Definition;

[Serializable]
public class InvalidSettingException : Exception
{
	public InvalidSettingException()
	{
	}

	public InvalidSettingException(string message)
		: base(message)
	{
	}

	public InvalidSettingException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected InvalidSettingException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
