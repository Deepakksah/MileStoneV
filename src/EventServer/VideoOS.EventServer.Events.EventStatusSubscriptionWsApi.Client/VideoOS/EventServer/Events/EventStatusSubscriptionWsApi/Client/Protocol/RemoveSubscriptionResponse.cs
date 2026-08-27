namespace VideoOS.EventServer.Events.EventStatusSubscriptionWsApi.Client.Protocol;

public class RemoveSubscriptionResponse : ResponseBase
{
	public RemoveSubscriptionResponse(ulong commandId, int status)
		: base(commandId, status)
	{
	}
}
