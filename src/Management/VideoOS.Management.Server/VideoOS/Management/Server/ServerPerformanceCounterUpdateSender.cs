using System;
using System.Threading.Tasks;

namespace VideoOS.Management.Server;

public class ServerPerformanceCounterUpdateSender : IPerformanceCounterUpdateSender
{
	public Task<bool> PostCreatedItems(IGeminiRequestContext requestContext, PerformanceStateRuleConfig newConfig)
	{
		return System.Threading.Tasks.Task.FromResult(result: true);
	}

	public Task<bool> PostCreatedSources(IGeminiRequestContext requestContext, RuleSource newSource)
	{
		SendRuleSourceUpdatesToServerRules(newSource, GeminiApplicationCore.RetentionTimeRule.Update);
		return System.Threading.Tasks.Task.FromResult(result: true);
	}

	public Task<bool> PostDeletedItems(IGeminiRequestContext requestContext, PerformanceStateRuleConfig oldConfig)
	{
		return System.Threading.Tasks.Task.FromResult(result: true);
	}

	public Task<bool> PostDeletedSources(IGeminiRequestContext requestContext, RuleSource existingRuleSource)
	{
		SendRuleSourceUpdatesToServerRules(existingRuleSource, GeminiApplicationCore.RetentionTimeRule.Update);
		return System.Threading.Tasks.Task.FromResult(result: true);
	}

	public Task<bool> PostUpdatedItems(IGeminiRequestContext requestContext, PerformanceStateRuleConfig newConfig, PerformanceStateRuleConfig oldConfig)
	{
		SendUpdatesToServerRules(newConfig, oldConfig, GeminiApplicationCore.RetentionTimeRule.Update);
		return System.Threading.Tasks.Task.FromResult(result: true);
	}

	public Task<bool> PostUpdatedSources(IGeminiRequestContext requestContext, RuleSource newSource, RuleSource oldSource)
	{
		SendRuleSourceUpdatesToServerRules(newSource, GeminiApplicationCore.RetentionTimeRule.Update);
		return System.Threading.Tasks.Task.FromResult(result: true);
	}

	public static void SendUpdatesToServerRules(PerformanceStateRuleConfig newConfig, PerformanceStateRuleConfig oldConfig, Action updateAction)
	{
		if (newConfig.RuleType == 7 && oldConfig.RuleType == 7 && (Math.Abs(newConfig.CriticalThreshold - oldConfig.CriticalThreshold) > 0.001 || Math.Abs(newConfig.WarningThreshold - oldConfig.WarningThreshold) > 0.001 || newConfig.TimeWindow != oldConfig.TimeWindow || newConfig.Disabled != oldConfig.Disabled))
		{
			updateAction();
		}
	}

	public static void SendRuleSourceUpdatesToServerRules(RuleSource newSource, Action updateAction)
	{
		if (newSource.RuleType == 7)
		{
			updateAction();
		}
	}
}
