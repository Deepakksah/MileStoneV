using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class RetentionOptionRoleRelationCreationException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60461;

	protected override int ErrorCodeDefault => 60461;

	public RetentionOptionRoleRelationCreationException()
		: base(VmoResources.VMO60461)
	{
	}

	[SqlError(60461)]
	public RetentionOptionRoleRelationCreationException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}

	public RetentionOptionRoleRelationCreationException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected RetentionOptionRoleRelationCreationException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
