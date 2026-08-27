using System;
using System.Runtime.Serialization;

namespace VideoOS.GisMapSharedData.CommandExceptions;

[Serializable]
[MapCommandException("SanityCheckFailed")]
public class MapCommandSanityCheckFailedException : MapCommandException
{
	public MapCommandSanityCheckFailedException()
		: base(MapCommandErrorReason.SanityCheckFailed, MapCommandErrorReason.SanityCheckFailed)
	{
	}

	public MapCommandSanityCheckFailedException(string errorDescription)
		: base(errorDescription, MapCommandErrorReason.SanityCheckFailed)
	{
	}

	public MapCommandSanityCheckFailedException(string errorDescription, Exception innerException)
		: base(errorDescription, MapCommandErrorReason.SanityCheckFailed, innerException)
	{
	}

	protected MapCommandSanityCheckFailedException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
