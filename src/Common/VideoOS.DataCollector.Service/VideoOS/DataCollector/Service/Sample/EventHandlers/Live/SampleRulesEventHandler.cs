using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VideoOS.DataCollector.Service.Persistence;
using VideoOS.DataCollector.Service.Rule.Entities.Sample;
using VideoOS.DataCollector.Service.Sample.Events;

namespace VideoOS.DataCollector.Service.Sample.EventHandlers.Live;

public class SampleRulesEventHandler : INotificationHandler<SampleRulesEvent>
{
	private readonly IRuleRepository _repository;

	private readonly IMediator _mediator;

	private readonly ISamplerFactory _samplerFactory;

	public SampleRulesEventHandler(IRuleRepository repository, IMediator mediator, ISamplerFactory samplerFactory)
	{
		_repository = repository;
		_mediator = mediator;
		_samplerFactory = samplerFactory;
	}

	public Task Handle(SampleRulesEvent notification, CancellationToken cancellationToken)
	{
		(from x in _repository.CounterRules.AsParallel()
			select x.Value).ForAll(async delegate(IRule rule)
		{
			await SampleRuleAndSendIfChanged(rule, cancellationToken);
		});
		(from x in _repository.NonCounterRules.AsParallel()
			select x.Value).ForAll(async delegate(IRule rule)
		{
			await SampleRuleAndSendIfChanged(rule, cancellationToken);
		});
		return Task.CompletedTask;
	}

	private async Task SampleRuleAndSendIfChanged(IRule rule, CancellationToken cancellationToken)
	{
		await rule.SampleIfDue(_samplerFactory, cancellationToken);
		IEnumerable<INotification> domainEvents = rule.DomainEvents;
		foreach (INotification item in domainEvents ?? Enumerable.Empty<INotification>())
		{
			await _mediator.Publish(item, cancellationToken);
		}
		rule.ClearDomainEvents();
	}
}
