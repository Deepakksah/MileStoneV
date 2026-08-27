using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class RetentionOptionRoleRelationDeleteException : VmoException
{
	public RetentionOptionRoleRelationDeleteException()
	{
	}

	public RetentionOptionRoleRelationDeleteException(string message)
		: base(message)
	{
	}

	public RetentionOptionRoleRelationDeleteException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected RetentionOptionRoleRelationDeleteException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
