using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class DriverSettingsHardwareNotFoundException : AdministrationSubsystemException
{
	protected override int ErrorCodeDefault => 60190;

	[SqlError(60190)]
	public DriverSettingsHardwareNotFoundException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.FormatMessage(), ex)
	{
		if (databaseError == null)
		{
			throw new ArgumentNullException("databaseError");
		}
		base.ErrorCode = databaseError.Number;
	}

	public DriverSettingsHardwareNotFoundException()
		: base(VmoResources.VMO60190)
	{
	}

	public DriverSettingsHardwareNotFoundException(string message)
		: base(message)
	{
	}

	public DriverSettingsHardwareNotFoundException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected DriverSettingsHardwareNotFoundException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
