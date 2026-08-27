using VideoOS.IdentityServer.DomainEventInfrastructure;

namespace VideoOS.IdentityServer.DomainEvents.VmsUser;

public record UserDeletedEvent(string Id) : DomainEvent(EventLevel.Level100);
