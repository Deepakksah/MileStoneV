using System;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class ServerGroupMoveUnknownException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60073;

	protected override int ErrorCodeDefault => 60073;

	public ServerGroupMoveUnknownException()
		: base(VmoResources.VMO60073)
	{
	}

	[SqlError(60073)]
	public ServerGroupMoveUnknownException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}
}
