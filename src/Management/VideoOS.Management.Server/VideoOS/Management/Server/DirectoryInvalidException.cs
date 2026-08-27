using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

[Serializable]
public class DirectoryInvalidException : AdministrationSubsystemException
{
	protected override int ErrorCodeDefault => 69000;

	public DirectoryInvalidException()
		: base(VmoResources.VMO69000)
	{
	}

	public DirectoryInvalidException(string message)
		: base(message)
	{
	}

	public DirectoryInvalidException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected DirectoryInvalidException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
