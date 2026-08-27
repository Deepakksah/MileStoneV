using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class RetentionOptionRoleRelationDeleteException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60464;

	protected override int ErrorCodeDefault => 60464;

	public RetentionOptionRoleRelationDeleteException()
		: base(VmoResources.VMO60464)
	{
	}

	[SqlError(60464)]
	public RetentionOptionRoleRelationDeleteException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}

	protected RetentionOptionRoleRelationDeleteException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
