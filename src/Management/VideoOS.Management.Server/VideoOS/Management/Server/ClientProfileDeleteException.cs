using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class ClientProfileDeleteException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60704;

	protected override int ErrorCodeDefault => 60704;

	public ClientProfileDeleteException()
		: base(VmoResources.VMO60704)
	{
	}

	[SqlError(60704)]
	public ClientProfileDeleteException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}

	protected ClientProfileDeleteException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
