using System;
using System.Runtime.Serialization;

namespace VideoOS.GisMapSharedData.CommandExceptions;

[Serializable]
[MapCommandException("MapAlreadyExists")]
public class MapCommandMapAlreadyExistsException : MapCommandException
{
	private const string MAP_ID_IDENTIFIER = "MapId";

	public Guid MapId
	{
		get
		{
			if (TryGetData("MapId", out var value) && Guid.TryParse(value, out var result))
			{
				return result;
			}
			return default(Guid);
		}
	}

	public MapCommandMapAlreadyExistsException()
		: base(MapCommandErrorReason.MapAlreadyExists, MapCommandErrorReason.MapAlreadyExists)
	{
	}

	public MapCommandMapAlreadyExistsException(Guid mapId)
		: this()
	{
		AddData("MapId", mapId.ToString());
	}

	public MapCommandMapAlreadyExistsException(string errorDescription)
		: base(errorDescription, MapCommandErrorReason.MapAlreadyExists)
	{
	}

	public MapCommandMapAlreadyExistsException(string errorDescription, Guid mapId)
		: this(errorDescription)
	{
		AddData("MapId", mapId.ToString());
	}

	public MapCommandMapAlreadyExistsException(string errorDescription, Exception innerException)
		: base(errorDescription, MapCommandErrorReason.MapAlreadyExists, innerException)
	{
	}

	public MapCommandMapAlreadyExistsException(string errorDescription, Guid mapId, Exception innerException)
		: this(errorDescription, innerException)
	{
		AddData("MapId", mapId.ToString());
	}

	protected MapCommandMapAlreadyExistsException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
