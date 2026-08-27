using System;
using System.Runtime.Serialization;

namespace VideoOS.GisMapSharedData.CommandExceptions;

[Serializable]
[MapCommandException("LevelAlreadyExists")]
public class MapCommandLevelAlreadyExistsException : MapCommandException
{
	private const string LEVEL_ID_IDENTIFIER = "LevelId";

	public Guid LevelId
	{
		get
		{
			if (TryGetData("LevelId", out var value) && Guid.TryParse(value, out var result))
			{
				return result;
			}
			return default(Guid);
		}
	}

	public MapCommandLevelAlreadyExistsException()
		: base(MapCommandErrorReason.LevelAlreadyExists, MapCommandErrorReason.LevelAlreadyExists)
	{
	}

	public MapCommandLevelAlreadyExistsException(Guid levelId)
		: this()
	{
		AddData("LevelId", levelId.ToString());
	}

	public MapCommandLevelAlreadyExistsException(string errorDescription)
		: base(errorDescription, MapCommandErrorReason.LevelAlreadyExists)
	{
	}

	public MapCommandLevelAlreadyExistsException(string errorDescription, Guid levelId)
		: this(errorDescription)
	{
		AddData("LevelId", levelId.ToString());
	}

	public MapCommandLevelAlreadyExistsException(string errorDescription, Exception innerException)
		: base(errorDescription, MapCommandErrorReason.LevelAlreadyExists, innerException)
	{
	}

	public MapCommandLevelAlreadyExistsException(string errorDescription, Guid levelId, Exception innerException)
		: this(errorDescription, innerException)
	{
		AddData("LevelId", levelId.ToString());
	}

	protected MapCommandLevelAlreadyExistsException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
