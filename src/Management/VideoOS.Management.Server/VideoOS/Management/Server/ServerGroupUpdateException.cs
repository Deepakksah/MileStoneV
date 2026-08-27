using System;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class ServerGroupUpdateException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60060;

	protected override int ErrorCodeDefault => 60060;

	public ServerGroupUpdateException()
		: base(VmoResources.VMO60060)
	{
	}

	[SqlError(60060)]
	public ServerGroupUpdateException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}
}
