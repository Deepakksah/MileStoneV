using System;
using System.Runtime.Serialization;

namespace VideoOS.GisMapSharedData.CommandExceptions;

[Serializable]
[MapCommandException("ItemNotFound")]
public class MapCommandItemNotFoundException : MapCommandException
{
	public MapCommandItemNotFoundException()
		: base(MapCommandErrorReason.ItemNotFound, MapCommandErrorReason.ItemNotFound)
	{
	}

	public MapCommandItemNotFoundException(string errorDescription)
		: base(errorDescription, MapCommandErrorReason.ItemNotFound)
	{
	}

	public MapCommandItemNotFoundException(string errorDescription, Exception innerException)
		: base(errorDescription, MapCommandErrorReason.ItemNotFound, innerException)
	{
	}

	protected MapCommandItemNotFoundException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
