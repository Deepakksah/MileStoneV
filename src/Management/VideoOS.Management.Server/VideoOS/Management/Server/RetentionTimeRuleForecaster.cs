using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VideoOS.Utilities.Database;
using VideoOS.WebService.Common.DataCollectorService;

namespace VideoOS.Management.Server;

public class RetentionTimeRuleForecaster : IRetentionTimeRuleForecaster
{
	public async System.Threading.Tasks.Task DoForeCastOnStorageFromDataCollector(IGrouping<int, RetentionTimeRuleConfig> storagesGroupedByDataCollector, IList<RuleSource> ruleOverrides, ForecastConfigData config, CancellationToken token, IRetentionTimeRuleProvider dataProvider, IScheduleLogger logger)
	{
		CounterState resultState = CounterState.Normal;
		foreach (RetentionTimeRuleConfig storageCounter in storagesGroupedByDataCollector)
		{
			ForecastConfigData forecastConfigData = GetForecastConfigData(ruleOverrides, config, storageCounter);
			GeminiTrace.Info(string.Format(CultureInfo.InvariantCulture, "Retention Time Rule: Forecast started on Storage = {0}, Warning = {1}, Critical = {2}", storageCounter.StorageId, forecastConfigData.WarningDays, forecastConfigData.CriticalDays));
			CounterState counterState = DoForecast(dataProvider.GetRetentionTimeHistoricalData(forecastConfigData, storageCounter, logger), storageCounter, forecastConfigData);
			if (counterState > resultState)
			{
				resultState = counterState;
			}
			try
			{
				await System.Threading.Tasks.Task.Delay(1000, token);
			}
			catch (OperationCanceledException)
			{
				GeminiTrace.Info(string.Format(CultureInfo.InvariantCulture, "Retention Time Rule: Forecast canceled", storageCounter.StorageId));
				throw;
			}
		}
		await FireStateChange(resultState, storagesGroupedByDataCollector.Key, config, token);
	}

	private static ForecastConfigData GetForecastConfigData(IList<RuleSource> ruleOverrides, ForecastConfigData configData, RetentionTimeRuleConfig ruleConfig)
	{
		RuleSource ruleSource = ruleOverrides.FirstOrDefault((RuleSource r) => r.CounterId == ruleConfig.CounterId);
		if (ruleSource != null)
		{
			return new ForecastConfigData(TimeSpan.FromDays(ruleSource.WarningThreshold + 1.0), TimeSpan.FromDays(ruleSource.CriticalThreshold), TimeSpan.FromDays(ruleSource.WarningThreshold), TimeSpan.FromDays(ruleSource.TimeWindow), configData.HistoricalSampleCount);
		}
		return configData;
	}

	private static CounterState DoForecast(RetentionTimeHistoricalData[] data, RetentionTimeRuleConfig config, ForecastConfigData forecastConfig)
	{
		TimeSpan timeSpan = RetentionTimeForecast.CalculateRetentionTimeLimitHit(data, config.StorageSizeMb * 1024 * 1024, forecastConfig.ForecastPeriod);
		if (timeSpan <= forecastConfig.WarningDays)
		{
			if (timeSpan <= forecastConfig.CriticalDays)
			{
				GeminiTrace.Info(string.Format(CultureInfo.InvariantCulture, "Retention Time Rule: State change to Critical fired on Storage = {0}", config.StorageId.ToString()));
				return CounterState.Critical;
			}
			GeminiTrace.Info(string.Format(CultureInfo.InvariantCulture, "Retention Time Rule: State change to Warning fired on Storage = {0}", config.StorageId.ToString()));
			return CounterState.Warning;
		}
		GeminiTrace.Info(string.Format(CultureInfo.InvariantCulture, "Retention Time Rule: State change to Normal fired on Storage = {0}", config.StorageId.ToString()));
		return CounterState.Normal;
	}

	private async System.Threading.Tasks.Task FireStateChange(CounterState state, int dataCollectorId, ForecastConfigData forecastConfig, CancellationToken token)
	{
		GeminiRequestContext systemRequestContext = GeminiApplicationCore.RuntimeServiceHost.CreateSystemContext();
		try
		{
			CounterStateChangeData stateData = new CounterStateChangeData
			{
				CounterRuleId = "BDFC173B-43C7-4600-A483-82ED37CFD73C",
				CounterPath = "\\VideoOS Recording Server Storage",
				State = (int)state,
				DataCollectorId = dataCollectorId,
				TriggerValue = (double)state
			};
			try
			{
				await AsyncRetryExecution.ExecuteWithRetry(delegate
				{
					PerformanceStateRuleConfig.RegisterStateChange(systemRequestContext, stateData);
				}, token, "Retention Time Rule: Failed to register state change - retrying..", 10000, typeof(DatabaseException));
			}
			catch (OperationCanceledException)
			{
				GeminiTrace.Info("Retention Time Rule: rule canceled and stopped");
				throw;
			}
		}
		finally
		{
			if (systemRequestContext != null)
			{
				((IDisposable)systemRequestContext).Dispose();
			}
		}
	}
}
