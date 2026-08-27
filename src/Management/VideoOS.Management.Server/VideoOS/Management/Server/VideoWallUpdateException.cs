using System;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class VideoWallUpdateException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60201;

	protected override int ErrorCodeDefault => 60201;

	public VideoWallUpdateException()
		: base(VmoResources.VMO60201)
	{
	}

	[SqlError(60201)]
	public VideoWallUpdateException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}
}
