using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class CustomSettingOutOfSyncException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 61033;

	protected override int ErrorCodeDefault => 61033;

	public CustomSettingOutOfSyncException()
		: base(VmoResources.VMO61033)
	{
	}

	[SqlError(61033)]
	public CustomSettingOutOfSyncException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}

	public CustomSettingOutOfSyncException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected CustomSettingOutOfSyncException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
