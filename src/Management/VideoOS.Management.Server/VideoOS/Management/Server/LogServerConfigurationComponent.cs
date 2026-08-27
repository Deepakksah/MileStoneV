using System;
using System.Threading.Tasks;
using VideoOS.Common.Services;
using VideoOS.LogServer.Communication.LogConfiguration;
using VideoOS.LogServer.Communication.LogConfigurationServiceReference;
using VideoOS.Utilities.Telemetry;

namespace VideoOS.Management.Server;

public class LogServerConfigurationComponent : GeminiResourceComponent, ILogServerConfigurationComponent
{
	private ILogConfigurationHandler ConstructHandler()
	{
		RegisteredServer[] array = RequestContext.CreateRegisteredServerComponent().QueryRegisteredServersByType(ServiceTypes.ServiceType_LogService);
		if (array != null && array.Length != 0)
		{
			string uRL = array[0].Addresses[0].URL;
			return LogConfigurationHandlerFactory.Create(VideoOS.Utilities.Telemetry.Telemetry.GetILogger("MainApplication"), new Uri(uRL));
		}
		return null;
	}

	public LogRetentions QueryLogServerConfiguration()
	{
		ILogConfigurationHandler handler = ConstructHandler();
		if (handler != null)
		{
			try
			{
				return System.Threading.Tasks.Task.Run(() => handler.GetLogRetentionsAsync()).Result;
			}
			catch (AggregateException ex)
			{
				GeminiTrace.Info("Unable to configurate LogServer:" + ex.Message);
			}
		}
		return null;
	}

	public void UpdateLogServerConfiguration(LogRetentions logRetentions)
	{
		ILogConfigurationHandler logConfigurationHandler = ConstructHandler();
		if (logConfigurationHandler == null)
		{
			return;
		}
		try
		{
			LogRetentions result = logConfigurationHandler.GetLogRetentionsAsync().Result;
			if (result.SystemLogRetention != logRetentions.SystemLogRetention || result.AuditLogRetention != logRetentions.AuditLogRetention || result.RuleLogRetention != logRetentions.RuleLogRetention)
			{
				logConfigurationHandler.UpdateLogRetentionsAsync(logRetentions);
			}
		}
		catch (AggregateException ex)
		{
			GeminiTrace.Info("Unable to configurate LogServer:" + ex.Message);
			throw;
		}
	}
}
