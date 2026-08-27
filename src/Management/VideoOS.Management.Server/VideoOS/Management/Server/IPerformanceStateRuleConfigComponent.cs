using System;
using System.Collections.Generic;
using VideoOS.WebService.Common.DataCollectorService;

namespace VideoOS.Management.Server;

public interface IPerformanceStateRuleConfigComponent : IDisposable
{
	ResultCollection RegisterStateChange(CounterStateChangeData stateChangeData, bool lookupServerSourcesRequired);

	List<DataCollectorParent> GetDataCollectorMachines();

	PerformanceStateRuleConfig QueryCountersStateRulesDataById(string counterRuleId);

	CounterStateChangeData[] SetStatesByRule(string counterRuleId, int newState);

	void DeleteStatesFromDatacollector(int datacollectorId, string[] activePaths);

	List<int> QueryActiveRecorders();
}
