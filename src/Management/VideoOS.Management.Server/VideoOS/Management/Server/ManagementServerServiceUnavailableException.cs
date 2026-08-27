using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

[Serializable]
public class ManagementServerServiceUnavailableException : AdministrationSubsystemException
{
	protected override int ErrorCodeDefault => 60142;

	public ManagementServerServiceUnavailableException()
		: base(VmoResources.VMO60142)
	{
	}

	public ManagementServerServiceUnavailableException(string message)
		: base(message)
	{
	}

	public ManagementServerServiceUnavailableException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected ManagementServerServiceUnavailableException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
