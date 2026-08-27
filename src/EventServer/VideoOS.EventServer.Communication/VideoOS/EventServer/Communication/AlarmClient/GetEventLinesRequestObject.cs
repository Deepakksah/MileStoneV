using VideoOS.Platform.Proxy.Alarm;

namespace VideoOS.EventServer.Communication.AlarmClient;

public class GetEventLinesRequestObject : RequestObject
{
	public int From { get; }

	public int MaxCount { get; }

	public EventFilter Filter { get; }

	public GetEventLinesRequestObject(int requestId, int from, int maxCount, EventFilter filter)
		: base(requestId)
	{
		From = from;
		MaxCount = maxCount;
		Filter = filter;
	}
}
