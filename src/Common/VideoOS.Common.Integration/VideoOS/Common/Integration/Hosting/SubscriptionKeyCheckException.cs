using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.Hosting;

[Serializable]
public class SubscriptionKeyCheckException : SecureTunnelServerException
{
	public string OriginalResponse { get; private set; }

	public SubscriptionKeyCheckException()
	{
	}

	public SubscriptionKeyCheckException(string message)
		: base(message)
	{
	}

	public SubscriptionKeyCheckException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected SubscriptionKeyCheckException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
		OriginalResponse = info.GetString("OriginalResponse");
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		if (info == null)
		{
			throw new ArgumentNullException("info");
		}
		info.AddValue("OriginalResponse", OriginalResponse);
		base.GetObjectData(info, context);
	}

	public SubscriptionKeyCheckException(string message, string originalResponse)
		: base(message)
	{
		OriginalResponse = originalResponse;
	}
}
