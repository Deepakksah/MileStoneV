using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.Server;

[Serializable]
public class InvalidCameraException : Exception
{
	public InvalidCameraException()
	{
	}

	public InvalidCameraException(string message)
		: base(message)
	{
	}

	public InvalidCameraException(string message, Exception ex)
		: base(message, ex)
	{
	}

	protected InvalidCameraException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
