using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.Common;

[Serializable]
public class GenericIdpException : VmoServiceException
{
	protected override int ErrorCodeDefault => -1;

	public GenericIdpException()
	{
	}

	public GenericIdpException(string message)
		: base(message)
	{
	}

	public GenericIdpException(int errorCode)
		: base(errorCode)
	{
	}

	public GenericIdpException(string message, int errorCode)
		: base(message, errorCode)
	{
	}

	public GenericIdpException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected GenericIdpException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public GenericIdpException(string message, int errorCode, Exception innerException)
		: base(message, errorCode, innerException)
	{
	}
}
