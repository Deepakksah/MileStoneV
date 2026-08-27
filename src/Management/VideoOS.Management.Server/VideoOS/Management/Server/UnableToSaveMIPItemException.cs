using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class UnableToSaveMIPItemException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60477;

	protected override int ErrorCodeDefault => 60477;

	public UnableToSaveMIPItemException()
		: base(VmoResources.VMO60477)
	{
	}

	[SqlError(60477)]
	public UnableToSaveMIPItemException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}

	protected UnableToSaveMIPItemException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
