using VideoOS.DataCollector.Service.Common;
using VideoOS.DataCollector.Service.Rule.Entities.State;

namespace VideoOS.DataCollector.Service.Rule.Events;

public class StateChangeEvent : BaseEvent
{
	public StateChangeData StateChange { get; }

	public StateChangeEvent(StateChangeData stateChange)
	{
		StateChange = stateChange;
	}
}
