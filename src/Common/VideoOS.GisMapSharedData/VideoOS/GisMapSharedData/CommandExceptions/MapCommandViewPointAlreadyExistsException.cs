using System;
using System.Runtime.Serialization;

namespace VideoOS.GisMapSharedData.CommandExceptions;

[Serializable]
[MapCommandException("ViewPointAlreadyExists")]
public class MapCommandViewPointAlreadyExistsException : MapCommandException
{
	private const string VIEW_POINT_ID_IDENTIFIER = "ViewPointId";

	public Guid ViewPointId
	{
		get
		{
			if (TryGetData("ViewPointId", out var value) && Guid.TryParse(value, out var result))
			{
				return result;
			}
			return default(Guid);
		}
	}

	public MapCommandViewPointAlreadyExistsException()
		: base(MapCommandErrorReason.ViewPointAlreadyExists, MapCommandErrorReason.ViewPointAlreadyExists)
	{
	}

	public MapCommandViewPointAlreadyExistsException(Guid viewPointId)
		: this()
	{
		AddData("ViewPointId", viewPointId.ToString());
	}

	public MapCommandViewPointAlreadyExistsException(string errorDescription)
		: base(errorDescription, MapCommandErrorReason.ViewPointAlreadyExists)
	{
	}

	public MapCommandViewPointAlreadyExistsException(string errorDescription, Guid viewPointId)
		: this(errorDescription)
	{
		AddData("ViewPointId", viewPointId.ToString());
	}

	public MapCommandViewPointAlreadyExistsException(string errorDescription, Exception innerException)
		: base(errorDescription, MapCommandErrorReason.ViewPointAlreadyExists, innerException)
	{
	}

	public MapCommandViewPointAlreadyExistsException(string errorDescription, Guid viewPointId, Exception innerException)
		: this(errorDescription, innerException)
	{
		AddData("ViewPointId", viewPointId.ToString());
	}

	protected MapCommandViewPointAlreadyExistsException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
