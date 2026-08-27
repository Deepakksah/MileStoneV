using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using VideoOS.DataCollector.Service.Configuration;
using VideoOS.DataCollector.Service.Entities.DataCollection;
using VideoOS.DataCollector.Service.Rule.Entities.Sample;
using VideoOS.DataCollector.Service.Rule.Entities.State;
using VideoOS.DataCollector.Service.Rule.Events;

namespace VideoOS.DataCollector.Service;

public abstract class ThresholdRule : Entity, IRule, IRuleBase, IEntity
{
	private enum Threshold
	{
		Critical,
		Warning
	}

	private float _warningThreshold;

	private float _warningThresholdBuffer;

	private float _criticalThreshold;

	private float _criticalThresholdBuffer;

	private bool _highIsCritical;

	private int _stateLatency;

	protected TimeSpan _timeWindow;

	protected DateTime _nextSampleTime;

	protected IStateHandler _stateHandler;

	protected bool _isActive;

	protected int _dataCollectorId;

	protected RuleOptions _options;

	public float WarningThreshold => _warningThreshold;

	public float CriticalThreshold => _criticalThreshold;

	public int StateLatency
	{
		get
		{
			return _stateLatency;
		}
		set
		{
			if (value != _stateLatency)
			{
				_stateLatency = value;
				_warningThresholdBuffer = CalculateBuffer(Threshold.Warning);
				_criticalThresholdBuffer = CalculateBuffer(Threshold.Critical);
				_stateHandler.Reset();
				_nextSampleTime = DateTime.UtcNow;
			}
		}
	}

	public string Name { get; set; }

	public bool IsDefault { get; set; }

	public TimeSpan TimeSpanNeeded => _timeWindow;

	public IStateHandler StateHandler => _stateHandler;

	public string RuleId { get; set; }

	public string CounterPath { get; set; }

	public bool IsActive => _isActive;

	protected ThresholdRule(int datacollectorId, CounterStateRuleConfiguration ruleConfiguration, bool isDefault, bool triggerPerValue, bool counterDirection, IStateHandlerFactory stateFactory, IOptions<RuleOptions> options)
	{
		_options = options.Value;
		_dataCollectorId = datacollectorId;
		RuleId = ruleConfiguration.CounterRuleId;
		CounterPath = ruleConfiguration.CounterPath;
		IsDefault = isDefault;
		_highIsCritical = counterDirection;
		_stateHandler = stateFactory.CreateStateHandler(triggerPerValue, this);
		Name = ruleConfiguration.Name;
		_stateLatency = _options.StateChangeLatency;
		_warningThreshold = ruleConfiguration.WarningThreshold;
		_criticalThreshold = ruleConfiguration.CriticalThreshold;
		_warningThresholdBuffer = CalculateBuffer(Threshold.Warning);
		_criticalThresholdBuffer = CalculateBuffer(Threshold.Critical);
		_timeWindow = TimeSpan.FromSeconds(ruleConfiguration.TimeWindow);
		_nextSampleTime = DateTime.UtcNow;
		_isActive = true;
	}

	public State GetState(ConcurrentQueue<CounterValue> values, State currentState)
	{
		if (!_isActive)
		{
			return currentState;
		}
		try
		{
			RemoveObsoleteValues(values);
			if (values.Count == 0)
			{
				return State.Undefined;
			}
			float value = values.Average((CounterValue p) => p.Value);
			if (IsCritical(value, currentState))
			{
				return State.Critical;
			}
			if (IsWarning(value, currentState))
			{
				return State.Warning;
			}
			return State.Normal;
		}
		catch (InvalidOperationException)
		{
			return currentState;
		}
	}

	public bool Refresh(CounterStateRuleConfiguration ruleConfiguration)
	{
		if (_warningThreshold != ruleConfiguration.WarningThreshold || _criticalThreshold != ruleConfiguration.CriticalThreshold || _timeWindow != TimeSpan.FromSeconds(ruleConfiguration.TimeWindow))
		{
			_warningThreshold = ruleConfiguration.WarningThreshold;
			_criticalThreshold = ruleConfiguration.CriticalThreshold;
			_warningThresholdBuffer = CalculateBuffer(Threshold.Warning);
			_criticalThresholdBuffer = CalculateBuffer(Threshold.Critical);
			_timeWindow = TimeSpan.FromSeconds(ruleConfiguration.TimeWindow);
			_stateHandler.Reset();
			_nextSampleTime = DateTime.UtcNow;
			return true;
		}
		return false;
	}

	public async Task SampleIfDue(ISamplerFactory samplerFactory, CancellationToken cancellationToken)
	{
		if (DateTime.UtcNow >= _nextSampleTime)
		{
			(bool, StateChange) tuple = await DoSample(samplerFactory, cancellationToken);
			if (tuple.Item1 && tuple.Item2 != null)
			{
				StateChangeData stateChange = new StateChangeData(_dataCollectorId, RuleId, CounterPath, (int)tuple.Item2.CounterState, tuple.Item2.TriggerValue);
				AddDomainEvent(new StateChangeEvent(stateChange));
			}
		}
	}

	public virtual void OnRecordingServerDown()
	{
	}

	public virtual void OnRecordingServerUp()
	{
	}

	protected abstract Task<(bool, StateChange?)> DoSample(ISamplerFactory samplerFactory, CancellationToken cancellationToken);

	private bool IsCritical(float value, State currentState)
	{
		if (_criticalThreshold > -1f)
		{
			if (_highIsCritical)
			{
				if (!(value >= _criticalThreshold))
				{
					if (currentState == State.Critical)
					{
						return value > _criticalThreshold - _criticalThresholdBuffer;
					}
					return false;
				}
				return true;
			}
			if (!(value <= _criticalThreshold))
			{
				if (currentState == State.Critical)
				{
					return value < _criticalThreshold + _criticalThresholdBuffer;
				}
				return false;
			}
			return true;
		}
		return false;
	}

	private bool IsWarning(float value, State currentState)
	{
		if (_warningThreshold > -1f)
		{
			if (_highIsCritical)
			{
				if (!(value >= _warningThreshold))
				{
					if (currentState == State.Warning)
					{
						return value > _warningThreshold - _warningThresholdBuffer;
					}
					return false;
				}
				return true;
			}
			if (!(value <= _warningThreshold))
			{
				if (currentState == State.Warning)
				{
					return value < _warningThreshold + _warningThresholdBuffer;
				}
				return false;
			}
			return true;
		}
		return false;
	}

	private void RemoveObsoleteValues(ConcurrentQueue<CounterValue> values)
	{
		while (values.Count > 0 && values.First().Time < DateTime.UtcNow - _timeWindow)
		{
			values.TryDequeue(out CounterValue _);
		}
	}

	private float CalculateBuffer(Threshold threshold)
	{
		if (StateLatency == 0)
		{
			return 0f;
		}
		float num = ((_criticalThreshold > -1f) ? _criticalThreshold : 0f);
		float num2 = ((_warningThreshold > -1f) ? _warningThreshold : 0f);
		if ((double)Math.Abs(num - num2) < 0.0001)
		{
			return num / (float)StateLatency;
		}
		return Math.Min(Math.Abs(num - num2), (threshold == Threshold.Warning) ? num2 : num) / (float)StateLatency;
	}
}
