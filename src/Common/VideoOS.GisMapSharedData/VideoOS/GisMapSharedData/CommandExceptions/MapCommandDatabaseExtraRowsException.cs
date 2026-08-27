using System;
using System.Runtime.Serialization;

namespace VideoOS.GisMapSharedData.CommandExceptions;

[Serializable]
[MapCommandException("DatabaseExtraRows")]
public class MapCommandDatabaseExtraRowsException : MapCommandException
{
	public MapCommandDatabaseExtraRowsException()
		: base(MapCommandErrorReason.DatabaseExtraRows, MapCommandErrorReason.DatabaseExtraRows)
	{
	}

	public MapCommandDatabaseExtraRowsException(string errorDescription)
		: base(errorDescription, MapCommandErrorReason.DatabaseExtraRows)
	{
	}

	public MapCommandDatabaseExtraRowsException(string errorDescription, Exception innerException)
		: base(errorDescription, MapCommandErrorReason.DatabaseExtraRows, innerException)
	{
	}

	protected MapCommandDatabaseExtraRowsException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
