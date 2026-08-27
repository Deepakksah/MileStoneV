using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class UpdateDeviceDriverSettingsDeviceNotFoundException : UpdateDriverSettingsException
{
	protected override int ErrorCodeDefault => 60191;

	[SqlError(60191)]
	public UpdateDeviceDriverSettingsDeviceNotFoundException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.FormatMessage(), ex)
	{
		if (databaseError == null)
		{
			throw new ArgumentNullException("databaseError");
		}
		base.ErrorCode = databaseError.Number;
	}

	public UpdateDeviceDriverSettingsDeviceNotFoundException()
		: base(VmoResources.VMO60191)
	{
	}

	public UpdateDeviceDriverSettingsDeviceNotFoundException(string message)
		: base(message)
	{
	}

	public UpdateDeviceDriverSettingsDeviceNotFoundException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected UpdateDeviceDriverSettingsDeviceNotFoundException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
