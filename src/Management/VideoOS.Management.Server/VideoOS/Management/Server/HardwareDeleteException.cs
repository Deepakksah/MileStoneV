using System;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class HardwareDeleteException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60082;

	protected override int ErrorCodeDefault => 60082;

	[SqlError(60082)]
	public HardwareDeleteException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}

	public HardwareDeleteException()
		: base(VmoResources.VMO60082)
	{
	}
}
