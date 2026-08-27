using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace VideoOS.IdentityServer.DomainEventInfrastructure.Monitoring;

public class OutboxMetricsTracker : IOutboxMetricsTracker
{
	private readonly ILogger<OutboxMetricsTracker> _logger;

	private readonly OutboxProcessorOptions _settings;

	private readonly int _performanceLogThreshold;

	private DateTime _startTime;

	private int _totalMessageCount;

	private int _totalBatchCount;

	private long _totalProcessingTicks;

	private long _maxBatchTicks;

	private long _minBatchTicks = long.MaxValue;

	private int _emptyBatchCount;

	private int _partialBatchCount;

	private int _currentPendingStreak;

	private int _maxPendingStreak;

	private bool _isProcessing;

	private int _nextThreshold;

	public OutboxMetricsTracker(ILogger<OutboxMetricsTracker> logger, IOptions<OutboxProcessorOptions> settings)
	{
		_logger = logger;
		_settings = settings.Value;
		_performanceLogThreshold = _settings.Monitoring.LogPerformanceAfterMessages;
	}

	public async Task<OutboxMessageResult> MonitorBatch(Func<Task<OutboxMessageResult>> work, string instanceId)
	{
		if (!_settings.Monitoring.Enabled)
		{
			return await work();
		}
		Stopwatch stopwatch = Stopwatch.StartNew();
		OutboxMessageResult outboxMessageResult = await work();
		stopwatch.Stop();
		TrackBatch(outboxMessageResult.Messages.Count, stopwatch.Elapsed, instanceId, outboxMessageResult);
		return outboxMessageResult;
	}

	private void TrackBatch(int messageCount, TimeSpan duration, string instanceId, OutboxMessageResult result)
	{
		if (!_isProcessing && (messageCount > 0 || result.HasPendingMessages))
		{
			InitializeNewMeasurement();
		}
		if (!_isProcessing)
		{
			return;
		}
		if (messageCount == 0 || messageCount < _settings.BatchSize)
		{
			if (messageCount == 0)
			{
				_emptyBatchCount++;
			}
			else
			{
				_partialBatchCount++;
			}
			_currentPendingStreak++;
			_maxPendingStreak = Math.Max(_maxPendingStreak, _currentPendingStreak);
		}
		else
		{
			_currentPendingStreak = 0;
		}
		if (messageCount > 0)
		{
			_totalMessageCount += messageCount;
			_totalBatchCount++;
			_totalProcessingTicks += duration.Ticks;
			if (duration.TotalMilliseconds > 1000.0)
			{
				_logger.LogWarning("Long batch detected - Duration: {Duration}ms, Messages: {Count}, Instance: {Instance}", duration.TotalMilliseconds.ToString("N2"), messageCount, instanceId);
			}
			_maxBatchTicks = Math.Max(_maxBatchTicks, duration.Ticks);
			_minBatchTicks = Math.Min(_minBatchTicks, duration.Ticks);
		}
		if (ShouldLogPerformance())
		{
			LogPerformanceSummary(instanceId);
			_nextThreshold += _performanceLogThreshold;
		}
		if (!result.HasPendingMessages)
		{
			LogFinalSummary(instanceId);
			ResetMetrics();
		}
	}

	private void InitializeNewMeasurement()
	{
		_logger.LogInformation("Starting new measurement");
		_startTime = DateTime.UtcNow;
		_nextThreshold = _performanceLogThreshold;
		_isProcessing = true;
	}

	private void ResetMetrics()
	{
		_totalMessageCount = 0;
		_totalBatchCount = 0;
		_totalProcessingTicks = 0L;
		_maxBatchTicks = 0L;
		_minBatchTicks = long.MaxValue;
		_emptyBatchCount = 0;
		_partialBatchCount = 0;
		_currentPendingStreak = 0;
		_maxPendingStreak = 0;
		_nextThreshold = _performanceLogThreshold;
		_isProcessing = false;
	}

	private bool ShouldLogPerformance()
	{
		if (_performanceLogThreshold > 0)
		{
			return _totalMessageCount >= _nextThreshold;
		}
		return false;
	}

