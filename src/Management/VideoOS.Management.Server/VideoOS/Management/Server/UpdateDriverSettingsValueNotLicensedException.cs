using System;
using System.Runtime.Serialization;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class UpdateDriverSettingsValueNotLicensedException : UpdateDriverSettingsException
{
	protected override int ErrorCodeDefault => 60253;

	[SqlError(60253)]
	public UpdateDriverSettingsValueNotLicensedException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.FormatMessage(), ex)
	{
		if (databaseError == null)
		{
			throw new ArgumentNullException("databaseError");
		}
		base.ErrorCode = databaseError.Number;
	}

	public UpdateDriverSettingsValueNotLicensedException(string message, string hwid, string capabilityName, Guid typeReferenceId, params MessageParameter[] parameters)
		: base(message, hwid, capabilityName, typeReferenceId, parameters)
	{
	}

	public UpdateDriverSettingsValueNotLicensedException()
		: base(VmoResources.VMO60253)
	{
	}

	public UpdateDriverSettingsValueNotLicensedException(string message)
		: base(message)
	{
	}

	public UpdateDriverSettingsValueNotLicensedException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected UpdateDriverSettingsValueNotLicensedException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
