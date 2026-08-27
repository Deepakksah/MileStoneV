using System;

namespace VideoOS.EventServer.Events.EventStatusSubscriptionWsApi.Client.Protocol;

public class SubscriptionUpdateResponse : ResponseBase
{
	public Guid SubscriptionId { get; }

	public SubscriptionUpdateResponse(ulong commandId, int status, Guid subscriptionId)
		: base(commandId, status)
	{
		SubscriptionId = subscriptionId;
	}
}
