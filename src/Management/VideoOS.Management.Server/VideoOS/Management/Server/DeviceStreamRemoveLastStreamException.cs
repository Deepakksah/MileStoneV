using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class DeviceStreamRemoveLastStreamException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60228;

	protected override int ErrorCodeDefault => 60228;

	public DeviceStreamRemoveLastStreamException()
		: base(VmoResources.VMO60228)
	{
	}

	public DeviceStreamRemoveLastStreamException(string message)
		: base(message)
	{
	}

	public DeviceStreamRemoveLastStreamException(string message, Exception inner)
		: base(message, inner)
	{
	}

	[SqlError(60228)]
	public DeviceStreamRemoveLastStreamException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}

	protected DeviceStreamRemoveLastStreamException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
