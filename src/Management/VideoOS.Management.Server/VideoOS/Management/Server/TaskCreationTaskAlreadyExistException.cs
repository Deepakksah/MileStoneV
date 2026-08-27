using System;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class TaskCreationTaskAlreadyExistException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60128;

	protected override int ErrorCodeDefault => 60128;

	public TaskCreationTaskAlreadyExistException()
		: base(VmoResources.VMO60128)
	{
	}

	[SqlError(60128)]
	public TaskCreationTaskAlreadyExistException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}
}
