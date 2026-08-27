using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class ObjectNotFoundException : VmoServerServiceException
{
	protected override int ErrorCodeDefault => 61032;

	public ObjectNotFoundException()
		: base(VmoResources.VMO61032)
	{
	}

	[SqlError(60316)]
	[SqlError(60350)]
	[SqlError(60367)]
	[SqlError(60375)]
	[SqlError(60411)]
	[SqlError(60412)]
	[SqlError(60414)]
	public ObjectNotFoundException(DatabaseException ex, DatabaseError databaseError)
		: base(ex, databaseError)
	{
	}

	public ObjectNotFoundException(string message)
		: base(message)
	{
	}

	public ObjectNotFoundException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected ObjectNotFoundException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
