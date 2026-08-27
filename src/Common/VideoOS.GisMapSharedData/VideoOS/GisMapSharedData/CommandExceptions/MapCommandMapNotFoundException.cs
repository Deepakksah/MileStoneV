using System;
using System.Runtime.Serialization;

namespace VideoOS.GisMapSharedData.CommandExceptions;

[Serializable]
[MapCommandException("MapNotFound")]
public class MapCommandMapNotFoundException : MapCommandException
{
	public MapCommandMapNotFoundException()
		: base(MapCommandErrorReason.MapNotFound, MapCommandErrorReason.MapNotFound)
	{
	}

	public MapCommandMapNotFoundException(string errorDescription)
		: base(errorDescription, MapCommandErrorReason.MapNotFound)
	{
	}

	public MapCommandMapNotFoundException(string errorDescription, Exception innerException)
		: base(errorDescription, MapCommandErrorReason.MapNotFound, innerException)
	{
	}

	protected MapCommandMapNotFoundException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
