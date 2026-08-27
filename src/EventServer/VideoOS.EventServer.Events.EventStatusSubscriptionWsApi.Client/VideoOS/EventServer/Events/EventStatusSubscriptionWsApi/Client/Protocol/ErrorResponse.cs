namespace VideoOS.EventServer.Events.EventStatusSubscriptionWsApi.Client.Protocol;

public class ErrorResponse : ResponseBase
{
	public Error Error { get; }

	public ErrorResponse(ulong commandId, int status, Error error)
		: base(commandId, status)
	{
		Error = error;
	}
}
