using System;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class ServerGroupRemovePrivateException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60067;

	protected override int ErrorCodeDefault => 60067;

	public ServerGroupRemovePrivateException()
		: base(VmoResources.VMO60067)
	{
	}

	[SqlError(60067)]
	public ServerGroupRemovePrivateException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}
}
