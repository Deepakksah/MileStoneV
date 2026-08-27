using FluentValidation;

namespace VideoOS.IdentityServer.DomainEventInfrastructure;

public class OutboxMessageValidator : AbstractValidator<OutboxMessage>
{
	public OutboxMessageValidator()
	{
		RuleFor((OutboxMessage x) => x.EventType).NotEmpty().WithMessage("EventType is required.").DependentRules(delegate
		{
			RuleFor((OutboxMessage x) => x.EventType).MaximumLength(200).WithMessage("EventType cannot exceed 200 characters.");
		});
		RuleFor((OutboxMessage x) => x.Payload).NotEmpty().WithMessage("Payload is required.");
	}
}
