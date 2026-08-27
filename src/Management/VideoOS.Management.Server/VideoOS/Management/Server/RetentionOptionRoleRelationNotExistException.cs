using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class RetentionOptionRoleRelationNotExistException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60465;

	protected override int ErrorCodeDefault => 60465;

	public RetentionOptionRoleRelationNotExistException()
		: base(VmoResources.VMO60465)
	{
	}

	[SqlError(60465)]
	public RetentionOptionRoleRelationNotExistException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}

	protected RetentionOptionRoleRelationNotExistException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
