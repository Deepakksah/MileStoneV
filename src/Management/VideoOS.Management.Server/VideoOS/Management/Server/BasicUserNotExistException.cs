using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class BasicUserNotExistException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60355;

	protected override int ErrorCodeDefault => 60355;

	public BasicUserNotExistException()
		: base(VmoResources.VMO60355)
	{
	}

	[SqlError(60355)]
	public BasicUserNotExistException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}

	protected BasicUserNotExistException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
