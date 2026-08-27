using System;
using System.Runtime.Serialization;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class UpdateHardwareDriverSettingsNotRecentVersionException : UpdateDriverSettingsException
{
	protected override int ErrorCodeDefault => 60053;

	[SqlError(60053)]
	public UpdateHardwareDriverSettingsNotRecentVersionException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.FormatMessage(), ex)
	{
		if (databaseError == null)
		{
			throw new ArgumentNullException("databaseError");
		}
		base.ErrorCode = databaseError.Number;
	}

	public UpdateHardwareDriverSettingsNotRecentVersionException(string message, params MessageParameter[] parameters)
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

	public UpdateHardwareDriverSettingsNotRecentVersionException()
		: base(VmoResources.VMO60053)
	{
	}

	public UpdateHardwareDriverSettingsNotRecentVersionException(string message)
		: base(message)
	{
	}

	public UpdateHardwareDriverSettingsNotRecentVersionException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected UpdateHardwareDriverSettingsNotRecentVersionException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
