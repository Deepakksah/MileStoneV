using System;
using System.Runtime.Serialization;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class UpdateDriverSettingsNotSupportedException : UpdateDriverSettingsException
{
	protected override int ErrorCodeDefault => 60195;

	[SqlError(60195)]
	public UpdateDriverSettingsNotSupportedException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.FormatMessage(), ex)
	{
		if (databaseError == null)
		{
			throw new ArgumentNullException("databaseError");
		}
		base.ErrorCode = databaseError.Number;
	}

	public UpdateDriverSettingsNotSupportedException(string message, string hwid, string capabilityName, Guid typeReferenceId, params MessageParameter[] parameters)
		: base(message, hwid, capabilityName, typeReferenceId, parameters)
	{
	}

	public UpdateDriverSettingsNotSupportedException()
		: base(VmoResources.VMO60195)
	{
	}

	public UpdateDriverSettingsNotSupportedException(string message)
		: base(message)
	{
	}

	public UpdateDriverSettingsNotSupportedException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected UpdateDriverSettingsNotSupportedException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
