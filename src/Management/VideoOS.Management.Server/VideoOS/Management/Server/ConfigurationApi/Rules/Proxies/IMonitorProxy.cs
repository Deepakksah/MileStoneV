namespace VideoOS.Management.Server.ConfigurationApi.Rules.Proxies;

public interface IMonitorProxy
{
	Monitor QueryMonitor(IGeminiRequestContext requestContext, string monitorId);
}
