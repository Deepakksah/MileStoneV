using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class HardwareNotRecentVersionException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60338;

	protected override int ErrorCodeDefault => 60338;

	public HardwareNotRecentVersionException()
		: base(VmoResources.VMO60338)
	{
	}

	public HardwareNotRecentVersionException(string message)
		: base(message)
	{
	}

	public HardwareNotRecentVersionException(string message, Exception inner)
		: base(message, inner)
	{
	}

	[SqlError(60338)]
	public HardwareNotRecentVersionException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}

	protected HardwareNotRecentVersionException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
