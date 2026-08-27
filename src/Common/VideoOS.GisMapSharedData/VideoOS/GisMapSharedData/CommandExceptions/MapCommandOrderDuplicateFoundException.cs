using System;
using System.Runtime.Serialization;

namespace VideoOS.GisMapSharedData.CommandExceptions;

[Serializable]
[MapCommandException("OrderDuplicateFound")]
public class MapCommandOrderDuplicateFoundException : MapCommandException
{
	public MapCommandOrderDuplicateFoundException()
		: base(MapCommandErrorReason.OrderDuplicateFound, MapCommandErrorReason.OrderDuplicateFound)
	{
	}

	public MapCommandOrderDuplicateFoundException(string errorDescription)
		: base(errorDescription, MapCommandErrorReason.OrderDuplicateFound)
	{
	}

	public MapCommandOrderDuplicateFoundException(string errorDescription, Exception innerException)
		: base(errorDescription, MapCommandErrorReason.OrderDuplicateFound, innerException)
	{
	}

	protected MapCommandOrderDuplicateFoundException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
