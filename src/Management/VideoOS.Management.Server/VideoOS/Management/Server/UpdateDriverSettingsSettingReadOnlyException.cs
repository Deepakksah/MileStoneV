using System;
using System.Runtime.Serialization;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class UpdateDriverSettingsSettingReadOnlyException : UpdateDriverSettingsException
{
	protected override int ErrorCodeDefault => 60194;

	[SqlError(60194)]
	public UpdateDriverSettingsSettingReadOnlyException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.FormatMessage(), ex)
	{
		if (databaseError == null)
		{
			throw new ArgumentNullException("databaseError");
		}
		base.ErrorCode = databaseError.Number;
	}

	public UpdateDriverSettingsSettingReadOnlyException(string message, string hwid, string capabilityName, Guid typeReferenceId, params MessageParameter[] parameters)
		: base(message, hwid, capabilityName, typeReferenceId, parameters)
	{
	}

	public UpdateDriverSettingsSettingReadOnlyException()
		: base(VmoResources.VMO60194)
	{
	}

	public UpdateDriverSettingsSettingReadOnlyException(string message)
		: base(message)
	{
	}

	public UpdateDriverSettingsSettingReadOnlyException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected UpdateDriverSettingsSettingReadOnlyException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
