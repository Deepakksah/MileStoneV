using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class DeleteArchiveStorageException : AdministrationSubsystemException
{
	private const int sqlError = 60151;

	protected override int ErrorCodeDefault => 60151;

	public DeleteArchiveStorageException()
	{
	}

	public DeleteArchiveStorageException(string message)
		: base(message)
	{
	}

	public DeleteArchiveStorageException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected DeleteArchiveStorageException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	[SqlError(60151)]
	public DeleteArchiveStorageException(DatabaseException ex, DatabaseError databaseError)
		: this(VmoResources.VMO60151)
	{
	}
}
