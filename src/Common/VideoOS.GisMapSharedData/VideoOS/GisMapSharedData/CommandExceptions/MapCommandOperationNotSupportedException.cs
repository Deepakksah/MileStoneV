using System;
using System.Runtime.Serialization;

namespace VideoOS.GisMapSharedData.CommandExceptions;

[Serializable]
[MapCommandException("OperationNotSupported")]
public class MapCommandOperationNotSupportedException : MapCommandException
{
	public MapCommandOperationNotSupportedException()
		: base(MapCommandErrorReason.OperationNotSupported, MapCommandErrorReason.OperationNotSupported)
	{
	}

	public MapCommandOperationNotSupportedException(string errorDescription)
		: base(errorDescription, MapCommandErrorReason.OperationNotSupported)
	{
	}

	public MapCommandOperationNotSupportedException(string errorDescription, Exception innerException)
		: base(errorDescription, MapCommandErrorReason.OperationNotSupported, innerException)
	{
	}

	protected MapCommandOperationNotSupportedException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
