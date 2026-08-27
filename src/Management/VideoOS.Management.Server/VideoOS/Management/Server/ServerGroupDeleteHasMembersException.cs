using System;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class ServerGroupDeleteHasMembersException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60074;

	protected override int ErrorCodeDefault => 60074;

	public ServerGroupDeleteHasMembersException()
		: base(VmoResources.VMO60074)
	{
	}

	[SqlError(60074)]
	public ServerGroupDeleteHasMembersException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}
}
