using System;
using System.Runtime.Serialization;

namespace VideoOS.GisMapSharedData.CommandExceptions;

[Serializable]
[MapCommandException("LayerOrderDuplicateFound")]
public class MapCommandLayerOrderDuplicateFoundException : MapCommandException
{
	public MapCommandLayerOrderDuplicateFoundException()
		: base(MapCommandErrorReason.LayerOrderDuplicateFound, MapCommandErrorReason.LayerOrderDuplicateFound)
	{
	}

	public MapCommandLayerOrderDuplicateFoundException(string errorDescription)
		: base(errorDescription, MapCommandErrorReason.LayerOrderDuplicateFound)
	{
	}

	public MapCommandLayerOrderDuplicateFoundException(string errorDescription, Exception innerException)
		: base(errorDescription, MapCommandErrorReason.LayerOrderDuplicateFound, innerException)
	{
	}

	protected MapCommandLayerOrderDuplicateFoundException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
