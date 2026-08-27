using System;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class RemoveNonExistingVideoWallPresetException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60207;

	protected override int ErrorCodeDefault => 60207;

	public RemoveNonExistingVideoWallPresetException()
		: base(VmoResources.VMO60207)
	{
	}

	[SqlError(60207)]
	public RemoveNonExistingVideoWallPresetException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}
}
