using System;
using System.Runtime.Serialization;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

[Serializable]
public class UnlicensedEdgeStorageFeatureException : VmoServiceException
{
	protected override int ErrorCodeDefault => 64030;

	public UnlicensedEdgeStorageFeatureException()
		: base(VmoResources.VMO64030)
	{
	}

	protected UnlicensedEdgeStorageFeatureException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public UnlicensedEdgeStorageFeatureException(string message)
		: base(message)
	{
	}

	public UnlicensedEdgeStorageFeatureException(string message, Exception ex)
		: base(message, ex)
	{
	}
}
