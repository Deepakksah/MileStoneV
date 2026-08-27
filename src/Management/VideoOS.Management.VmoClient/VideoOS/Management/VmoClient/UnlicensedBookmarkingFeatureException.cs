using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class UnlicensedBookmarkingFeatureException : VmoException
{
	public UnlicensedBookmarkingFeatureException()
	{
	}

	protected UnlicensedBookmarkingFeatureException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public UnlicensedBookmarkingFeatureException(string message)
		: base(message)
	{
	}

	public UnlicensedBookmarkingFeatureException(string message, Exception ex)
		: base(message, ex)
	{
	}
}
