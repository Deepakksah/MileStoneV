using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class PropertyReadOnlyException : VmoException
{
	public PropertyReadOnlyException()
	{
	}

	public PropertyReadOnlyException(string message)
		: base(message)
	{
	}

	public PropertyReadOnlyException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected PropertyReadOnlyException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
