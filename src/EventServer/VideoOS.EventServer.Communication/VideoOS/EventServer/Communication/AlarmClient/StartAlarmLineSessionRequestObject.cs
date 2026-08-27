using VideoOS.Platform.Proxy.Alarm;

namespace VideoOS.EventServer.Communication.AlarmClient;

public class StartAlarmLineSessionRequestObject : RequestObject
{
	public AlarmFilter Filter { get; }

	public StartAlarmLineSessionRequestObject(int requestId, AlarmFilter filter)
		: base(requestId)
	{
		Filter = filter;
	}
}
