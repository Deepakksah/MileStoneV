using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class BasicOwnerInfoNotExistException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60455;

	protected override int ErrorCodeDefault => 60455;

	public BasicOwnerInfoNotExistException()
		: base(VmoResources.VMO60455)
	{
	}

	[SqlError(60455)]
	public BasicOwnerInfoNotExistException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}

	protected BasicOwnerInfoNotExistException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
