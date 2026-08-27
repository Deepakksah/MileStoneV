using System;
using System.Runtime.Serialization;

namespace VideoOS.GisMapSharedData.CommandExceptions;

[Serializable]
[MapCommandException("NonUniqueBuildingId")]
public class MapCommandNonUniqueBuildingIdException : MapCommandException
{
	public MapCommandNonUniqueBuildingIdException()
		: base(MapCommandErrorReason.NonUniqueBuildingId, MapCommandErrorReason.NonUniqueBuildingId)
	{
	}

	public MapCommandNonUniqueBuildingIdException(string errorDescription)
		: base(errorDescription, MapCommandErrorReason.NonUniqueBuildingId)
	{
	}

	public MapCommandNonUniqueBuildingIdException(string errorDescription, Exception innerException)
		: base(errorDescription, MapCommandErrorReason.NonUniqueBuildingId, innerException)
	{
	}

	protected MapCommandNonUniqueBuildingIdException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
