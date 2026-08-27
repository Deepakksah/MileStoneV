using System;

namespace VideoOS.EventServer.Communication.AlarmClient;

public class StopAlarmLineSessionRequestObject : RequestObject
{
	public Guid SessionId { get; }

	public StopAlarmLineSessionRequestObject(int requestId, Guid sessionId)
		: base(requestId)
	{
		SessionId = sessionId;
	}
}
