using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public sealed class InvalidPropertyValueException : ConnectionException
{
	public InvalidPropertyValueException()
	{
	}

	public InvalidPropertyValueException(string message)
		: base(message)
	{
	}

	public InvalidPropertyValueException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	private InvalidPropertyValueException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
