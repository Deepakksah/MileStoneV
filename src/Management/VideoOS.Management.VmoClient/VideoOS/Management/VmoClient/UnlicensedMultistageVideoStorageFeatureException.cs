using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class UnlicensedMultistageVideoStorageFeatureException : VmoException
{
	public UnlicensedMultistageVideoStorageFeatureException()
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
