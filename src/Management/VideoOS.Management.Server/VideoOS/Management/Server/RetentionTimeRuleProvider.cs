using System;
using System.Collections.Generic;

namespace VideoOS.Management.Server;

public class RetentionTimeRuleProvider : IRetentionTimeRuleProvider
{
	public RetentionTimeHistoricalData[] GetRetentionTimeHistoricalData(ForecastConfigData storageConfig, RetentionTimeRuleConfig ruleConfig, IScheduleLogger logger)
	{
		using GeminiRequestContext requestContext = GeminiApplicationCore.RuntimeServiceHost.CreateSystemContext();
		try
		{
			return RetentionTimeHistoricalData.QueryRetentionTimeHistoricalData(requestContext, new RetentionTimeQueryData
			{
				CounterInstanceId = ruleConfig.CounterId,
				Days = storageConfig.HistoricalDataPeriod.Days,
				SampleCount = storageConfig.HistoricalSampleCount,
				StartDate = DateTime.UtcNow
			});
		}
		catch (Exception ex)
		{
			logger.LogException("Retention Time Rule: Failed to query historical retention time data", ex);
			throw;
		}
	}

	public IList<RuleSource> GetRuleSources(IScheduleLogger logger)
	{
		using GeminiRequestContext requestContext = GeminiApplicationCore.RuntimeServiceHost.CreateSystemContext();
		try
		{
			return RuleSource.QueryRuleSourceByCounterRuleId(requestContext, "BDFC173B-43C7-4600-A483-82ED37CFD73C");
		}
		catch (Exception ex)
		{
			logger.LogException("Retention Time Rule: Failed to query storage counters", ex);
			throw;
		}
	}

	public RetentionTimeRuleConfig[] GetStorageCounters(IScheduleLogger logger)
	{
		using GeminiRequestContext requestContext = GeminiApplicationCore.RuntimeServiceHost.CreateSystemContext();
		try
		{
			return RetentionTimeRuleConfig.QueryStorageCounters(requestContext);
		}
		catch (Exception ex)
		{
			logger.LogException("Retention Time Rule: Failed to query storage counters", ex);
			throw;
		}
	}

	public PerformanceStateRuleConfig QueryRuleConfiguration()
	{
		using GeminiRequestContext requestContext = GeminiApplicationCore.RuntimeServiceHost.CreateSystemContext();
		return PerformanceStateRuleConfig.QueryPerformanceStateRuleConfigById(requestContext, "BDFC173B-43C7-4600-A483-82ED37CFD73C");
	}
}
