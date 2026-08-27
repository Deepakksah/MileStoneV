using System;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class ServerGroupNoGroupException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60056;

	protected override int ErrorCodeDefault => 60056;

	public ServerGroupNoGroupException()
		: base(VmoResources.VMO60056)
	{
	}

	[SqlError(60056)]
	public ServerGroupNoGroupException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}
}
