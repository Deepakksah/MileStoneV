using System;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class TaskDeleteTaskNotFoundException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60130;

	protected override int ErrorCodeDefault => 60130;

	public TaskDeleteTaskNotFoundException()
		: base(VmoResources.VMO60130)
	{
	}

	[SqlError(60130)]
	public TaskDeleteTaskNotFoundException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}
}
