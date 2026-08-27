using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class DatabaseConnectionException : GeminiServiceException
{
	protected override int ErrorCodeDefault => 60161;

	public DatabaseConnectionException(DatabaseException innerException)
		: base(VmoResources.VMO60161, innerException)
	{
		base.LogLevel = EventLogEntryType.Warning;
		base.EventId = GeminiEventId.DatabaseConnectionException;
	}

	public DatabaseConnectionException(InvalidOperationException innerException)
		: base(VmoResources.VMO60161, innerException)
	{
		base.LogLevel = EventLogEntryType.Warning;
		base.EventId = GeminiEventId.DatabaseConnectionException;
	}

	public DatabaseConnectionException(IGeminiRequestContext requestContext, DatabaseException ex, DatabaseError databaseError)
		: this(ex)
	{
	}

	protected DatabaseConnectionException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
		base.EventId = GeminiEventId.DatabaseConnectionException;
	}
}
