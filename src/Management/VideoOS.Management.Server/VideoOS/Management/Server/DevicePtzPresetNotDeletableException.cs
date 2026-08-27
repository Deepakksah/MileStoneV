using System;
using System.Runtime.Serialization;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class DevicePtzPresetNotDeletableException : AdministrationSubsystemException
{
	protected override int ErrorCodeDefault => 60270;

	[SqlError(60270)]
	public DevicePtzPresetNotDeletableException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.FormatMessage(), ex)
	{
		if (databaseError == null)
		{
			throw new ArgumentNullException("databaseError");
		}
		base.ErrorCode = databaseError.Number;
	}

	public DevicePtzPresetNotDeletableException()
	{
	}

	public DevicePtzPresetNotDeletableException(string message)
		: base(message)
	{
	}

	public DevicePtzPresetNotDeletableException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected DevicePtzPresetNotDeletableException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
