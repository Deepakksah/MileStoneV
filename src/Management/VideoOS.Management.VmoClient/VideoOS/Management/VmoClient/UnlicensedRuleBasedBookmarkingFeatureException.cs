using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class UnlicensedRuleBasedBookmarkingFeatureException : VmoException
{
	public UnlicensedRuleBasedBookmarkingFeatureException()
	{
	}

	protected UnlicensedRuleBasedBookmarkingFeatureException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public UnlicensedRuleBasedBookmarkingFeatureException(string message)
		: base(message)
	{
	}

	public UnlicensedRuleBasedBookmarkingFeatureException(string message, Exception ex)
		: base(message, ex)
	{
	}
}
