using System;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class DeviceGroupUpdateReadOnlyException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60110;

	protected override int ErrorCodeDefault => 60110;

	public DeviceGroupUpdateReadOnlyException()
		: base(VmoResources.VMO60110)
	{
	}

	[SqlError(60110)]
	public DeviceGroupUpdateReadOnlyException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}
}
