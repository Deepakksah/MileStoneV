using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class HardwareUpdateException : AdministrationSubsystemException
{
	protected override int ErrorCodeDefault => 60058;

	[SqlError(60058)]
	[SqlError(60245)]
	[SqlError(60246)]
	public HardwareUpdateException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.FormatMessage(), ex)
	{
		if (databaseError == null)
		{
			throw new ArgumentNullException("databaseError");
		}
		base.ErrorCode = databaseError.Number;
	}

	public HardwareUpdateException()
		: base(VmoResources.VMO60058)
	{
	}

	public HardwareUpdateException(string message)
		: base(message)
	{
	}

	public HardwareUpdateException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected HardwareUpdateException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