	private OutboxMetrics GetCurrentMetrics()
	{
		TimeSpan totalProcessingTime = TimeSpan.FromTicks(_totalProcessingTicks);
		int num = _emptyBatchCount + _partialBatchCount;
		int num2 = _totalBatchCount + _emptyBatchCount;
		return new OutboxMetrics
		{
			StartTime = _startTime,
			TotalMessageCount = _totalMessageCount,
			TotalBatchCount = _totalBatchCount,
			TotalProcessingTime = totalProcessingTime,
			AverageMessagesPerBatch = ((_totalBatchCount > 0) ? ((double)_totalMessageCount / (double)_totalBatchCount) : 0.0),
			AverageMessagesPerSecond = ((totalProcessingTime.TotalSeconds > 0.0) ? ((double)_totalMessageCount / totalProcessingTime.TotalSeconds) : 0.0),
			AverageBatchDuration = ((_totalBatchCount > 0) ? TimeSpan.FromTicks(_totalProcessingTicks / _totalBatchCount) : TimeSpan.Zero),
			MinBatchDuration = ((_minBatchTicks < long.MaxValue) ? TimeSpan.FromTicks(_minBatchTicks) : TimeSpan.Zero),
			MaxBatchDuration = TimeSpan.FromTicks(_maxBatchTicks),
			EmptyBatchCount = _emptyBatchCount,
			PartialBatchCount = _partialBatchCount,
			TotalPartialBatchCount = num,
			PartialBatchRate = ((num2 > 0) ? ((double)num * 100.0 / (double)num2) : 0.0),
			MaxPartialBatchStreak = _maxPendingStreak
		};
	}

	private void LogMetrics(string instanceId, bool isCompleted = false)
	{
		OutboxMetrics currentMetrics = GetCurrentMetrics();
		_logger.LogInformation("\r\n===== Outbox {Type} =====\r\nInstance:               {Instance}\r\nTotal Messages:         {Count,12}\r\nTotal Batches:          {BatchCount,12}\r\nTotal Process Time:     {ProcessTime,12} seconds\r\nAvg Batch Size:         {BatchSize,12} messages\r\nAvg Process Rate:       {Rate,12} messages/sec\r\nAvg Batch Duration:     {BatchDuration,12} ms\r\nMin Batch Duration:     {MinBatchDuration,12} ms\r\nMax Batch Duration:     {MaxBatchDuration,12} ms\r\nBatch Stats:\r\n  Empty Batches:        {EmptyBatches,12}\r\n  Partial Batches:      {PartialBatches,12}\r\n  Total Partial:        {PartialTotal,12}\r\n  Partial Rate:         {PartialRate,12}%\r\n  Max Partial Streak:   {MaxStreak,12}\r\nBatch Size Setting:     {ConfiguredBatchSize,12}\r\n=====================================", isCompleted ? "Processing Completed" : "Performance Summary", instanceId, currentMetrics.TotalMessageCount.ToString("N0"), currentMetrics.TotalBatchCount.ToString("N0"), currentMetrics.TotalProcessingTime.TotalSeconds.ToString("N2"), currentMetrics.AverageMessagesPerBatch.ToString("N1"), currentMetrics.AverageMessagesPerSecond.ToString("N1"), currentMetrics.AverageBatchDuration.TotalMilliseconds.ToString("N2"), currentMetrics.MinBatchDuration.TotalMilliseconds.ToString("N2"), currentMetrics.MaxBatchDuration.TotalMilliseconds.ToString("N2"), currentMetrics.EmptyBatchCount, currentMetrics.PartialBatchCount, currentMetrics.TotalPartialBatchCount, currentMetrics.PartialBatchRate.ToString("N1"), currentMetrics.MaxPartialBatchStreak, _settings.BatchSize);
	}

	private void LogPerformanceSummary(string instanceId)
	{
		LogMetrics(instanceId);
	}

	private void LogFinalSummary(string instanceId)
	{
		LogMetrics(instanceId, isCompleted: true);
	}
}
