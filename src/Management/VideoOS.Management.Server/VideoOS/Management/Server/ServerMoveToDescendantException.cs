using System;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class ServerMoveToDescendantException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60077;

	protected override int ErrorCodeDefault => 60077;

	public ServerMoveToDescendantException()
		: base(VmoResources.VMO60077)
	{
	}

	[SqlError(60077)]
	public ServerMoveToDescendantException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}
}
