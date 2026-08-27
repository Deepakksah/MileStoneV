using System;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class ServerGroupMaxDepthExceededException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60057;

	protected override int ErrorCodeDefault => 60057;

	public ServerGroupMaxDepthExceededException()
		: base(VmoResources.VMO60057)
	{
	}

	[SqlError(60057)]
	public ServerGroupMaxDepthExceededException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}
}
