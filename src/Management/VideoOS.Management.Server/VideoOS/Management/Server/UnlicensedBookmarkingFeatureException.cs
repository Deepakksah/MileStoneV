using System;
using System.Runtime.Serialization;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

[Serializable]
public class UnlicensedBookmarkingFeatureException : VmoServiceException
{
	protected override int ErrorCodeDefault => 64027;

	public UnlicensedBookmarkingFeatureException()
		: base(VmoResources.VMO64022)
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
