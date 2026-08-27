using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VideoOS.DataCollector.Service.Common.ResultResponse;
using VideoOS.DataCollector.Service.Entities.DataCollection;
using VideoOS.DataCollector.Service.Rule.Entities.State;

namespace VideoOS.DataCollector.Service.Rule.RuleConfiguration.Commands;

public class StateChangeServerCommandHandler : IRequestHandler<StateChangeServerCommand, Result<Unit>>
{
	private readonly IDataCollection _dataCollectorServiceProxy;

	public StateChangeServerCommandHandler(IDataCollection dataCollectorServiceProxy)
	{
		_dataCollectorServiceProxy = dataCollectorServiceProxy;
	}

	public Task<Result<Unit>> Handle(StateChangeServerCommand request, CancellationToken cancellationToken)
	{
		_dataCollectorServiceProxy.RegisterStateChangeData(Map(request.StateChange));
		return Task.FromResult(default(Result<Unit>));
	}

	private static CounterStateChangeData Map(StateChangeData data)
	{
		return new CounterStateChangeData(data.DataCollectorId, data.CounterRuleId, data.CounterPath, data.State, data.TriggerValue);
	}
}
