using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class RemoveNonExistingViewException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60217;

	protected override int ErrorCodeDefault => 60217;

	public RemoveNonExistingViewException()
		: this(VmoResources.VMO60217)
	{
	}

	public RemoveNonExistingViewException(string message)
		: base(message)
	{
	}

	public RemoveNonExistingViewException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected RemoveNonExistingViewException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	[SqlError(60217)]
	public RemoveNonExistingViewException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}
}
