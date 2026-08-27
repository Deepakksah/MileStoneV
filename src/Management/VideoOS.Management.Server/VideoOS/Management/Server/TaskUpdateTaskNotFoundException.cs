using System;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class TaskUpdateTaskNotFoundException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60129;

	protected override int ErrorCodeDefault => 60129;

	public TaskUpdateTaskNotFoundException()
		: base(VmoResources.VMO60129)
	{
	}

	[SqlError(60129)]
	public TaskUpdateTaskNotFoundException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}
}
