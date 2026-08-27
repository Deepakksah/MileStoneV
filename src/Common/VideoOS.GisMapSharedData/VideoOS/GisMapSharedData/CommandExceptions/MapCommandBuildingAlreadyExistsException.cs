using System;
using System.Runtime.Serialization;

namespace VideoOS.GisMapSharedData.CommandExceptions;

[Serializable]
[MapCommandException("BuildingAlreadyExists")]
public class MapCommandBuildingAlreadyExistsException : MapCommandException
{
	private const string BUILDING_ID_IDENTIFIER = "BuildingId";

	public Guid BuildingId
	{
		get
		{
			if (TryGetData("BuildingId", out var value) && Guid.TryParse(value, out var result))
			{
				return result;
			}
			return default(Guid);
		}
	}

	public MapCommandBuildingAlreadyExistsException()
		: base(MapCommandErrorReason.BuildingAlreadyExists, MapCommandErrorReason.BuildingAlreadyExists)
	{
	}

	public MapCommandBuildingAlreadyExistsException(Guid buildingId)
		: this()
	{
		AddData("BuildingId", buildingId.ToString());
	}

	public MapCommandBuildingAlreadyExistsException(string errorDescription)
		: base(errorDescription, MapCommandErrorReason.BuildingAlreadyExists)
	{
	}

	public MapCommandBuildingAlreadyExistsException(string errorDescription, Guid buildingId)
		: this(errorDescription)
	{
		AddData("BuildingId", buildingId.ToString());
	}

	public MapCommandBuildingAlreadyExistsException(string errorDescription, Exception innerException)
		: base(errorDescription, MapCommandErrorReason.BuildingAlreadyExists, innerException)
	{
	}

	public MapCommandBuildingAlreadyExistsException(string errorDescription, Guid buildingId, Exception innerException)
		: this(errorDescription, innerException)
	{
		AddData("BuildingId", buildingId.ToString());
	}

	protected MapCommandBuildingAlreadyExistsException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
