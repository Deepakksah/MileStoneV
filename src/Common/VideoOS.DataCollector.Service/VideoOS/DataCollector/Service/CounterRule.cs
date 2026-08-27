using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using VideoOS.DataCollector.Service.Configuration;
using VideoOS.DataCollector.Service.Entities.DataCollection;
using VideoOS.DataCollector.Service.Rule.Entities.Sample;

namespace VideoOS.DataCollector.Service;

public class CounterRule : ThresholdRule
{
	private int TimeBetweenSamples => Math.Max((int)_timeWindow.TotalMilliseconds / _options.CounterStateSampleCount, _options.CounterStateMinSampleIntervalMS);

	public CounterRule(int datacollectoId, CounterStateRuleConfiguration ruleConfiguration, bool isDefault, bool triggerPerValue, bool counterDirection, IStateHandlerFactory stateFactory, IOptions<RuleOptions> options)
		: base(datacollectoId, ruleConfiguration, isDefault, triggerPerValue, counterDirection, stateFactory, options)
	{
		_options = options.Value;
	}

	protected override async Task<(bool, StateChange?)> DoSample(ISamplerFactory samplerFactory, CancellationToken cancellationToken)
	{
		if (!_isActive)
		{
			return (false, null);
		}
		PerformanceCounterSample performanceCounterSample = await samplerFactory.GetPerformanceCounterValueSampler().Sample(base.CounterPath);
		(bool, StateChange) result = ((!(performanceCounterSample == null)) ? _stateHandler.AddCounterValue(performanceCounterSample) : _stateHandler.SetUndefined());
		_nextSampleTime = _nextSampleTime.AddMilliseconds(TimeBetweenSamples);
		return result;
	}
}
