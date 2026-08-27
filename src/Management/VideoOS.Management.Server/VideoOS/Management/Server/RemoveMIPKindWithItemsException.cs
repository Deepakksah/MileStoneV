using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class RemoveMIPKindWithItemsException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60475;

	protected override int ErrorCodeDefault => 60475;

	public RemoveMIPKindWithItemsException()
		: base(VmoResources.VMO60475)
	{
	}

	[SqlError(60475)]
	public RemoveMIPKindWithItemsException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}

	protected RemoveMIPKindWithItemsException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
