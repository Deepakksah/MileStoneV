using System;

namespace VideoOS.IdentityServer.DomainEventInfrastructure.Monitoring;

public record OutboxMetrics
{
	public DateTime StartTime { get; init; }

	public int TotalMessageCount { get; init; }

	public int TotalBatchCount { get; init; }

	public TimeSpan TotalProcessingTime { get; init; }

	public double AverageMessagesPerBatch { get; init; }

	public double AverageMessagesPerSecond { get; init; }

	public TimeSpan AverageBatchDuration { get; init; }

	public TimeSpan MinBatchDuration { get; init; }

	public TimeSpan MaxBatchDuration { get; init; }

	public int EmptyBatchCount { get; init; }

	public int PartialBatchCount { get; init; }

	public int TotalPartialBatchCount { get; init; }

	public double PartialBatchRate { get; init; }

	public int MaxPartialBatchStreak { get; init; }
}
