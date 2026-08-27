using System;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class ServerGroupMovePrimaryException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60076;

	protected override int ErrorCodeDefault => 60076;

	public ServerGroupMovePrimaryException()
		: base(VmoResources.VMO60076)
	{
	}

	[SqlError(60076)]
	public ServerGroupMovePrimaryException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}
}
