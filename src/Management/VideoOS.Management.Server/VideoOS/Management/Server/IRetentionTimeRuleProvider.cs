using System.Collections.Generic;

namespace VideoOS.Management.Server;

public interface IRetentionTimeRuleProvider
{
	PerformanceStateRuleConfig QueryRuleConfiguration();

	RetentionTimeRuleConfig[] GetStorageCounters(IScheduleLogger logger);

	IList<RuleSource> GetRuleSources(IScheduleLogger logger);

	RetentionTimeHistoricalData[] GetRetentionTimeHistoricalData(ForecastConfigData storageConfig, RetentionTimeRuleConfig ruleConfig, IScheduleLogger logger);
}
