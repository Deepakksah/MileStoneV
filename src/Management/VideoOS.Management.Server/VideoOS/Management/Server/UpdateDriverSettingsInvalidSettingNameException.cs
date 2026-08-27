using System;
using System.Runtime.Serialization;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class UpdateDriverSettingsInvalidSettingNameException : UpdateDriverSettingsException
{
	protected override int ErrorCodeDefault => 60193;

	[SqlError(60193)]
	public UpdateDriverSettingsInvalidSettingNameException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.FormatMessage(), ex)
	{
		if (databaseError == null)
		{
			throw new ArgumentNullException("databaseError");
		}
		base.ErrorCode = databaseError.Number;
	}

	public UpdateDriverSettingsInvalidSettingNameException(string message, string hwid, string capabilityName, Guid typeReferenceId, params MessageParameter[] parameters)
		: base(message, hwid, capabilityName, typeReferenceId, parameters)
	{
	}

	public UpdateDriverSettingsInvalidSettingNameException()
		: base(VmoResources.VMO60193)
	{
	}

	public UpdateDriverSettingsInvalidSettingNameException(string message)
		: base(message)
	{
	}

	public UpdateDriverSettingsInvalidSettingNameException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected UpdateDriverSettingsInvalidSettingNameException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
