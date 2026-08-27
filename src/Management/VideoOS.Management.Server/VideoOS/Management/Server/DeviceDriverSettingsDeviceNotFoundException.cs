using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class DeviceDriverSettingsDeviceNotFoundException : UpdateDriverSettingsException
{
	protected override int ErrorCodeDefault => 60188;

	[SqlError(60188)]
	public DeviceDriverSettingsDeviceNotFoundException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.FormatMessage(), ex)
	{
		if (databaseError == null)
		{
			throw new ArgumentNullException("databaseError");
		}
		base.ErrorCode = databaseError.Number;
	}

	public DeviceDriverSettingsDeviceNotFoundException()
		: base(VmoResources.VMO60188)
	{
	}

	public DeviceDriverSettingsDeviceNotFoundException(string message)
		: base(message)
	{
	}

	public DeviceDriverSettingsDeviceNotFoundException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected DeviceDriverSettingsDeviceNotFoundException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
