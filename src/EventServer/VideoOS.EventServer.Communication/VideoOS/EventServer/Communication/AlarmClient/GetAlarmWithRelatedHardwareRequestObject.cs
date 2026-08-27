using VideoOS.Platform.Proxy.Alarm;

namespace VideoOS.EventServer.Communication.AlarmClient;

public class GetAlarmWithRelatedHardwareRequestObject : RequestObject
{
	public int MaxCount { get; }

	public AlarmSearchCriteria[] Filter { get; }

	public GetAlarmWithRelatedHardwareRequestObject(int requestId, int maxCount, AlarmSearchCriteria[] filter)
		: base(requestId)
	{
		MaxCount = maxCount;
		Filter = filter;
	}
}
