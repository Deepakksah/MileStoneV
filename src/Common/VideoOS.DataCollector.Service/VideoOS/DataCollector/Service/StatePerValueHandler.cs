using System;
using System.Collections.Concurrent;
using VideoOS.DataCollector.Service.Rule.Entities.Sample;
using VideoOS.DataCollector.Service.Rule.Entities.State;

namespace VideoOS.DataCollector.Service;

public class StatePerValueHandler : IStateHandler
{
	private readonly IRule _rule;

	private readonly ConcurrentQueue<CounterValue> _buffer = new ConcurrentQueue<CounterValue>();

	private StateChange _currentState;

	private bool _firstStateIsSend;

	public StateChange CurrentState => _currentState;

	public StatePerValueHandler(IRule rule)
	{
		_rule = rule;
		_currentState = new StateChange(State.Undefined, -2.1474836E+09f);
	}

	public (bool, StateChange?) AddCounterValue(PerformanceCounterSample counterValue)
	{
		_buffer.Enqueue(new CounterValue(counterValue.CollectionTime, counterValue.CalculatedValue));
		State state = _rule.GetState(_buffer, State.Undefined);
		return HasStateChanged(_currentState.TriggerValue, counterValue.CalculatedValue, _currentState.CounterState, state);
	}

	public void Reset()
	{
		_currentState = new StateChange(State.Undefined, -2.1474836E+09f);
	}

	public (bool, StateChange?) SetUndefined()
	{
		return HasStateChanged(CurrentState.TriggerValue, -2.1474836E+09f, CurrentState.CounterState, State.Undefined);
	}

	public (bool, StateChange?) AddStateChange(StateChange stateChange)
	{
		return HasStateChanged(CurrentState.TriggerValue, stateChange.TriggerValue, CurrentState.CounterState, stateChange.CounterState);
	}

	private (bool, StateChange?) HasStateChanged(float oldTriggerValue, float newTriggerValue, State oldState, State newState)
	{
		if ((double)Math.Abs(newTriggerValue - oldTriggerValue) > 0.0001 || newState != oldState || !_firstStateIsSend)
		{
			_firstStateIsSend = true;
			_currentState = new StateChange(newState, newTriggerValue);
			return (true, _currentState);
		}
		return (false, null);
	}
}
