using System;
using System.Runtime.Serialization;

namespace VideoOS.GisMapSharedData.CommandExceptions;

[Serializable]
[MapCommandException("NonUniqueMapId")]
public class MapCommandNonUniqueMapIdException : MapCommandException
{
	public MapCommandNonUniqueMapIdException()
		: base(MapCommandErrorReason.NonUniqueMapId, MapCommandErrorReason.NonUniqueMapId)
	{
	}

	public MapCommandNonUniqueMapIdException(string errorDescription)
		: base(errorDescription, MapCommandErrorReason.NonUniqueMapId)
	{
	}

	public MapCommandNonUniqueMapIdException(string errorDescription, Exception innerException)
		: base(errorDescription, MapCommandErrorReason.NonUniqueMapId, innerException)
	{
	}

	protected MapCommandNonUniqueMapIdException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
