using System;
using System.Runtime.Serialization;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class UpdateDriverSettingsValueOutOfRangeException : UpdateDriverSettingsException
{
	protected override int ErrorCodeDefault => 60196;

	[SqlError(60196)]
	public UpdateDriverSettingsValueOutOfRangeException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.FormatMessage(), ex)
	{
		if (databaseError == null)
		{
			throw new ArgumentNullException("databaseError");
		}
		base.ErrorCode = databaseError.Number;
	}

	public UpdateDriverSettingsValueOutOfRangeException(string message, string hwid, string capabilityName, Guid typeReferenceId, params MessageParameter[] parameters)
		: base(message, hwid, capabilityName, typeReferenceId, parameters)
	{
	}

	public UpdateDriverSettingsValueOutOfRangeException()
		: base(VmoResources.VMO60196)
	{
	}

	public UpdateDriverSettingsValueOutOfRangeException(string message)
		: base(message)
	{
	}

	public UpdateDriverSettingsValueOutOfRangeException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected UpdateDriverSettingsValueOutOfRangeException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
