using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VideoOS.DataCollector.Service.Common.ResultResponse;
using VideoOS.DataCollector.Service.Entities.DataCollection;

namespace VideoOS.DataCollector.Service.PerfCounterConfiguration.Refresh.EventHandlers.RefreshActivePerformanceCounters;

public class MarkPerformanceCounterDataAsObsoleteServerCommandHandler : IRequestHandler<MarkPerformanceCounterDataAsObsoleteServerCommand, Result<Unit>>
{
	private readonly IDataCollection _dataCollectorServiceProxy;

	public MarkPerformanceCounterDataAsObsoleteServerCommandHandler(IDataCollection dataCollectorServiceProxy)
	{
		_dataCollectorServiceProxy = dataCollectorServiceProxy;
	}

	public Task<Result<Unit>> Handle(MarkPerformanceCounterDataAsObsoleteServerCommand request, CancellationToken cancellationToken)
	{
		_dataCollectorServiceProxy.MarkPerformanceCounterDataAsObsolete(request.ObsoleteCounterData);
		return Task.FromResult(default(Result<Unit>));
	}
}
