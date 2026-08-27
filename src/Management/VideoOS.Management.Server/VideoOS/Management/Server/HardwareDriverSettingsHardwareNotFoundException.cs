using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class HardwareDriverSettingsHardwareNotFoundException : UpdateDriverSettingsException
{
	protected override int ErrorCodeDefault => 60096;

	[SqlError(60096)]
	public HardwareDriverSettingsHardwareNotFoundException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.FormatMessage(), ex)
	{
		if (databaseError == null)
		{
			throw new ArgumentNullException("databaseError");
		}
		base.ErrorCode = databaseError.Number;
	}

	public HardwareDriverSettingsHardwareNotFoundException()
		: base(VmoResources.VMO60096)
	{
	}

	public HardwareDriverSettingsHardwareNotFoundException(string message)
		: base(message)
	{
	}

	public HardwareDriverSettingsHardwareNotFoundException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected HardwareDriverSettingsHardwareNotFoundException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
