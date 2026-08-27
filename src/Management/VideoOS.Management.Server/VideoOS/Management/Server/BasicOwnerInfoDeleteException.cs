using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class BasicOwnerInfoDeleteException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60454;

	protected override int ErrorCodeDefault => 60454;

	public BasicOwnerInfoDeleteException()
		: base(VmoResources.VMO60454)
	{
	}

	[SqlError(60454)]
	public BasicOwnerInfoDeleteException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}

	protected BasicOwnerInfoDeleteException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
