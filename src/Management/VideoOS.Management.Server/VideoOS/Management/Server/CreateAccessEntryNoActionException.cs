using System;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class CreateAccessEntryNoActionException : SecuritySubsystemException
{
	public const int SqlErrorCode = 60022;

	protected override int ErrorCodeDefault => 60022;

	public CreateAccessEntryNoActionException()
		: base(VmoResources.VMO60022)
	{
	}

	[SqlError(60022)]
	public CreateAccessEntryNoActionException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}
}
