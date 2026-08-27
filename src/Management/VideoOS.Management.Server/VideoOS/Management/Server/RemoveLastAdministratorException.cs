using System;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class RemoveLastAdministratorException : SecuritySubsystemException
{
	public const int SqlErrorCode = 66040;

	protected override int ErrorCodeDefault => 66040;

	public RemoveLastAdministratorException()
		: base(VmoResources.VMO66040)
	{
	}

	[SqlError(66040)]
	public RemoveLastAdministratorException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}
}
