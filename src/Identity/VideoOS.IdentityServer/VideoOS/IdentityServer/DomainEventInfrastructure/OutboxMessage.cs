using System;

namespace VideoOS.IdentityServer.DomainEventInfrastructure;

public record OutboxMessage
{
	public Guid Id { get; set; }

	public string EventType { get; set; }

	public string Payload { get; set; }

	public int CompatibilityLevel { get; set; }

	public OutboxMessageStatus Status { get; set; }

	public DateTime CreatedAtUtc { get; set; }

	public DateTime? ProcessedAtUtc { get; set; }

	public string ProcessingInstanceId { get; set; }

	public DateTime? LockExpiresAtUtc { get; set; }

	public DateTime? LastAttemptUtc { get; set; }

	public int RetryCount { get; set; }

	public string Error { get; set; }
}
