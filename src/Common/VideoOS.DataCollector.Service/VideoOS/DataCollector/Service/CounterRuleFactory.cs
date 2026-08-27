using System;
using Microsoft.Extensions.Options;
using VideoOS.DataCollector.Service.Configuration;
using VideoOS.DataCollector.Service.Entities.DataCollection;

namespace VideoOS.DataCollector.Service;

public class CounterRuleFactory : ICounterRuleFactory
{
	private readonly IOptions<RuleOptions> _options;

	public CounterRuleFactory(IOptions<RuleOptions> options)
	{
		_options = options;
	}

	public CounterRule CreateCounterRule(int datacollectoId, CounterStateRuleConfiguration ruleConfiguration, bool isDefault, bool triggerPerValue, bool counterDirection, IStateHandlerFactory stateHandlerFactory)
	{
		if (IsCameraCounter(ruleConfiguration.CategoryName))
		{
			if (IsCameraStateCounter(ruleConfiguration.CounterName))
			{
				return new CameraStateCounterRule(datacollectoId, ruleConfiguration, isDefault, triggerPerValue, counterDirection, stateHandlerFactory, _options);
			}
			if (IsCameraUsedSpaceCounter(ruleConfiguration.CategoryName, ruleConfiguration.CounterName))
			{
				return new CameraUsedSpaceCounterRule(datacollectoId, ruleConfiguration, isDefault, triggerPerValue, counterDirection, stateHandlerFactory, _options);
			}
			return new CameraCounterRule(datacollectoId, ruleConfiguration, isDefault, triggerPerValue, counterDirection, stateHandlerFactory, _options);
		}
		return new CounterRule(datacollectoId, ruleConfiguration, isDefault, triggerPerValue, counterDirection, stateHandlerFactory, _options);
	}

	public bool ValidateCounter(CounterStateRuleConfiguration ruleConfiguration, string counterPath)
	{
		if (IsCameraCounter(ruleConfiguration.CategoryName) && !CameraCounterRule.IsValidPath(counterPath))
		{
			return false;
		}
		return true;
	}

	private static bool IsCameraCounter(string categoryName)
	{
		if (string.Compare(categoryName, "VideoOS Recording Server Device", StringComparison.Ordinal) != 0 && string.Compare(categoryName, "VideoOS Recording Server Device Storage", StringComparison.Ordinal) != 0)
		{
			return string.Compare(categoryName, "VideoOS Recording Server Drivers", StringComparison.Ordinal) == 0;
		}
		return true;
	}

	private static bool IsCameraStateCounter(string counterName)
	{
		return string.Compare(counterName, "State", StringComparison.Ordinal) == 0;
	}

	private static bool IsCameraUsedSpaceCounter(string categoryName, string counterName)
	{
		if (string.Compare(counterName, "Used Bytes", StringComparison.Ordinal) == 0)
		{
			return string.Compare(categoryName, "VideoOS Recording Server Device Storage", StringComparison.Ordinal) == 0;
		}
		return false;
	}
}
