using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VideoOS.DataCollector.Service.Rule.Events;

namespace VideoOS.DataCollector.Service.Rule.RuleConfiguration.Commands;

public class PostUpdatedCounterStateRuleConfigurationsCommandHandler : IRequestHandler<PostUpdatedCounterStateRuleConfigurationsCommand, Unit>
{
	private readonly IMediator _mediator;

	public PostUpdatedCounterStateRuleConfigurationsCommandHandler(IMediator mediator)
	{
		_mediator = mediator;
	}

	public async Task<Unit> Handle(PostUpdatedCounterStateRuleConfigurationsCommand request, CancellationToken cancellationToken)
	{
		await _mediator.Publish(new RuleConfigurationChangedEvent(request.RulesConfigurationUpdates), CancellationToken.None);
		return default(Unit);
	}
}
