using System;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class ServerGroupQueryException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60086;

	protected override int ErrorCodeDefault => 60086;

	public ServerGroupQueryException()
		: base(VmoResources.VMO60086)
	{
	}

	[SqlError(60086)]
	public ServerGroupQueryException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}
}
