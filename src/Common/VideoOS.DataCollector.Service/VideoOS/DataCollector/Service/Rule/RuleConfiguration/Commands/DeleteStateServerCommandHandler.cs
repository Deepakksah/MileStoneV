using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VideoOS.DataCollector.Service.Common.ResultResponse;
using VideoOS.DataCollector.Service.Entities.DataCollection;
using VideoOS.DataCollector.Service.Rule.Entities.State;

namespace VideoOS.DataCollector.Service.Rule.RuleConfiguration.Commands;

public class DeleteStateServerCommandHandler : IRequestHandler<DeleteStateServerCommand, Result<Unit>>
{
	private readonly IDataCollection _dataCollectorServiceProxy;

	private readonly IMediator _mediator;

	public DeleteStateServerCommandHandler(IDataCollection dataCollectorServiceProxy, IMediator mediator)
	{
		_dataCollectorServiceProxy = dataCollectorServiceProxy;
		_mediator = mediator;
	}

	public async Task<Result<Unit>> Handle(DeleteStateServerCommand request, CancellationToken cancellationToken)
	{
		await _mediator.Send((IRequest<Result<Unit>>)new StateChangeServerCommand(request.StateChange), cancellationToken);
		_dataCollectorServiceProxy.DeleteStateData(Map(request.StateChange));
		return default(Result<Unit>);
	}

	private static CounterStateChangeData Map(StateChangeData data)
	{
		return new CounterStateChangeData(data.DataCollectorId, data.CounterRuleId, data.CounterPath, data.State, data.TriggerValue);
	}
}
