using System;
using System.Runtime.Serialization;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class DevicePtzPresetNameKeyConflictException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60320;

	protected override int ErrorCodeDefault => 60320;

	public DevicePtzPresetNameKeyConflictException()
	{
	}

	public DevicePtzPresetNameKeyConflictException(string message)
		: base(message)
	{
	}

	public DevicePtzPresetNameKeyConflictException(string message, Exception inner)
		: base(message, inner)
	{
	}

	[SqlError(60320)]
	public DevicePtzPresetNameKeyConflictException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.FormatMessage(), ex)
	{
		if (databaseError == null)
		{
			throw new ArgumentNullException("databaseError");
		}
		base.ErrorCode = databaseError.Number;
	}

	protected DevicePtzPresetNameKeyConflictException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
