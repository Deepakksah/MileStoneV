using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class TimeProfileUpdateException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60221;

	protected override int ErrorCodeDefault => 60221;

	public TimeProfileUpdateException()
		: base(VmoResources.VMO60221)
	{
	}

	[SqlError(60221)]
	public TimeProfileUpdateException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}

	protected TimeProfileUpdateException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
