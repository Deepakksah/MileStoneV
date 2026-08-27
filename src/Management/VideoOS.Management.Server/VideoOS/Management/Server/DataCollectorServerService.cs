using System.Collections.Generic;
using VideoOS.Common.Integration;

namespace VideoOS.Management.Server;

public sealed class DataCollectorServerService : DataCollectorParent
{
	public static List<DataCollectorParent> GetMachines(IGeminiRequestContext requestContext)
	{
		return ConvertToDataCollectorParents(SystemMonitorManager.GetServerSourcesByIds(requestContext, PerformanceRuleServiceTypes.RuleServiceTypes));
	}

	private static List<DataCollectorParent> ConvertToDataCollectorParents(IEnumerable<SystemMonitorManager.EventSource> serverSources)
	{
		List<DataCollectorParent> list = new List<DataCollectorParent>();
		foreach (SystemMonitorManager.EventSource serverSource in serverSources)
		{
			DataCollectorParent dataCollectorParent = new DataCollectorParent();
			dataCollectorParent.ParentName = serverSource.Name;
			dataCollectorParent.Id = serverSource.SourceId;
			dataCollectorParent.DataCollectorId = serverSource.DataCollectorId;
			dataCollectorParent.ServerType = serverSource.ServerType;
			dataCollectorParent.Children = new List<DataCollectorChild>();
			list.Add(dataCollectorParent);
		}
		return list;
	}
}
