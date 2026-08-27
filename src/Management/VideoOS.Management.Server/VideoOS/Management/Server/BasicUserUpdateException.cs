using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class BasicUserUpdateException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60352;

	protected override int ErrorCodeDefault => 60352;

	public BasicUserUpdateException()
		: base(VmoResources.VMO60352)
	{
	}

	[SqlError(60352)]
	public BasicUserUpdateException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}

	protected BasicUserUpdateException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
