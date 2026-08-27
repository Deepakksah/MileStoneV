using System;
using System.Runtime.Serialization;

namespace VideoOS.GisMapSharedData.CommandExceptions;

[Serializable]
[MapCommandException("BuildingNotFound")]
public class MapCommandBuildingNotFoundException : MapCommandException
{
	public MapCommandBuildingNotFoundException()
		: base(MapCommandErrorReason.BuildingNotFound, MapCommandErrorReason.BuildingNotFound)
	{
	}

	public MapCommandBuildingNotFoundException(string errorDescription)
		: base(errorDescription, MapCommandErrorReason.BuildingNotFound)
	{
	}

	public MapCommandBuildingNotFoundException(string errorDescription, Exception innerException)
		: base(errorDescription, MapCommandErrorReason.BuildingNotFound, innerException)
	{
	}

	protected MapCommandBuildingNotFoundException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
