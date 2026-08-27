using System;
using System.Runtime.Serialization;

namespace VideoOS.GisMapSharedData.CommandExceptions;

[Serializable]
[MapCommandException("InvalidAuthentication")]
public class MapCommandInvalidAuthenticationException : MapCommandException
{
	public MapCommandInvalidAuthenticationException()
		: base(MapCommandErrorReason.InvalidAuthentication, MapCommandErrorReason.InvalidAuthentication)
	{
	}

	public MapCommandInvalidAuthenticationException(string errorDescription)
		: base(errorDescription, MapCommandErrorReason.InvalidAuthentication)
	{
	}

	public MapCommandInvalidAuthenticationException(string errorDescription, Exception innerException)
		: base(errorDescription, MapCommandErrorReason.InvalidAuthentication, innerException)
	{
	}

	protected MapCommandInvalidAuthenticationException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
