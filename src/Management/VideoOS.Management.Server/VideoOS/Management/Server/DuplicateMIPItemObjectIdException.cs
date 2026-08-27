using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class DuplicateMIPItemObjectIdException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60476;

	protected override int ErrorCodeDefault => 60476;

	public DuplicateMIPItemObjectIdException()
		: base(VmoResources.VMO60476)
	{
	}

	[SqlError(60476)]
	public DuplicateMIPItemObjectIdException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}

	protected DuplicateMIPItemObjectIdException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
