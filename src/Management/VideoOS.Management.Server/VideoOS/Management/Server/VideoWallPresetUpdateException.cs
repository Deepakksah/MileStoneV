using System;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class VideoWallPresetUpdateException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60205;

	protected override int ErrorCodeDefault => 60205;

	public VideoWallPresetUpdateException()
		: base(VmoResources.VMO60205)
	{
	}

	[SqlError(60205)]
	public VideoWallPresetUpdateException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}
}
