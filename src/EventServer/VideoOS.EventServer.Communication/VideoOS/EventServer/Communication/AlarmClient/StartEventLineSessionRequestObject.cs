using VideoOS.Platform.Proxy.Alarm;

namespace VideoOS.EventServer.Communication.AlarmClient;

public class StartEventLineSessionRequestObject : RequestObject
{
	public EventFilter Filter { get; }

	public StartEventLineSessionRequestObject(int requestId, EventFilter filter)
		: base(requestId)
	{
		Filter = filter;
	}
}
