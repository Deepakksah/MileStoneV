using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class BasicOwnerInfoUpdateException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60452;

	protected override int ErrorCodeDefault => 60452;

	public BasicOwnerInfoUpdateException()
		: base(VmoResources.VMO60452)
	{
	}

	[SqlError(60452)]
	public BasicOwnerInfoUpdateException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}

	protected BasicOwnerInfoUpdateException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
