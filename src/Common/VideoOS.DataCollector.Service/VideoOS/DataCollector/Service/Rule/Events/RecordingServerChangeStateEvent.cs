using VideoOS.DataCollector.Service.Common;

namespace VideoOS.DataCollector.Service.Rule.Events;

public class RecordingServerChangeStateEvent : BaseEvent
{
	public bool RecordingServerRunning { get; }

	public RecordingServerChangeStateEvent(bool up)
	{
		RecordingServerRunning = up;
	}
}
