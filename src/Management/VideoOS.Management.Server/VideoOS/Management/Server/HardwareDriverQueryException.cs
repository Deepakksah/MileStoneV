using System;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class HardwareDriverQueryException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60081;

	protected override int ErrorCodeDefault => 60081;

	public HardwareDriverQueryException()
		: base(VmoResources.VMO60081)
	{
	}

	[SqlError(60081)]
	public HardwareDriverQueryException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}
}
