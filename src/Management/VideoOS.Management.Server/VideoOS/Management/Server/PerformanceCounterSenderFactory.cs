using System.Linq;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

public class PerformanceCounterSenderFactory : IPerformanceCounterSenderFactory
{
	private ServerPerformanceCounterUpdateSender _serverPerformanceCounterUpdateSender = new ServerPerformanceCounterUpdateSender();

	private DatacollectorPerformanceCounterUpdateSender _datacollectorPerformanceCounterUpdateSender = new DatacollectorPerformanceCounterUpdateSender();

	public IPerformanceCounterUpdateSender GetSender(int ruleType)
	{
		if (PerformanceStateRuleConfigType.TriggeredByManagementServer.Contains(ruleType))
		{
			return _serverPerformanceCounterUpdateSender;
		}
		return _datacollectorPerformanceCounterUpdateSender;
	}
}
