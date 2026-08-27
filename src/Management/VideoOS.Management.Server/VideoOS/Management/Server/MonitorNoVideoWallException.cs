using System;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class MonitorNoVideoWallException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60208;

	protected override int ErrorCodeDefault => 60208;

	public MonitorNoVideoWallException()
		: base(VmoResources.VMO60208)
	{
	}

	[SqlError(60208)]
	public MonitorNoVideoWallException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}
}
