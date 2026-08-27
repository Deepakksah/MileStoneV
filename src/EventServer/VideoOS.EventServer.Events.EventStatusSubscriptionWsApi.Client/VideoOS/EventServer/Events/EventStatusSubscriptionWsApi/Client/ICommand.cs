namespace VideoOS.EventServer.Events.EventStatusSubscriptionWsApi.Client;

public interface ICommand
{
	ulong CommandId { get; set; }

	string ToJson();
}
