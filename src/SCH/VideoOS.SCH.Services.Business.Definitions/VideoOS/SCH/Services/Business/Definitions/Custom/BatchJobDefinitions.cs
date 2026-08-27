namespace VideoOS.SCH.Services.Business.Definitions.Custom;

public class BatchJobDefinitions
{
	public int DeleteExpiredSessionsJobIntervalSecs;

	public int DeleteNonSessionMessagesIntervalSecs;

	public int ServiceRegistrationIntervalSecs;

	public int SessionMsgIndicatorUpdateIntervalMs;

	public bool SessionMsgIndicatorUsed;

	public BatchJobDefinitions()
	{
	}

	public BatchJobDefinitions(int deleteExpiredSessionsJobIntervalSecs, int deleteNonSessionMessagesIntervalSecs, int serviceRegistrationIntervalSecs, int sessionMsgIndicatorUpdateIntervalMs, bool sessionMsgIndicatorUsed)
	{
		DeleteExpiredSessionsJobIntervalSecs = deleteExpiredSessionsJobIntervalSecs;
		DeleteNonSessionMessagesIntervalSecs = deleteNonSessionMessagesIntervalSecs;
		ServiceRegistrationIntervalSecs = serviceRegistrationIntervalSecs;
		SessionMsgIndicatorUpdateIntervalMs = sessionMsgIndicatorUpdateIntervalMs;
		SessionMsgIndicatorUsed = sessionMsgIndicatorUsed;
	}
}
