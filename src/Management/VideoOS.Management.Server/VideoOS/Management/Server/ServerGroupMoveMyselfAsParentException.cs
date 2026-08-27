using System;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class ServerGroupMoveMyselfAsParentException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60078;

	protected override int ErrorCodeDefault => 60078;

	public ServerGroupMoveMyselfAsParentException()
		: base(VmoResources.VMO60078)
	{
	}

	[SqlError(60078)]
	public ServerGroupMoveMyselfAsParentException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}
}
