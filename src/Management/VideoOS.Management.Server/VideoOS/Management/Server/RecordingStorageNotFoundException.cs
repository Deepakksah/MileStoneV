using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class RecordingStorageNotFoundException : AdministrationSubsystemException
{
	protected override int ErrorCodeDefault => 60249;

	[SqlError(60249)]
	public RecordingStorageNotFoundException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.FormatMessage(), ex)
	{
		if (databaseError == null)
		{
			throw new ArgumentNullException("databaseError");
		}
		base.ErrorCode = databaseError.Number;
	}

	public RecordingStorageNotFoundException()
		: base(VmoResources.VMO60249)
	{
	}

	public RecordingStorageNotFoundException(string message)
		: base(message)
	{
	}

	public RecordingStorageNotFoundException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected RecordingStorageNotFoundException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
