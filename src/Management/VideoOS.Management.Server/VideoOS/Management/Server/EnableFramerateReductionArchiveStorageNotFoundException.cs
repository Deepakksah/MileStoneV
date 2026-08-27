using System;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class EnableFramerateReductionArchiveStorageNotFoundException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60131;

	protected override int ErrorCodeDefault => 60131;

	public EnableFramerateReductionArchiveStorageNotFoundException()
		: base(VmoResources.VMO60131)
	{
	}

	[SqlError(60131)]
	public EnableFramerateReductionArchiveStorageNotFoundException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}
}
