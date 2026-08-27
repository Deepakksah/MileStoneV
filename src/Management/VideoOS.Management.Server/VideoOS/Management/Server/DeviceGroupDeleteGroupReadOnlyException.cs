using System;
using System.Runtime.Serialization;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class DeviceGroupDeleteGroupReadOnlyException : AdministrationSubsystemException
{
	protected override int ErrorCodeDefault => 60186;

	[SqlError(60186)]
	public DeviceGroupDeleteGroupReadOnlyException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.FormatMessage(), ex)
	{
		if (databaseError == null)
		{
			throw new ArgumentNullException("databaseError");
		}
		base.ErrorCode = databaseError.Number;
	}

	public DeviceGroupDeleteGroupReadOnlyException()
	{
	}

	public DeviceGroupDeleteGroupReadOnlyException(string message)
		: base(message)
	{
	}

	public DeviceGroupDeleteGroupReadOnlyException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected DeviceGroupDeleteGroupReadOnlyException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
