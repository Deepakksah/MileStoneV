using System;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class RemoveNonExistingCustomPropertyException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60250;

	protected override int ErrorCodeDefault => 60250;

	public RemoveNonExistingCustomPropertyException()
		: base(VmoResources.VMO60250)
	{
	}

	[SqlError(60250)]
	public RemoveNonExistingCustomPropertyException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}
}
