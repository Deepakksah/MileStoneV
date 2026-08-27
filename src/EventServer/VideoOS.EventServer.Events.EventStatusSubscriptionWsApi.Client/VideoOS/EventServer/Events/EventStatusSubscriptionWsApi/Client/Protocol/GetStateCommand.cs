namespace VideoOS.EventServer.Events.EventStatusSubscriptionWsApi.Client.Protocol;

public class GetStateCommand : CommandBase
{
	public GetStateCommand()
		: base("getState")
	{
	}
}
