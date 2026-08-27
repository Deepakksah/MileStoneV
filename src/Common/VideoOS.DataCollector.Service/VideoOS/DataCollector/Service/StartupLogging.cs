using System;
using System.Runtime;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VideoOS.DataCollector.Service.Configuration;

namespace VideoOS.DataCollector.Service;

public static class StartupLogging
{
	public static void Log(ILogger logger, IOptions<PerformanceCounterOptions> options)
	{
		logger.LogInformation("DataCollector service starting");
		logger.LogInformation(" Version                     : {0}", "25");
		logger.LogInformation(" Number of processors        : {0}", Environment.ProcessorCount);
		logger.LogInformation(" Server GC enabled           : {0}", GCSettings.IsServerGC);
		logger.LogInformation(" Platform                    : {0}-bit", IntPtr.Size * 8);
		logger.LogInformation("");
		logger.LogInformation("Configuration");
		logger.LogInformation(" Config refresh interval     : {0} secs", options.Value.ConfigurationRefreshIntervalInSec);
		logger.LogInformation(" Max batch size              : {0}", options.Value.BatchSize);
		logger.LogInformation(" Max queue lifetime          : {0} secs", options.Value.BatchSenderTimeInSeconds);
	}
}
