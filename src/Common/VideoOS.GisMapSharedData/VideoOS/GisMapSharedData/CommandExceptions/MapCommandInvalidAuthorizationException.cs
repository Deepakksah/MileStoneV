using System;
using System.Runtime.Serialization;

namespace VideoOS.GisMapSharedData.CommandExceptions;

[Serializable]
[MapCommandException("InvalidAuthorization")]
public class MapCommandInvalidAuthorizationException : MapCommandException
{
	public MapCommandInvalidAuthorizationException()
		: base(MapCommandErrorReason.InvalidAuthorization, MapCommandErrorReason.InvalidAuthorization)
	{
	}

	public MapCommandInvalidAuthorizationException(string errorDescription)
		: base(errorDescription, MapCommandErrorReason.InvalidAuthorization)
	{
	}

	public MapCommandInvalidAuthorizationException(string errorDescription, Exception innerException)
		: base(errorDescription, MapCommandErrorReason.InvalidAuthorization, innerException)
	{
	}

	protected MapCommandInvalidAuthorizationException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
