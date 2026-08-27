using System;

namespace VideoOS.EventServer.Communication.AlarmClient;

public class GetSessionAlarmLinesRequestObject : RequestObject
{
	public int MaxCount { get; }

	public Guid SessionId { get; }

	public GetSessionAlarmLinesRequestObject(int requestId, int maxCount, Guid sessionId)
		: base(requestId)
	{
		MaxCount = maxCount;
		SessionId = sessionId;
	}
}
