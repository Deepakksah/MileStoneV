using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class MIPKindNotFoundException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60474;

	protected override int ErrorCodeDefault => 60474;

	public MIPKindNotFoundException()
		: base(VmoResources.VMO60474)
	{
	}

	[SqlError(60474)]
	public MIPKindNotFoundException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}

	protected MIPKindNotFoundException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
