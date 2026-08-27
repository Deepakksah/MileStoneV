using System;
using System.Runtime.Serialization;

namespace VideoOS.GisMapSharedData.CommandExceptions;

[Serializable]
[MapCommandException("RepositoryNotFound")]
public class MapCommandRepositoryNotFoundException : MapCommandException
{
	public MapCommandRepositoryNotFoundException()
		: base(MapCommandErrorReason.RepositoryNotFound, MapCommandErrorReason.RepositoryNotFound)
	{
	}

	public MapCommandRepositoryNotFoundException(string errorDescription)
		: base(errorDescription, MapCommandErrorReason.RepositoryNotFound)
	{
	}

	public MapCommandRepositoryNotFoundException(string errorDescription, Exception innerException)
		: base(errorDescription, MapCommandErrorReason.RepositoryNotFound, innerException)
	{
	}

	protected MapCommandRepositoryNotFoundException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
