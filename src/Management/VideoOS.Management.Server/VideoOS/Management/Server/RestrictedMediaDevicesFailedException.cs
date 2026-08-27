using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.Server;

[Serializable]
public sealed class RestrictedMediaDevicesFailedException : Exception
{
	public RestrictedMediaDevicesFailedException()
	{
	}

	public RestrictedMediaDevicesFailedException(string message)
		: base(message)
	{
	}

	public RestrictedMediaDevicesFailedException(string message, Exception inner)
		: base(message, inner)
	{
	}

	private RestrictedMediaDevicesFailedException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
