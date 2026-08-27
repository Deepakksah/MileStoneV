using System;
using System.Runtime.Serialization;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

[Serializable]
public class AutomaticActivationUserCredentialsMissingException : VmoServiceException
{
	protected override int ErrorCodeDefault => 65009;

	public AutomaticActivationUserCredentialsMissingException()
		: base(VmoResources.VMO65009)
	{
	}

	protected AutomaticActivationUserCredentialsMissingException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public AutomaticActivationUserCredentialsMissingException(string message)
		: base(message)
	{
	}

	public AutomaticActivationUserCredentialsMissingException(string message, Exception ex)
		: base(message, ex)
	{
	}
}
