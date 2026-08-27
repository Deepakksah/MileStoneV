using System;
using System.Runtime.Serialization;

namespace VideoOS.GisMapSharedData.CommandExceptions;

[Serializable]
[MapCommandException("LevelNotFound")]
public class MapCommandLevelNotFoundException : MapCommandException
{
	public MapCommandLevelNotFoundException()
		: base(MapCommandErrorReason.LevelNotFound, MapCommandErrorReason.LevelNotFound)
	{
	}

	public MapCommandLevelNotFoundException(string errorDescription)
		: base(errorDescription, MapCommandErrorReason.LevelNotFound)
	{
	}

	public MapCommandLevelNotFoundException(string errorDescription, Exception innerException)
		: base(errorDescription, MapCommandErrorReason.LevelNotFound, innerException)
	{
	}

	protected MapCommandLevelNotFoundException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
