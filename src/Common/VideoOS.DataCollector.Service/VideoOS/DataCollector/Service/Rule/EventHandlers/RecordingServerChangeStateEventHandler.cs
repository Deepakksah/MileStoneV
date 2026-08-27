using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VideoOS.DataCollector.Service.Persistence;
using VideoOS.DataCollector.Service.Rule.Events;

namespace VideoOS.DataCollector.Service.Rule.EventHandlers;

public class RecordingServerChangeStateEventHandler : INotificationHandler<RecordingServerChangeStateEvent>
{
	private readonly IRuleRepository _repository;

	private readonly IMediator _mediator;

	public RecordingServerChangeStateEventHandler(IRuleRepository repository, IMediator mediator)
	{
		_repository = repository;
		_mediator = mediator;
	}

	public async Task Handle(RecordingServerChangeStateEvent notification, CancellationToken cancellationToken)
	{
		foreach (IRule rule in _repository.CounterRules.Values)
		{
			if (notification.RecordingServerRunning)
			{
				rule.OnRecordingServerUp();
			}
			else
			{
				rule.OnRecordingServerDown();
			}
			IEnumerable<INotification> domainEvents = rule.DomainEvents;
			foreach (INotification item in domainEvents ?? Enumerable.Empty<INotification>())
			{
				await _mediator.Publish(item, cancellationToken);
			}
			rule.ClearDomainEvents();
		}
	}
}
