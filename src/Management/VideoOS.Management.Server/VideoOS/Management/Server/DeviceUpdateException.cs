using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class DeviceUpdateException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60064;

	protected override int ErrorCodeDefault => 60064;

	public DeviceUpdateException()
		: base(VmoResources.VMO60064)
	{
	}

	[SqlError(60064)]
	[SqlError(60302)]
	public DeviceUpdateException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.FormatMessage(), ex)
	{
		if (databaseError == null)
		{
			throw new ArgumentNullException("databaseError");
		}
		base.ErrorCode = databaseError.Number;
	}

	public DeviceUpdateException(string message)
		: base(message)
	{
	}

	public DeviceUpdateException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected DeviceUpdateException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
