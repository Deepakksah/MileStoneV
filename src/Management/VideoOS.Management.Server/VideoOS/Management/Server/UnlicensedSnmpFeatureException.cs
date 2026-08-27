using System;
using System.Runtime.Serialization;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

[Serializable]
public class UnlicensedSnmpFeatureException : VmoServiceException
{
	protected override int ErrorCodeDefault => 64025;

	public UnlicensedSnmpFeatureException()
		: base(VmoResources.VMO64025)
	{
	}

	protected UnlicensedSnmpFeatureException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public UnlicensedSnmpFeatureException(string message)
		: base(message)
	{
	}

	public UnlicensedSnmpFeatureException(string message, Exception ex)
		: base(message, ex)
	{
	}
}
