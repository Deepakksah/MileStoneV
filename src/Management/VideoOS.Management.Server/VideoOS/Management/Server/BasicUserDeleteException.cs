using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class BasicUserDeleteException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60354;

	protected override int ErrorCodeDefault => 60354;

	public BasicUserDeleteException()
		: base(VmoResources.VMO60354)
	{
	}

	[SqlError(60354)]
	public BasicUserDeleteException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}

	protected BasicUserDeleteException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
