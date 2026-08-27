namespace VideoOS.EventServer.Events.EventStatusSubscriptionWsApi.Client.Protocol;

public class RemoveSubscriptionCommand : CommandBase
{
	public string SubscriptionId { get; set; }

	public RemoveSubscriptionCommand()
		: base("removeSubscription")
	{
	}
}
