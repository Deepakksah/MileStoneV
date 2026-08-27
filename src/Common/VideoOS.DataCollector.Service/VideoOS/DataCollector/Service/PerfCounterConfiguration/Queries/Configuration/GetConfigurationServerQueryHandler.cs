using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VideoOS.DataCollector.Service.Common.ResultResponse;
using VideoOS.DataCollector.Service.Entities.DataCollection;

namespace VideoOS.DataCollector.Service.PerfCounterConfiguration.Queries.Configuration;

public class GetConfigurationServerQueryHandler : IRequestHandler<GetConfigurationServerQuery, Result<DataCollectorConfiguration>>
{
	private readonly IDataCollection _dataCollectorServiceProxy;

	public GetConfigurationServerQueryHandler(IDataCollection dataCollectorServiceProxy)
	{
		_dataCollectorServiceProxy = dataCollectorServiceProxy;
	}

	public Task<Result<DataCollectorConfiguration>> Handle(GetConfigurationServerQuery request, CancellationToken cancellationToken)
	{
		DataCollectorConfiguration dataCollectorConfiguration = _dataCollectorServiceProxy.GetDataCollectorConfiguration(request.DataCollectorId);
		Result<DataCollectorConfiguration> result = dataCollectorConfiguration;
		return Task.FromResult(result);
	}
}
