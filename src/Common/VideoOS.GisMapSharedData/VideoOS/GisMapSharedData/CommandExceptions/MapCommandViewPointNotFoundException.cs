using System;
using System.Runtime.Serialization;

namespace VideoOS.GisMapSharedData.CommandExceptions;

[Serializable]
[MapCommandException("ViewPointNotFound")]
public class MapCommandViewPointNotFoundException : MapCommandException
{
	public MapCommandViewPointNotFoundException()
		: base(MapCommandErrorReason.ViewPointNotFound, MapCommandErrorReason.ViewPointNotFound)
	{
	}

	public MapCommandViewPointNotFoundException(string errorDescription)
		: base(errorDescription, MapCommandErrorReason.ViewPointNotFound)
	{
	}

	public MapCommandViewPointNotFoundException(string errorDescription, Exception innerException)
		: base(errorDescription, MapCommandErrorReason.ViewPointNotFound, innerException)
	{
	}

	protected MapCommandViewPointNotFoundException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
