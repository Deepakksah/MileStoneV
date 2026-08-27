using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VideoOS.DataCollector.Service.Common.ExtensionMethods;
using VideoOS.DataCollector.Service.Common.ResultResponse;
using VideoOS.DataCollector.Service.Entities.DataCollection;

namespace VideoOS.DataCollector.Service.Sample.EventHandlers.Historical;

public class SendBatchToServerServerCommandHandler : IRequestHandler<SendBatchToServerServerCommand, Result<Unit>>
{
	private readonly IDataCollection _dataCollectorServiceProxy;

	public SendBatchToServerServerCommandHandler(IDataCollection dataCollectorServiceProxy)
	{
		_dataCollectorServiceProxy = dataCollectorServiceProxy;
	}

	public Task<Result<Unit>> Handle(SendBatchToServerServerCommand request, CancellationToken cancellationToken)
	{
		PerformanceCounterValue[] values = request.CounterValues.Select((PerformanceCounterValue i) => i.FormatInstanceNameWithId()).ToArray();
		PerformanceCounterData counterData = new PerformanceCounterData(request.DataCollectorId, values);
		_dataCollectorServiceProxy.AddPerformanceCounterData(counterData);
		return Task.FromResult(default(Result<Unit>));
	}
}
