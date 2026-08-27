using System.Threading.Tasks;

namespace VideoOS.Management.Server;

public interface IPerfomanceCounterUpdatesManager
{
	Task<bool> PostUpdatedItems(IGeminiRequestContext requestContext, PerformanceStateRuleConfig newConfig, PerformanceStateRuleConfig oldConfig);

	Task<bool> PostDeletedItems(IGeminiRequestContext requestContext, PerformanceStateRuleConfig oldConfig);

	Task<bool> PostCreatedItems(IGeminiRequestContext requestContext, PerformanceStateRuleConfig newConfig);

	Task<bool> PostDeletedSources(IGeminiRequestContext requestContext, RuleSource existingRuleSource);

	Task<bool> PostUpdatedSources(IGeminiRequestContext requestContext, RuleSource newSource, RuleSource oldSource);

	Task<bool> PostCreatedSources(IGeminiRequestContext requestContext, RuleSource newSource);
}
