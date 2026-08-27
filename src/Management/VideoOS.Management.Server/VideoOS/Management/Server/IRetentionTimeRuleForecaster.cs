using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace VideoOS.Management.Server;

public interface IRetentionTimeRuleForecaster
{
	System.Threading.Tasks.Task DoForeCastOnStorageFromDataCollector(IGrouping<int, RetentionTimeRuleConfig> storagesGroupedByDataCollector, IList<RuleSource> ruleOverrides, ForecastConfigData config, CancellationToken token, IRetentionTimeRuleProvider dataProvider, IScheduleLogger logger);
}
