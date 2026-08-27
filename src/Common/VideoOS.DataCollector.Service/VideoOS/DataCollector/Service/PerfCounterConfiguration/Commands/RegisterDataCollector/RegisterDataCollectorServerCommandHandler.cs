using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VideoOS.DataCollector.Service.Common.ResultResponse;
using VideoOS.DataCollector.Service.Entities.DataCollection;

namespace VideoOS.DataCollector.Service.PerfCounterConfiguration.Commands.RegisterDataCollector;

public class RegisterDataCollectorServerCommandHandler : IRequestHandler<RegisterDataCollectorServerCommand, Result<int>>
{
	private readonly IDataCollection _dataCollectorServiceProxy;

	public RegisterDataCollectorServerCommandHandler(IDataCollection dataCollectorServiceProxy)
	{
		_dataCollectorServiceProxy = dataCollectorServiceProxy;
	}

	public Task<Result<int>> Handle(RegisterDataCollectorServerCommand request, CancellationToken cancellationToken)
	{
		int num = ((!string.IsNullOrEmpty(request.ClusterAddress)) ? _dataCollectorServiceProxy.RegisterDataCollectorNewApplicationId(request.ClusterApplicationId, request.ApplicationId, request.HostName, request.DataCollectorVersion) : _dataCollectorServiceProxy.RegisterDataCollectorWithVersion(request.ApplicationId, request.HostName, request.DataCollectorVersion));
		Result<int> result = num;
		return Task.FromResult(result);
	}
}
