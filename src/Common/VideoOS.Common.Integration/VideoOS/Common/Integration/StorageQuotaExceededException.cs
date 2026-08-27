using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration;

[Serializable]
public class StorageQuotaExceededException : Exception
{
	public StorageQuotaExceededException()
	{
	}

	public StorageQuotaExceededException(string message)
		: base(message)
	{
	}

	public StorageQuotaExceededException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected StorageQuotaExceededException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
