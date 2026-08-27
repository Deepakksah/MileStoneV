using System;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class ServerGroupDeleteException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60059;

	protected override int ErrorCodeDefault => 60059;

	public ServerGroupDeleteException()
		: base(VmoResources.VMO60059)
	{
	}

	[SqlError(60059)]
	public ServerGroupDeleteException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}
}
