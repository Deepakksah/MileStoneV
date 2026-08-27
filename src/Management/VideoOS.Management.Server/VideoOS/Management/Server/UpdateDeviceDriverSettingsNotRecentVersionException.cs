using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class UpdateDeviceDriverSettingsNotRecentVersionException : UpdateDriverSettingsException
{
	protected override int ErrorCodeDefault => 60192;

	[SqlError(60192)]
	public UpdateDeviceDriverSettingsNotRecentVersionException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.FormatMessage(), ex)
	{
		if (databaseError == null)
		{
			throw new ArgumentNullException("databaseError");
		}
		base.ErrorCode = databaseError.Number;
	}

	public UpdateDeviceDriverSettingsNotRecentVersionException()
		: base(VmoResources.VMO60192)
	{
	}

	public UpdateDeviceDriverSettingsNotRecentVersionException(string message)
		: base(message)
	{
	}

	public UpdateDeviceDriverSettingsNotRecentVersionException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected UpdateDeviceDriverSettingsNotRecentVersionException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
