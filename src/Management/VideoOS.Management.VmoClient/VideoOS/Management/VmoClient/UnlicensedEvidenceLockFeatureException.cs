using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class UnlicensedEvidenceLockFeatureException : VmoException
{
	public UnlicensedEvidenceLockFeatureException()
	{
	}

	protected UnlicensedEvidenceLockFeatureException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public UnlicensedEvidenceLockFeatureException(string message)
		: base(message)
	{
	}

	public UnlicensedEvidenceLockFeatureException(string message, Exception ex)
		: base(message, ex)
	{
	}
}
