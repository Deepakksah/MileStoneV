using System;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class RemoveNonExistingVideoWallException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60203;

	protected override int ErrorCodeDefault => 60203;

	public RemoveNonExistingVideoWallException()
		: base(VmoResources.VMO60203)
	{
	}

	[SqlError(60203)]
	public RemoveNonExistingVideoWallException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}
}
