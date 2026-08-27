using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class ViewItemUpdateException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60219;

	protected override int ErrorCodeDefault => 60219;

	public ViewItemUpdateException()
		: this(VmoResources.VMO60219)
	{
	}

	public ViewItemUpdateException(string message)
		: base(message)
	{
	}

	public ViewItemUpdateException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected ViewItemUpdateException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	[SqlError(60219)]
	public ViewItemUpdateException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}
}
