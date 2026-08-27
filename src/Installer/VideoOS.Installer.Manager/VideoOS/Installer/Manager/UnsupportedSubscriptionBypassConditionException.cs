using System;
using System.Runtime.Serialization;

namespace VideoOS.Installer.Manager;

[Serializable]
public class UnsupportedSubscriptionBypassConditionException : Exception
{
	public UnsupportedSubscriptionBypassConditionException()
	{
	}

	public UnsupportedSubscriptionBypassConditionException(string message)
		: base(message)
	{
	}

	public UnsupportedSubscriptionBypassConditionException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected UnsupportedSubscriptionBypassConditionException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
