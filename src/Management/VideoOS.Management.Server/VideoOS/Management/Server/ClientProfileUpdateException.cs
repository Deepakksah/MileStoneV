using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class ClientProfileUpdateException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60702;

	protected override int ErrorCodeDefault => 60702;

	public ClientProfileUpdateException()
		: base(VmoResources.VMO60702)
	{
	}

	[SqlError(60702)]
	public ClientProfileUpdateException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}

	protected ClientProfileUpdateException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
