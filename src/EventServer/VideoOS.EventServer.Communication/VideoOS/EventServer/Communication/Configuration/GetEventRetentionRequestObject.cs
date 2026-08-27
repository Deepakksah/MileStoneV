namespace VideoOS.EventServer.Communication.Configuration;

public class GetEventRetentionRequestObject : RequestObject
{
	public GetEventRetentionParams GetEventRetentionParams { get; private set; }

	public GetEventRetentionRequestObject(int requestId, GetEventRetentionParams getEventRetentionParams)
		: base(requestId)
	{
		GetEventRetentionParams = getEventRetentionParams;
	}
}
