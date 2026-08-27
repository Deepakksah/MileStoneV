using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class ObjectNotCreatedException : VmoException
{
	public ObjectNotCreatedException()
	{
	}

	public ObjectNotCreatedException(string message)
		: base(message)
	{
	}

	public ObjectNotCreatedException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected ObjectNotCreatedException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
