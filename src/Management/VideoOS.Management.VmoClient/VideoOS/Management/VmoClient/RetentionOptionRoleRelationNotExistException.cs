using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class RetentionOptionRoleRelationNotExistException : VmoException
{
	public RetentionOptionRoleRelationNotExistException()
	{
	}

	public RetentionOptionRoleRelationNotExistException(string message)
		: base(message)
	{
	}

	public RetentionOptionRoleRelationNotExistException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected RetentionOptionRoleRelationNotExistException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
