namespace VideoOS.Management.Server.ConfigurationApi.Rules.Proxies;

public class MonitorProxy : IMonitorProxy
{
	public Monitor QueryMonitor(IGeminiRequestContext requestContext, string monitorId)
	{
		return Monitor.QueryMonitor(requestContext, monitorId);
	}
}
