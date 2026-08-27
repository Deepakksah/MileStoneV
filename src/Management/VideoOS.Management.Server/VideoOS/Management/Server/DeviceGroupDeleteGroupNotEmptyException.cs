using System;
using System.Runtime.Serialization;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class DeviceGroupDeleteGroupNotEmptyException : AdministrationSubsystemException
{
	protected override int ErrorCodeDefault => 60187;

	[SqlError(60187)]
	public DeviceGroupDeleteGroupNotEmptyException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.FormatMessage(), ex)
	{
		if (databaseError == null)
		{
			throw new ArgumentNullException("databaseError");
		}
		base.ErrorCode = databaseError.Number;
	}

	public DeviceGroupDeleteGroupNotEmptyException()
	{
	}

	public DeviceGroupDeleteGroupNotEmptyException(string message)
		: base(message)
	{
	}

	public DeviceGroupDeleteGroupNotEmptyException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected DeviceGroupDeleteGroupNotEmptyException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
