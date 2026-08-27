using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class RemoveNonExistingTimeProfileException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60223;

	protected override int ErrorCodeDefault => 60223;

	public RemoveNonExistingTimeProfileException()
		: base(VmoResources.VMO60223)
	{
	}

	[SqlError(60223)]
	public RemoveNonExistingTimeProfileException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}

	protected RemoveNonExistingTimeProfileException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
