using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VideoOS.DataCollector.Service.Rule.Entities.State;

namespace VideoOS.DataCollector.Service.Rule.Entities.Rules;

public class MultiCounterRule : IMultiCounterRule, IRuleBase
{
	public class CounterStateHelper
	{
		public bool IsReported { get; set; }

		public string CounterPath { get; private set; }

		public VideoOS.DataCollector.Service.Rule.Entities.State.State State { get; set; }

		public float TriggerValue { get; set; }

		public CounterStateHelper(string counterPath, VideoOS.DataCollector.Service.Rule.Entities.State.State state, bool isReported)
		{
			IsReported = isReported;
			CounterPath = counterPath;
			State = state;
			TriggerValue = 0f;
		}
	}

	private readonly ConcurrentDictionary<string, CounterStateHelper> _registeredCounters;

	private long _lastReportedState;

	private long _allRegistered;

	private VideoOS.DataCollector.Service.Rule.Entities.State.State? LastReportedState
	{
		get
		{
			return (VideoOS.DataCollector.Service.Rule.Entities.State.State)Interlocked.Read(in _lastReportedState);
		}
		set
		{
			Interlocked.Exchange(ref _lastReportedState, Convert.ToInt64(value));
		}
	}

	private bool AllRegistered
	{
		get
		{
			return Interlocked.Read(in _allRegistered) == 1;
		}
		set
		{
			Interlocked.Exchange(ref _allRegistered, Convert.ToInt64(value));
		}
	}

	public IReadOnlyDictionary<string, CounterStateHelper> RegisteredCounters => _registeredCounters.ToDictionary<KeyValuePair<string, CounterStateHelper>, string, CounterStateHelper>((KeyValuePair<string, CounterStateHelper> c) => c.Key, (KeyValuePair<string, CounterStateHelper> c) => c.Value);

	public string CounterPath { get; set; }

	public string RuleId { get; set; }

	public MultiCounterRule(string counterPath, string ruleId)
		: this(counterPath, ruleId, new ConcurrentDictionary<string, CounterStateHelper>(StringComparer.OrdinalIgnoreCase))
	{
	}

	public MultiCounterRule(string counterPath, string ruleId, ConcurrentDictionary<string, CounterStateHelper> registeredCounters)
	{
		CounterPath = counterPath ?? throw new ArgumentNullException("counterPath");
		RuleId = ruleId ?? throw new ArgumentNullException("ruleId");
		_registeredCounters = registeredCounters;
	}

	public Task<(bool, StateChange?)> SampleStateChange(StateChangeData state)
	{
		if (state == null)
		{
			throw new ArgumentNullException("state");
		}
		if (!_registeredCounters.TryGetValue(state.CounterPath, out CounterStateHelper value))
		{
			return Task.FromResult<(bool, StateChange)>((false, null));
		}
		value.State = (VideoOS.DataCollector.Service.Rule.Entities.State.State)state.State;
		value.TriggerValue = (float)state.TriggerValue;
		value.IsReported = true;
		if (!AllRegistered && _registeredCounters.All<KeyValuePair<string, CounterStateHelper>>((KeyValuePair<string, CounterStateHelper> c) => c.Value.IsReported))
		{
			AllRegistered = true;
		}
		return HasStateChange();
	}

	private Task<(bool, StateChange?)> HasStateChange()
	{
		if (AllRegistered)
		{
			StateChange maxState = GetMaxState();
			if (maxState != null && maxState.CounterState != LastReportedState)
			{
				LastReportedState = maxState.CounterState;
				return Task.FromResult((true, new StateChange(maxState.CounterState, maxState.TriggerValue)));
			}
		}
		return Task.FromResult<(bool, StateChange)>((false, null));
	}

	private StateChange? GetMaxState()
	{
		if (!_registeredCounters.Any())
		{
			return null;
		}
		CounterStateHelper counterStateHelper = null;
		foreach (KeyValuePair<string, CounterStateHelper> registeredCounter in _registeredCounters)
		{
			if (counterStateHelper == null || registeredCounter.Value.State > counterStateHelper.State)
			{
				counterStateHelper = registeredCounter.Value;
			}
		}
		return new StateChange(counterStateHelper.State, counterStateHelper.TriggerValue);
	}

	public void RegisterPerformanceCounterInstance(string performanceCounterInstancePath)
	{
		if (performanceCounterInstancePath == null)
		{
			throw new ArgumentNullException("performanceCounterInstancePath");
		}
		_registeredCounters[performanceCounterInstancePath] = new CounterStateHelper(performanceCounterInstancePath, VideoOS.DataCollector.Service.Rule.Entities.State.State.Undefined, isReported: false);
		AllRegistered = false;
	}

	public Task<(bool, StateChange?)> UnRegisterPerformanceCounterInstance(string performanceCounterInstancePath)
	{
		if (performanceCounterInstancePath == null)
		{
			throw new ArgumentNullException("performanceCounterInstancePath");
		}
		if (_registeredCounters.TryRemove(performanceCounterInstancePath, out CounterStateHelper _))
		{
			return HasStateChange();
		}
		return Task.FromResult<(bool, StateChange)>((false, null));
	}

	public void ResetState()
	{
		AllRegistered = false;
		LastReportedState = null;
		_registeredCounters.Clear();
	}
}
