using VideoOS.DataCollector.Service.Rule.Entities.State;

namespace VideoOS.DataCollector.Service;

public class StateChange
{
	private State _counterState;

	private float _triggerValue;

	public State CounterState => _counterState;

	public float TriggerValue => _triggerValue;

	public StateChange(State counterState, float triggerValue)
	{
		_counterState = counterState;
		_triggerValue = triggerValue;
	}
}
