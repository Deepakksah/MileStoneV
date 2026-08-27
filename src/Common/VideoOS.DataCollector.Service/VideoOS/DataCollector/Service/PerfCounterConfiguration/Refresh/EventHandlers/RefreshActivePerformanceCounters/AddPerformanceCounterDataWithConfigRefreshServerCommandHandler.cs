using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VideoOS.DataCollector.Service.Common.ExtensionMethods;
using VideoOS.DataCollector.Service.Common.ResultResponse;
using VideoOS.DataCollector.Service.Entities.DataCollection;

namespace VideoOS.DataCollector.Service.PerfCounterConfiguration.Refresh.EventHandlers.RefreshActivePerformanceCounters;

public class AddPerformanceCounterDataWithConfigRefreshServerCommandHandler : IRequestHandler<AddPerformanceCounterDataWithConfigRefreshServerCommand, Result<Unit>>
{
	private readonly IDataCollection _dataCollectorServiceProxy;

	public AddPerformanceCounterDataWithConfigRefreshServerCommandHandler(IDataCollection dataCollectorServiceProxy)
	{
		_dataCollectorServiceProxy = dataCollectorServiceProxy;
	}

	public Task<Result<Unit>> Handle(AddPerformanceCounterDataWithConfigRefreshServerCommand request, CancellationToken cancellationToken)
	{
		PerformanceCounterValue[] values = request.CounterValues.Select((PerformanceCounterValue i) => i.FormatInstanceNameWithId()).ToArray();
		PerformanceCounterData counterData = new PerformanceCounterData(request.DataCollectorId, values);
		_dataCollectorServiceProxy.AddPerformanceCounterDataWithConfigRefresh(counterData);
		return Task.FromResult(default(Result<Unit>));
	}
}
