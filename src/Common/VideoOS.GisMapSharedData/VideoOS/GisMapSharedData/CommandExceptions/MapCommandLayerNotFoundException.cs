using System;
using System.Runtime.Serialization;

namespace VideoOS.GisMapSharedData.CommandExceptions;

[Serializable]
[MapCommandException("LayerNotFound")]
public class MapCommandLayerNotFoundException : MapCommandException
{
	public MapCommandLayerNotFoundException()
		: base(MapCommandErrorReason.LayerNotFound, MapCommandErrorReason.LayerNotFound)
	{
	}

	public MapCommandLayerNotFoundException(string errorDescription)
		: base(errorDescription, MapCommandErrorReason.LayerNotFound)
	{
	}

	public MapCommandLayerNotFoundException(string errorDescription, Exception innerException)
		: base(errorDescription, MapCommandErrorReason.LayerNotFound, innerException)
	{
	}

	protected MapCommandLayerNotFoundException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
