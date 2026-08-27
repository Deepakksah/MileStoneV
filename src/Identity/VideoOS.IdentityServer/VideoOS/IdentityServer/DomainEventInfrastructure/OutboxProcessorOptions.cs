using VideoOS.IdentityServer.DomainEventInfrastructure.Monitoring;

namespace VideoOS.IdentityServer.DomainEventInfrastructure;

public class OutboxProcessorOptions
{
	public int BatchSize { get; set; } = 1000;

	public int PollingIntervalSeconds { get; set; } = 5;

	public int MaxRetryDelayInPollingIntervals { get; set; } = 12;

	public int LockExpirationSeconds { get; set; } = 15;

	public MonitoringSettings Monitoring { get; set; } = new MonitoringSettings();

	public int RetentionDurationInDays { get; set; } = 30;
}
