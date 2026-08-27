using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class MonitorPresetUpdateException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60220;

	protected override int ErrorCodeDefault => 60220;

	public MonitorPresetUpdateException()
		: this(VmoResources.VMO60220)
	{
	}

	public MonitorPresetUpdateException(string message)
		: base(message)
	{
	}

	public MonitorPresetUpdateException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected MonitorPresetUpdateException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	[SqlError(60220)]
	public MonitorPresetUpdateException(DatabaseException ex, DatabaseError databaseError)
	{
	}
}
