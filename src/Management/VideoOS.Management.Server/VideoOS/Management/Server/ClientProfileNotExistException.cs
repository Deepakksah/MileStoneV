using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class ClientProfileNotExistException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60705;

	protected override int ErrorCodeDefault => 60705;

	public ClientProfileNotExistException()
		: base(VmoResources.VMO60705)
	{
	}

	[SqlError(60705)]
	public ClientProfileNotExistException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}

	protected ClientProfileNotExistException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
