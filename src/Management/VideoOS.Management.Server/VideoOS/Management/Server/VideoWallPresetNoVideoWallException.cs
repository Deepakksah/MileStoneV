using System;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class VideoWallPresetNoVideoWallException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60209;

	protected override int ErrorCodeDefault => 60209;

	public VideoWallPresetNoVideoWallException()
		: base(VmoResources.VMO60209)
	{
	}

	[SqlError(60209)]
	public VideoWallPresetNoVideoWallException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}
}
