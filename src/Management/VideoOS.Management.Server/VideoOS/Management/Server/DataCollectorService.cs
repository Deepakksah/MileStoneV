using System.Collections.Generic;

namespace VideoOS.Management.Server;

public sealed class DataCollectorService : DataCollectorParent
{
	public static List<DataCollectorParent> GetMachines(IGeminiRequestContext requestContext)
	{
		using IPerformanceStateRuleConfigComponent performanceStateRuleConfigComponent = requestContext.CreatePerformanceStateRuleConfigComponent();
		return performanceStateRuleConfigComponent.GetDataCollectorMachines();
	}
}
