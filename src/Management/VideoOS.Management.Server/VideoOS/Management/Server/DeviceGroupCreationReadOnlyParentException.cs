using System;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class DeviceGroupCreationReadOnlyParentException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60107;

	protected override int ErrorCodeDefault => 60107;

	public DeviceGroupCreationReadOnlyParentException()
		: base(VmoResources.VMO60107)
	{
	}

	[SqlError(60107)]
	public DeviceGroupCreationReadOnlyParentException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}
}
