using System;
using System.Runtime.Serialization;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

[Serializable]
public class UnlicensedMultistageVideoStorageFeatureException : VmoServiceException
{
	protected override int ErrorCodeDefault => 64017;

	public UnlicensedMultistageVideoStorageFeatureException()
		: base(VmoResources.VMO64017)
	{
	}

	protected UnlicensedMultistageVideoStorageFeatureException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public UnlicensedMultistageVideoStorageFeatureException(string message)
		: base(message)
	{
	}

	public UnlicensedMultistageVideoStorageFeatureException(string message, Exception ex)
		: base(message, ex)
	{
	}
}
