using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VideoOS.Management.Server;

public class RetentionTimeRule : IDisposable
{
	private IScheduledActionController<ForecastConfigData> _scheduleController;

	private IScheduledActionConfigurationProvider<ForecastConfigData> _configurationProvider;

	private IRetentionTimeRuleProvider _retentionTimeRuleProvider;

	private IRetentionTimeRuleForecaster _retentionTimeRuleForecaster;

	private IScheduledAction<ForecastConfigData> _scheduleAction;

	private IScheduleLogger _logger;

	public RetentionTimeRule(IScheduledActionController<ForecastConfigData> scheduleController, IScheduledActionConfigurationProvider<ForecastConfigData> configurationProvider, IRetentionTimeRuleProvider retentionTimeRuleProvider, IRetentionTimeRuleForecaster retentionTimeRuleForecaster, IScheduledAction<ForecastConfigData> scheduleAction, IScheduleLogger logger)
	{
		_retentionTimeRuleForecaster = retentionTimeRuleForecaster;
		_retentionTimeRuleProvider = retentionTimeRuleProvider;
		_scheduleController = scheduleController;
		_scheduleAction = scheduleAction;
		_configurationProvider = configurationProvider;
		_logger = logger;
	}

	public RetentionTimeRule(IScheduleLogger logger)
	{
		_retentionTimeRuleForecaster = new RetentionTimeRuleForecaster();
		_retentionTimeRuleProvider = new RetentionTimeRuleProvider();
		_scheduleAction = new ScheduledAction<ForecastConfigData>(RuleActionAsync, new TimeSpan(15, 0, 0));
		_scheduleController = new ScheduledActionController<ForecastConfigData>(_scheduleAction);
		_configurationProvider = new RetentionTimeRuleConfigurationProvider(_retentionTimeRuleProvider, logger);
		_logger = logger;
	}

	public void Update()
	{
		System.Threading.Tasks.Task.Run(delegate
		{
			_scheduleController.UpdateSchedule(_configurationProvider);
		});
	}

	public async System.Threading.Tasks.Task RuleActionAsync(object state)
	{
		if (!(state is ScheduledActionData<ForecastConfigData> config))
		{
			throw new ArgumentException("RetentionTimeRule. config is null");
		}
		try
		{
			RetentionTimeRuleConfig[] storageCounters = _retentionTimeRuleProvider.GetStorageCounters(_logger);
			IList<RuleSource> ruleOverrides = _retentionTimeRuleProvider.GetRuleSources(_logger);
			if (config.Token.IsCancellationRequested)
			{
				_logger.LogInfo("Retention Time Rule: Forecast canceled");
			}
			else
			{
				if (storageCounters == null || storageCounters.Length == 0)
				{
					return;
				}
				foreach (IGrouping<int, RetentionTimeRuleConfig> item in from s in storageCounters
					group s by s.DatacollectorId)
				{
					await _retentionTimeRuleForecaster.DoForeCastOnStorageFromDataCollector(item, ruleOverrides, config.InputData, config.Token, _retentionTimeRuleProvider, _logger);
				}
			}
		}
		catch (Exception ex)
		{
			if (ex is OperationCanceledException)
			{
				_logger.LogInfo("Rule Action stopped due to cancellation");
			}
			else
			{
				_logger.LogException("Rule Action stopped", ex);
			}
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (disposing)
		{
			if (_scheduleController != null)
			{
				_scheduleController.Dispose();
			}
			if (_scheduleAction != null)
			{
				_scheduleAction.Dispose();
			}
		}
	}
}
