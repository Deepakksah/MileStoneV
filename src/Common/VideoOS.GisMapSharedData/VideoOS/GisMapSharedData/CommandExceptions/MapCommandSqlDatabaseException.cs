using System;
using System.Runtime.Serialization;

namespace VideoOS.GisMapSharedData.CommandExceptions;

[Serializable]
[MapCommandException("SqlDatabase")]
public class MapCommandSqlDatabaseException : MapCommandException
{
	public MapCommandSqlDatabaseException()
		: base(MapCommandErrorReason.SqlDatabase, MapCommandErrorReason.SqlDatabase)
	{
	}

	public MapCommandSqlDatabaseException(string errorDescription)
		: base(errorDescription, MapCommandErrorReason.SqlDatabase)
	{
	}

	public MapCommandSqlDatabaseException(string errorDescription, Exception innerException)
		: base(errorDescription, MapCommandErrorReason.SqlDatabase, innerException)
	{
	}

	protected MapCommandSqlDatabaseException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
