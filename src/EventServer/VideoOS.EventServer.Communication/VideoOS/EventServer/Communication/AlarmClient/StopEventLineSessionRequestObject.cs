using System;

namespace VideoOS.EventServer.Communication.AlarmClient;

public class StopEventLineSessionRequestObject : RequestObject
{
	public Guid SessionId { get; }

	public StopEventLineSessionRequestObject(int requestId, Guid sessionId)
		: base(requestId)
	{
		SessionId = sessionId;
	}
}
