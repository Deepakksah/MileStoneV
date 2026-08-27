using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using VideoOS.DataCollector.Service.Rule.Entities.Rules;
using VideoOS.DataCollector.Service.Rule.RuleConfiguration.Refresh.EventHandlers.RefreshRules;

namespace VideoOS.DataCollector.Service.Persistence;

public class RuleRepository : IRuleRepository
{
	public const string DiskFreeSpace = "47B9163F-D9EB-43B4-BB3F-4BCCA591D5F4";

	public const string DatabaseAvailableState = "E8240A18-9A93-424D-B45C-E83DCDD99EB5";

	public const string ArchiveAvailableState = "D128E0E8-57E5-481E-8936-74423687CA82";

	public ConcurrentDictionary<string, IRule> CounterRules { get; } = new ConcurrentDictionary<string, IRule>();

	public ConcurrentDictionary<string, IRule> NonCounterRules { get; } = new ConcurrentDictionary<string, IRule>();

	public ConcurrentDictionary<string, IMultiCounterRule> MultiRules { get; } = new ConcurrentDictionary<string, IMultiCounterRule>(StringComparer.OrdinalIgnoreCase);

	public IEnumerable<RefreshRuleConfigurationEventHandler.RuleConfiguration> RuleConfigurations { get; set; } = new List<RefreshRuleConfigurationEventHandler.RuleConfiguration>();

	public RuleRepository()
	{
		MultiRules.TryAdd("47B9163F-D9EB-43B4-BB3F-4BCCA591D5F4", new MultiCounterRule("\\VideoOS Recording Server Database Disk", "47B9163F-D9EB-43B4-BB3F-4BCCA591D5F4"));
		MultiRules.TryAdd("E8240A18-9A93-424D-B45C-E83DCDD99EB5", new MultiCounterRule("\\VideoOS Recording Server Storage Database", "E8240A18-9A93-424D-B45C-E83DCDD99EB5"));
		MultiRules.TryAdd("D128E0E8-57E5-481E-8936-74423687CA82", new MultiCounterRule("\\VideoOS Recording Server Storage Archive", "D128E0E8-57E5-481E-8936-74423687CA82"));
	}
}
