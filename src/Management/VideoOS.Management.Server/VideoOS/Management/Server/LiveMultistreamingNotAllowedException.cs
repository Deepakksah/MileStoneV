using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class LiveMultistreamingNotAllowedException : DeviceStreamConfigurationException
{
	public const int SqlErrorCode = 60239;

	protected override int ErrorCodeDefault => 60239;

	public LiveMultistreamingNotAllowedException()
		: base(VmoResources.VMO60239)
	{
	}

	public LiveMultistreamingNotAllowedException(string message)
		: base(message)
	{
	}

	public LiveMultistreamingNotAllowedException(string message, Exception inner)
		: base(message, inner)
	{
	}

	[SqlError(60239)]
	public LiveMultistreamingNotAllowedException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}

	protected LiveMultistreamingNotAllowedException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
