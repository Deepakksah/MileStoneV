using System;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class ServerGroupDeletePrimaryException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60075;

	protected override int ErrorCodeDefault => 60075;

	public ServerGroupDeletePrimaryException()
		: base(VmoResources.VMO60075)
	{
	}

	[SqlError(60075)]
	public ServerGroupDeletePrimaryException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}
}
