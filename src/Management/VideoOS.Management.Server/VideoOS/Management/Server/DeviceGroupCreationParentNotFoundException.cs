using System;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class DeviceGroupCreationParentNotFoundException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60106;

	protected override int ErrorCodeDefault => 60106;

	public DeviceGroupCreationParentNotFoundException()
		: base(VmoResources.VMO60106)
	{
	}

	[SqlError(60106)]
	public DeviceGroupCreationParentNotFoundException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}
}
