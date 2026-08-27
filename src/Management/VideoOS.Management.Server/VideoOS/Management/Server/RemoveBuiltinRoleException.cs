using System;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class RemoveBuiltinRoleException : SecuritySubsystemException
{
	public const int SqlErrorCode = 60005;

	protected override int ErrorCodeDefault => 60005;

	public RemoveBuiltinRoleException()
		: base(VmoResources.VMO60005)
	{
	}

	[SqlError(60005)]
	public RemoveBuiltinRoleException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}
}
