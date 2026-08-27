using System.Threading.Tasks;

namespace VideoOS.Management.Server;

public sealed class PerformanceCounterUpdatesManager : IPerfomanceCounterUpdatesManager
{
	private IPerformanceCounterSenderFactory _performanceCounterSenderFactory;

	public PerformanceCounterUpdatesManager(IPerformanceCounterSenderFactory performanceCounterSenderFactory)
	{
		_performanceCounterSenderFactory = performanceCounterSenderFactory;
	}

	public Task<bool> PostCreatedItems(IGeminiRequestContext requestContext, PerformanceStateRuleConfig newConfig)
	{
		return _performanceCounterSenderFactory.GetSender(newConfig.RuleType).PostCreatedItems(requestContext, newConfig);
	}

	public Task<bool> PostCreatedSources(IGeminiRequestContext requestContext, RuleSource newSource)
	{
		return _performanceCounterSenderFactory.GetSender(newSource.RuleType).PostCreatedSources(requestContext, newSource);
	}

	public Task<bool> PostDeletedItems(IGeminiRequestContext requestContext, PerformanceStateRuleConfig oldConfig)
	{
		return _performanceCounterSenderFactory.GetSender(oldConfig.RuleType).PostDeletedItems(requestContext, oldConfig);
	}

	public Task<bool> PostDeletedSources(IGeminiRequestContext requestContext, RuleSource existingRuleSource)
	{
		return _performanceCounterSenderFactory.GetSender(existingRuleSource.RuleType).PostDeletedSources(requestContext, existingRuleSource);
	}

	public Task<bool> PostUpdatedItems(IGeminiRequestContext requestContext, PerformanceStateRuleConfig newConfig, PerformanceStateRuleConfig oldConfig)
	{
		return _performanceCounterSenderFactory.GetSender(newConfig.RuleType).PostUpdatedItems(requestContext, newConfig, oldConfig);
	}

	public Task<bool> PostUpdatedSources(IGeminiRequestContext requestContext, RuleSource newSource, RuleSource oldSource)
	{
		return _performanceCounterSenderFactory.GetSender(newSource.RuleType).PostUpdatedSources(requestContext, newSource, oldSource);
	}
}
