using System;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class DeviceGroupUpdateGroupNotFoundException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60109;

	protected override int ErrorCodeDefault => 60109;

	public DeviceGroupUpdateGroupNotFoundException()
		: base(VmoResources.VMO60109)
	{
	}

	[SqlError(60109)]
	public DeviceGroupUpdateGroupNotFoundException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}
}
