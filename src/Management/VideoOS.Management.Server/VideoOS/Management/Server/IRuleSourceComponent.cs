using System;
using System.Collections.Generic;

namespace VideoOS.Management.Server;

public interface IRuleSourceComponent : IDisposable
{
	List<RuleSource> QueryRuleSourcesByIds(int dataCollectorId, string[] ids, bool generateSources);

	RuleSource QueryRuleSourceById(string ruleSourceId);

	List<RuleSource> QueryRuleSourcesByCounterId(string counterRuleId);

	void CreateRuleSource(RuleSource newRuleSource);

	void UpdateRuleSource(RuleSource newRuleSource);

	void DeleteRuleSource(string sourceId);
}
