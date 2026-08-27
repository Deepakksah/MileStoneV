using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class RemoveNonExistingMIPItemException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60473;

	protected override int ErrorCodeDefault => 60473;

	public RemoveNonExistingMIPItemException()
		: base(VmoResources.VMO60473)
	{
	}

	[SqlError(60473)]
	public RemoveNonExistingMIPItemException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}

	protected RemoveNonExistingMIPItemException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
