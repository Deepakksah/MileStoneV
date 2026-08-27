using System;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class ServerGroupMoveToInvalidParentException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60072;

	protected override int ErrorCodeDefault => 60072;

	public ServerGroupMoveToInvalidParentException()
		: base(VmoResources.VMO60072)
	{
	}

	[SqlError(60072)]
	public ServerGroupMoveToInvalidParentException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}
}
