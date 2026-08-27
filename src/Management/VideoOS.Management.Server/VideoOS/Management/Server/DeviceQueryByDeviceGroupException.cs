using System;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class DeviceQueryByDeviceGroupException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60090;

	protected override int ErrorCodeDefault => 60090;

	public DeviceQueryByDeviceGroupException()
		: base(VmoResources.VMO60090)
	{
	}

	[SqlError(60090)]
	public DeviceQueryByDeviceGroupException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}
}
