namespace VideoOS.EventServer.Communication.Configuration;

public class GetDefaultEventRetentionRequestObject : RequestObject
{
	public GetDefaultEventRetentionParams GetDefaultEventRetentionParams { get; private set; }

	public GetDefaultEventRetentionRequestObject(int requestId, GetDefaultEventRetentionParams getDefaultEventRetentionParams)
		: base(requestId)
	{
		GetDefaultEventRetentionParams = getDefaultEventRetentionParams;
	}
}
