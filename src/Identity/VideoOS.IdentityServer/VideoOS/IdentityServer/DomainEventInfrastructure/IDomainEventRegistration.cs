namespace VideoOS.IdentityServer.DomainEventInfrastructure;

public interface IDomainEventRegistration
{
	void Register(DomainEventsRegistry registry);
}
