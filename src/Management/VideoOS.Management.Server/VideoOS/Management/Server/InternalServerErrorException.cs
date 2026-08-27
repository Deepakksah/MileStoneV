using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class InternalServerErrorException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 65002;

	protected override int ErrorCodeDefault => 65002;

	public InternalServerErrorException()
		: base(VmoResources.VMO65002)
	{
	}

	public InternalServerErrorException(string message)
		: base(message)
	{
	}

	public InternalServerErrorException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected InternalServerErrorException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	[SqlError(65002)]
	public InternalServerErrorException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}
}
