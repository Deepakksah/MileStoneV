using System.Collections.Concurrent;
using System.Collections.Generic;
using VideoOS.DataCollector.Service.Rule.Entities.Rules;
using VideoOS.DataCollector.Service.Rule.RuleConfiguration.Refresh.EventHandlers.RefreshRules;

namespace VideoOS.DataCollector.Service.Persistence;

public interface IRuleRepository
{
	ConcurrentDictionary<string, IRule> CounterRules { get; }

	ConcurrentDictionary<string, IRule> NonCounterRules { get; }

	ConcurrentDictionary<string, IMultiCounterRule> MultiRules { get; }

	IEnumerable<RefreshRuleConfigurationEventHandler.RuleConfiguration> RuleConfigurations { get; set; }
}
