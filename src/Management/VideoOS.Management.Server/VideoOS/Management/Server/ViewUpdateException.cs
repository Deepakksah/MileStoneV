using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class ViewUpdateException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60215;

	protected override int ErrorCodeDefault => 60215;

	public ViewUpdateException()
		: this(VmoResources.VMO60215)
	{
	}

	public ViewUpdateException(string message)
		: base(message)
	{
	}

	public ViewUpdateException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected ViewUpdateException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	[SqlError(60215)]
	public ViewUpdateException(DatabaseException ex, DatabaseError databaseError)
	{
	}
}
