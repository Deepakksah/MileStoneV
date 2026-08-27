using System;
using System.Collections.Concurrent;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using VideoOS.DataCollector.Service.Entities.DataCollection;
using VideoOS.DataCollector.Service.Rule.Entities.Sample;
using VideoOS.DataCollector.Service.Rule.Entities.State;
using VideoOS.DataCollector.Service.Rule.Events;

namespace VideoOS.DataCollector.Service;

public class ServiceAvailableRule : Entity, IRule, IRuleBase, IEntity
{
	private IStateHandler _stateHandler;

	private DateTime _nextSampleTime;

	public const string CategoryName = "Windows Services";

	public const string ServerAvailableRuleId = "063EC923-9E6B-4A95-B949-AE5A76DB8B5C";

	private string _serviceName;

	private string _instanceName;

	private readonly TimeSpan _timeSpan = new TimeSpan(0, 0, 0, 2);

	private readonly int _dataCollectorId;

	public string Name { get; set; }

	public bool IsDefault { get; } = true;

	public string CounterPath { get; set; }

	public string RuleId { get; set; }

	public TimeSpan TimeSpanNeeded => _timeSpan;

	public bool IsActive => true;

	public ServiceAvailableRule(int dataCollectorId, string name, string serviceName, string serviceType, string id, IStateHandlerFactory stateHandlerFactory)
	{
		Name = name;
		_serviceName = serviceName;
		CounterPath = string.Format(CultureInfo.InvariantCulture, "{0}({1})", serviceName, serviceType);
		_instanceName = serviceType;
		RuleId = id;
		_stateHandler = stateHandlerFactory.CreateStateHandler(triggerPerValue: false, this);
		_nextSampleTime = DateTime.UtcNow;
		_dataCollectorId = dataCollectorId;
	}

	public bool Refresh(CounterStateRuleConfiguration ruleConfiguration)
	{
		if (CounterPath != ruleConfiguration.CounterPath)
		{
			CounterPath = ruleConfiguration.CounterPath;
			_stateHandler.Reset();
			return true;
		}
		return false;
	}

	public State GetState(ConcurrentQueue<CounterValue> values, State currentState)
	{
		if (values.IsEmpty)
		{
			return State.Undefined;
		}
		float? num = DequeueAllAndReturnLast(values)?.Value;
		if (num.HasValue)
		{
			float valueOrDefault = num.GetValueOrDefault();
			if (valueOrDefault == 1f)
			{
				return State.Normal;
			}
			if (valueOrDefault == 3f)
			{
				return State.Critical;
			}
		}
		return State.Undefined;
	}

	private CounterValue? DequeueAllAndReturnLast(ConcurrentQueue<CounterValue> values)
	{
		CounterValue result = null;
		while (!values.IsEmpty)
		{
			values.TryDequeue(out result);
		}
		return result;
	}

	public virtual Task SampleIfDue(ISamplerFactory samplerFactory, CancellationToken cancellationToken)
	{
		if (_nextSampleTime <= DateTime.UtcNow)
		{
			int num = (samplerFactory.GetServiceSampler().Sample(_serviceName) ? 1 : 3);
			(bool, StateChange) tuple = _stateHandler.AddCounterValue(new PerformanceCounterSample("Windows Services", CounterPath, _instanceName, DateTime.UtcNow, num, num));
			_nextSampleTime += _timeSpan;
			if (tuple.Item1 && tuple.Item2 != null)
			{
				StateChangeData stateChange = new StateChangeData(_dataCollectorId, RuleId, CounterPath, (int)tuple.Item2.CounterState, tuple.Item2.TriggerValue);
				AddDomainEvent(new StateChangeEvent(stateChange));
			}
		}
		return Task.CompletedTask;
	}

	public void OnRecordingServerDown()
	{
	}

	public void OnRecordingServerUp()
	{
	}
}
