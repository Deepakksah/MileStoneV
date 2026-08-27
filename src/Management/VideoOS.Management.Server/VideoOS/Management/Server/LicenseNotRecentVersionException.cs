using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class LicenseNotRecentVersionException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60252;

	protected override int ErrorCodeDefault => 60252;

	public LicenseNotRecentVersionException()
		: base(VmoResources.VMO60252)
	{
	}

	public LicenseNotRecentVersionException(string message)
		: base(message)
	{
	}

	public LicenseNotRecentVersionException(string message, Exception inner)
		: base(message, inner)
	{
	}

	[SqlError(60252)]
	public LicenseNotRecentVersionException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.FormatMessage(), ex)
	{
	}

	protected LicenseNotRecentVersionException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
