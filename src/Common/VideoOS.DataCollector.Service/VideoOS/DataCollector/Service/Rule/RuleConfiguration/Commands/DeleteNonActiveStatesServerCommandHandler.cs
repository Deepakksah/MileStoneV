using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VideoOS.DataCollector.Service.Common.ResultResponse;
using VideoOS.DataCollector.Service.Entities.DataCollection;

namespace VideoOS.DataCollector.Service.Rule.RuleConfiguration.Commands;

public class DeleteNonActiveStatesServerCommandHandler : IRequestHandler<DeleteNonActiveStatesServerCommand, Result<Unit>>
{
	private readonly IDataCollection _dataCollectorServiceProxy;

	public DeleteNonActiveStatesServerCommandHandler(IDataCollection dataCollectorServiceProxy)
	{
		_dataCollectorServiceProxy = dataCollectorServiceProxy;
	}

	public Task<Result<Unit>> Handle(DeleteNonActiveStatesServerCommand request, CancellationToken cancellationToken)
	{
		_dataCollectorServiceProxy.DeleteStateDataFromDataCollector(request.DataCollectorId, request.ActiveRules.ToArray());
		return Task.FromResult(default(Result<Unit>));
	}
}
