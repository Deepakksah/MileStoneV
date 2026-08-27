namespace VideoOS.EventServer.Events.EventStatusSubscriptionWsApi.Client.Protocol;

public class StartSessionCommand : CommandBase
{
	public string SessionId { get; set; }

	public string EventId { get; set; }

	public StartSessionCommand()
		: base("startSession")
	{
	}
}
