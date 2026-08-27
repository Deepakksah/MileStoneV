using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class UnlicensedSnmpFeatureException : VmoException
{
	public UnlicensedSnmpFeatureException()
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
