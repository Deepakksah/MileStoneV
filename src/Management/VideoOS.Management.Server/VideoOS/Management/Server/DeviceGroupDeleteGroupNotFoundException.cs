using System;
using System.Runtime.Serialization;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class DeviceGroupDeleteGroupNotFoundException : AdministrationSubsystemException
{
	protected override int ErrorCodeDefault => 60185;

	[SqlError(60185)]
	public DeviceGroupDeleteGroupNotFoundException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.FormatMessage(), ex)
	{
		if (databaseError == null)
		{
			throw new ArgumentNullException("databaseError");
		}
		base.ErrorCode = databaseError.Number;
	}

	public DeviceGroupDeleteGroupNotFoundException()
	{
	}

	public DeviceGroupDeleteGroupNotFoundException(string message)
		: base(message)
	{
	}

	public DeviceGroupDeleteGroupNotFoundException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected DeviceGroupDeleteGroupNotFoundException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
