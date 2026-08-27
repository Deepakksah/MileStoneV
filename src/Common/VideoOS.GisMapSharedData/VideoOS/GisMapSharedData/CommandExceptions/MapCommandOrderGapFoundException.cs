using System;
using System.Runtime.Serialization;

namespace VideoOS.GisMapSharedData.CommandExceptions;

[Serializable]
[MapCommandException("OrderGapFound")]
public class MapCommandOrderGapFoundException : MapCommandException
{
	public MapCommandOrderGapFoundException()
		: base(MapCommandErrorReason.OrderGapFound, MapCommandErrorReason.OrderGapFound)
	{
	}

	public MapCommandOrderGapFoundException(string errorDescription)
		: base(errorDescription, MapCommandErrorReason.OrderGapFound)
	{
	}

	public MapCommandOrderGapFoundException(string errorDescription, Exception innerException)
		: base(errorDescription, MapCommandErrorReason.OrderGapFound, innerException)
	{
	}

	protected MapCommandOrderGapFoundException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
