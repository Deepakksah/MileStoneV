using System;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class RemoveNonExistingMonitorException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60213;

	protected override int ErrorCodeDefault => 60213;

	public RemoveNonExistingMonitorException()
		: base(VmoResources.VMO60213)
	{
	}

	[SqlError(60213)]
	public RemoveNonExistingMonitorException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}
}
