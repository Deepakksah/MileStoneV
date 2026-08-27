using System;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class DeviceNotFoundException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60071;

	protected override int ErrorCodeDefault => 60071;

	public DeviceNotFoundException()
		: base(VmoResources.VMO60071)
	{
	}

	[SqlError(60071)]
	public DeviceNotFoundException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}
}
