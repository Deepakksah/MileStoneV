using System;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class RemoveAdministratorException : SecuritySubsystemException
{
	public const int SqlErrorCode = 60010;

	protected override int ErrorCodeDefault => 60010;

	public RemoveAdministratorException()
		: base(VmoResources.VMO60010)
	{
	}

	[SqlError(60010)]
	public RemoveAdministratorException(DatabaseException ex, DatabaseError databaseError)
	{
	}
}
