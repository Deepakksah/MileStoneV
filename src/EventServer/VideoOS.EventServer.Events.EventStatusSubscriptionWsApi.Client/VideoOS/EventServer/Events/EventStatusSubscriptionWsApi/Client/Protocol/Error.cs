namespace VideoOS.EventServer.Events.EventStatusSubscriptionWsApi.Client.Protocol;

public class Error
{
	public string ErrorText { get; }

	public Error(string errortext)
	{
		ErrorText = errortext;
	}
}
