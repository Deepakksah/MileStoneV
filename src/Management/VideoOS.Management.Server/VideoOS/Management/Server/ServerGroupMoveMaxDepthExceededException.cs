using System;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class ServerGroupMoveMaxDepthExceededException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60088;

	protected override int ErrorCodeDefault => 60088;

	public ServerGroupMoveMaxDepthExceededException()
		: base(VmoResources.VMO60088)
	{
	}

	[SqlError(60088)]
	public ServerGroupMoveMaxDepthExceededException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}
}
