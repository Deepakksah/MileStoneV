using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

[Serializable]
public class CreateArchiveStoragePathInvalidException : AdministrationSubsystemException
{
	protected override int ErrorCodeDefault => 60153;

	public CreateArchiveStoragePathInvalidException()
		: base(VmoResources.VMO60153)
	{
	}

	public CreateArchiveStoragePathInvalidException(string message)
		: base(message)
	{
	}

	public CreateArchiveStoragePathInvalidException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected CreateArchiveStoragePathInvalidException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
