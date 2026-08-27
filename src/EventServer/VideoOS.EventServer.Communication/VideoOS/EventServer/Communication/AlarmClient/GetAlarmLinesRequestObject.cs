using VideoOS.Platform.Proxy.Alarm;

namespace VideoOS.EventServer.Communication.AlarmClient;

public class GetAlarmLinesRequestObject : RequestObject
{
	public int From { get; }

	public int MaxCount { get; }

	public AlarmFilter Filter { get; }

	public GetAlarmLinesRequestObject(int requestId, int from, int maxCount, AlarmFilter filter)
		: base(requestId)
	{
		From = from;
		MaxCount = maxCount;
		Filter = filter;
	}
}
