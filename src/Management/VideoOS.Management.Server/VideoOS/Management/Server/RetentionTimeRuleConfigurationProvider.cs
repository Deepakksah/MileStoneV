using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

public class RetentionTimeRuleConfigurationProvider : IScheduledActionConfigurationProvider<ForecastConfigData>
{
	private IRetentionTimeRuleProvider _retentionTimeRuleProvider;

	private IScheduleLogger _logger;

	private int _historicalDataSampleCount = 500;

	public RetentionTimeRuleConfigurationProvider(IRetentionTimeRuleProvider retentionTimeRuleProvider, IScheduleLogger logger)
	{
		_retentionTimeRuleProvider = retentionTimeRuleProvider;
		_logger = logger;
	}

	public async Task<ForecastConfigData> GetActionConfigurationAsync(CancellationToken token)
	{
		PerformanceStateRuleConfig performanceStateRuleConfig = await AsyncRetryExecution.ExecuteWithRetry(() => _retentionTimeRuleProvider.QueryRuleConfiguration(), token, "Retention Time Rule: Failed to query retention time rule configuration - retrying", 3000, typeof(VmoServerException));
		if (performanceStateRuleConfig == null)
		{
			_logger.LogInfo("Retention Time Rule: Configuration is empty, stopping rule");
			return null;
		}
		if (performanceStateRuleConfig.Disabled)
		{
			_logger.LogInfo("Retention Time Rule: rule disabled");
			return null;
		}
		TimeSpan forecastPeriod = TimeSpan.FromDays(performanceStateRuleConfig.WarningThreshold + 1.0);
		TimeSpan timeSpan = TimeSpan.FromDays(performanceStateRuleConfig.CriticalThreshold);
		TimeSpan timeSpan2 = TimeSpan.FromDays(performanceStateRuleConfig.WarningThreshold);
		TimeSpan historicalDataPeriod = TimeSpan.FromDays(performanceStateRuleConfig.TimeWindow);
		_logger.LogInfo(string.Format(CultureInfo.InvariantCulture, "Retention Time Rule: starting rule, with threshold levels: Critical={0}, Warning={1}", timeSpan, timeSpan2));
		return new ForecastConfigData(forecastPeriod, timeSpan, timeSpan2, historicalDataPeriod, _historicalDataSampleCount);
	}
}
