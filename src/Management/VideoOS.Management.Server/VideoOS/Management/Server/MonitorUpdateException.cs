using System;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class MonitorUpdateException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60211;

	protected override int ErrorCodeDefault => 60211;

	public MonitorUpdateException()
		: base(VmoResources.VMO60211)
	{
	}

	[SqlError(60211)]
	public MonitorUpdateException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}
}
