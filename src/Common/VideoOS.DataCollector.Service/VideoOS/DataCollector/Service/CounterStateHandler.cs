using System;
using System.Collections.Concurrent;
using VideoOS.DataCollector.Service.Rule.Entities.Sample;
using VideoOS.DataCollector.Service.Rule.Entities.State;

namespace VideoOS.DataCollector.Service;

public class CounterStateHandler : IStateHandler
{
	private readonly IRule _rule;

	private readonly ConcurrentQueue<CounterValue> _values;

	private bool _firstStateIsSend;

	public State CurrentState{ get; set; }

	public CounterStateHandler(IRule rule)
	{
		_rule = rule;
		CurrentState = State.Undefined;
		_values = new ConcurrentQueue<CounterValue>();
	}

	public (bool, StateChange?) AddCounterValue(PerformanceCounterSample counterValue)
	{
		if (counterValue == null)
		{
			throw new ArgumentNullException("counterValue");
		}
		_values.Enqueue(new CounterValue(counterValue.CollectionTime, counterValue.CalculatedValue));
		State state = _rule.GetState(_values, CurrentState);
		return HasStateChanged(CurrentState, state, counterValue.CalculatedValue);
	}

	public void Reset()
	{
		CurrentState = State.Undefined;
	}

	public (bool, StateChange?) AddStateChange(StateChange stateChange)
	{
		if (stateChange == null)
		{
			throw new ArgumentNullException("stateChange");
		}
		return HasStateChanged(CurrentState, stateChange.CounterState, stateChange.TriggerValue);
	}

	public (bool, StateChange?) SetUndefined()
	{
		return HasStateChanged(CurrentState, State.Undefined, -2.1474836E+09f);
	}

	private (bool, StateChange?) HasStateChanged(State oldState, State newState, float newTriggerValue)
	{
		if (newState != oldState || !_firstStateIsSend)
		{
			_firstStateIsSend = true;
			CurrentState = newState;
			return (true, new StateChange(newState, newTriggerValue));
		}
		return (false, new StateChange(oldState, newTriggerValue));
	}
}
