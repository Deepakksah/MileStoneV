namespace VideoOS.EventServer.Communication.Configuration;

public class SetEventRetentionRequestObject : RequestObject
{
	public SetEventRetentionParams SetEventRetentionParams { get; private set; }

	public SetEventRetentionRequestObject(int requestId, SetEventRetentionParams setEventRetentionParams)
		: base(requestId)
	{
		SetEventRetentionParams = setEventRetentionParams;
	}
}
