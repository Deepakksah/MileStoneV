using System;
using System.Runtime.Serialization;

namespace VideoOS.GisMapSharedData.CommandExceptions;

[Serializable]
[MapCommandException("ArgumentIsNull")]
public class MapCommandArgumentNullException : MapCommandException
{
	public MapCommandArgumentNullException()
		: base(MapCommandErrorReason.ArgumentIsNull, MapCommandErrorReason.ArgumentIsNull)
	{
	}

	public MapCommandArgumentNullException(string errorDescription)
		: base(errorDescription, MapCommandErrorReason.ArgumentIsNull)
	{
	}

	public MapCommandArgumentNullException(string errorDescription, Exception innerException)
		: base(errorDescription, MapCommandErrorReason.ArgumentIsNull, innerException)
	{
	}

	protected MapCommandArgumentNullException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
