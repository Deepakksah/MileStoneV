namespace VideoOS.IdentityServer.DomainEventInfrastructure.Monitoring;

public class MonitoringSettings
{
	public bool Enabled { get; set; }

	public int LogPerformanceAfterMessages { get; set; } = 1000;
}
