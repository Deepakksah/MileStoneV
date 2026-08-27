using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VideoOS.DataCollector.Service.Grpc;
using VideoOS.DataCollector.Service.PerfCounterConfiguration.Commands.RegisterDataCollector;

namespace VideoOS.DataCollector.Service.PerfCounterConfiguration.Queries.Configuration;

public class GetDataCollectorIdQueryHandler : IRequestHandler<GetDataCollectorIdQuery, GetDataCollectorIdResponse>
{
	private readonly IConfigurationCache _configurationCache;

	public GetDataCollectorIdQueryHandler(IConfigurationCache configurationCache)
	{
		_configurationCache = configurationCache;
	}

	public async Task<GetDataCollectorIdResponse> Handle(GetDataCollectorIdQuery request, CancellationToken cancellationToken)
	{
		GetDataCollectorIdResponse getDataCollectorIdResponse = new GetDataCollectorIdResponse();
		GetDataCollectorIdResponse getDataCollectorIdResponse2 = getDataCollectorIdResponse;
		getDataCollectorIdResponse2.Value = await _configurationCache.GetOrAddDataCollectorIdAsync(cancellationToken);
		return getDataCollectorIdResponse;
	}
}
