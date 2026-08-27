using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class RetentionOptionRoleRelationCreationException : VmoException
{
	public RetentionOptionRoleRelationCreationException()
	{
	}

	public RetentionOptionRoleRelationCreationException(string message)
		: base(message)
	{
	}

	public RetentionOptionRoleRelationCreationException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected RetentionOptionRoleRelationCreationException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
