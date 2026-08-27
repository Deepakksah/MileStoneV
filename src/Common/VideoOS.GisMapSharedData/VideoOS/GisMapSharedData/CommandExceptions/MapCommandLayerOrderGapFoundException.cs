using System;
using System.Runtime.Serialization;

namespace VideoOS.GisMapSharedData.CommandExceptions;

[Serializable]
[MapCommandException("LayerOrderGapFound")]
public class MapCommandLayerOrderGapFoundException : MapCommandException
{
	public MapCommandLayerOrderGapFoundException()
		: base(MapCommandErrorReason.LayerOrderGapFound, MapCommandErrorReason.LayerOrderGapFound)
	{
	}

	public MapCommandLayerOrderGapFoundException(string errorDescription)
		: base(errorDescription, MapCommandErrorReason.LayerOrderGapFound)
	{
	}

	public MapCommandLayerOrderGapFoundException(string errorDescription, Exception innerException)
		: base(errorDescription, MapCommandErrorReason.LayerOrderGapFound, innerException)
	{
	}

	protected MapCommandLayerOrderGapFoundException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
