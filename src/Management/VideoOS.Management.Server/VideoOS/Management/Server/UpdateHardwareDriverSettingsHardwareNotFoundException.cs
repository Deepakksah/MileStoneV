using System;
using System.Runtime.Serialization;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class UpdateHardwareDriverSettingsHardwareNotFoundException : UpdateDriverSettingsException
{
	protected override int ErrorCodeDefault => 60127;

	[SqlError(60127)]
	public UpdateHardwareDriverSettingsHardwareNotFoundException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.FormatMessage(), ex)
	{
		if (databaseError == null)
		{
			throw new ArgumentNullException("databaseError");
		}
		base.ErrorCode = databaseError.Number;
	}

	public UpdateHardwareDriverSettingsHardwareNotFoundException(string message, params MessageParameter[] parameters)
		: base(message)
	{
		if (parameters != null)
		{
			foreach (MessageParameter messageParameter in parameters)
			{
				base.MessageParameters.Add(messageParameter.Index, messageParameter);
			}
		}
	}

	public UpdateHardwareDriverSettingsHardwareNotFoundException()
		: base(VmoResources.VMO60127)
	{
	}

	public UpdateHardwareDriverSettingsHardwareNotFoundException(string message)
		: base(message)
	{
	}

	public UpdateHardwareDriverSettingsHardwareNotFoundException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected UpdateHardwareDriverSettingsHardwareNotFoundException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
