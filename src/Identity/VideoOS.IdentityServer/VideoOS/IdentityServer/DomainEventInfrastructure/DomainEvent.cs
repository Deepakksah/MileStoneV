using MediatR;

namespace VideoOS.IdentityServer.DomainEventInfrastructure;

public abstract record DomainEvent(EventLevel CompatibilityLevel) : INotification
{
	public EventLevel CompatibilityLevel { get; } = CompatibilityLevel;
}
