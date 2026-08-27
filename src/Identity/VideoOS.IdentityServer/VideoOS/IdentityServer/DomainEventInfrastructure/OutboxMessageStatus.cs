namespace VideoOS.IdentityServer.DomainEventInfrastructure;

public enum OutboxMessageStatus
{
	Created,
	Processing,
	Delivered,
	Failed,
	RetryRequired
}
