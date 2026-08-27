using System;
using System.Runtime.Serialization;

namespace VideoOS.GisMapSharedData.CommandExceptions;

[Serializable]
[MapCommandException("InvalidType")]
public class MapCommandInvalidTypeException : MapCommandException
{
	public MapCommandInvalidTypeException()
		: base(MapCommandErrorReason.InvalidType, MapCommandErrorReason.InvalidType)
	{
	}

	public MapCommandInvalidTypeException(string errorDescription)
		: base(errorDescription, MapCommandErrorReason.InvalidType)
	{
	}

	public MapCommandInvalidTypeException(string errorDescription, Exception innerException)
		: base(errorDescription, MapCommandErrorReason.InvalidType, innerException)
	{
	}

	protected MapCommandInvalidTypeException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
