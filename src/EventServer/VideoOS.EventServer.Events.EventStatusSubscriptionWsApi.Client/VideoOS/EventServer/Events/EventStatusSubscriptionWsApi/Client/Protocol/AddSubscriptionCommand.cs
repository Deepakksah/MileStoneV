using System.Collections.Generic;

namespace VideoOS.EventServer.Events.EventStatusSubscriptionWsApi.Client.Protocol;

public class AddSubscriptionCommand : CommandBase
{
	public List<SubscriptionFilter> Filters { get; set; }

	public AddSubscriptionCommand()
		: base("addSubscription")
	{
	}
}
