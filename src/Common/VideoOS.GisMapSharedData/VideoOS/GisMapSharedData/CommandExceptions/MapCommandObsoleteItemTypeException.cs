using System;
using System.Runtime.Serialization;

namespace VideoOS.GisMapSharedData.CommandExceptions;

[Serializable]
[MapCommandException("ObsoleteItemType")]
public class MapCommandObsoleteItemTypeException : MapCommandException
{
	public MapCommandObsoleteItemTypeException()
		: base(MapCommandErrorReason.ObsoleteItemType, MapCommandErrorReason.ObsoleteItemType)
	{
	}

	public MapCommandObsoleteItemTypeException(string errorDescription)
		: base(errorDescription, MapCommandErrorReason.ObsoleteItemType)
	{
	}

	public MapCommandObsoleteItemTypeException(string errorDescription, Exception innerException)
		: base(errorDescription, MapCommandErrorReason.ObsoleteItemType, innerException)
	{
	}

	protected MapCommandObsoleteItemTypeException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
