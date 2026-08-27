using System;

namespace VideoOS.EventServer.Communication.AlarmClient;

public class GetSessionEventLinesRequestObject : RequestObject
{
	public int MaxCount { get; }

	public Guid SessionId { get; }

	public GetSessionEventLinesRequestObject(int requestId, int maxCount, Guid sessionId)
		: base(requestId)
	{
		MaxCount = maxCount;
		SessionId = sessionId;
	}
}
