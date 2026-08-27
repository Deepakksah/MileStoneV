using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VideoOS.DataCollector.Service.Grpc;
using VideoOS.DataCollector.Service.Services;

namespace VideoOS.DataCollector.Service.PerfCounterConfiguration.Queries.Configuration;

public class GetPerformanceCategoriesQueryHandler : IRequestHandler<GetPerformanceCategoriesQuery, GetPerformanceCategoriesResponse>
{
	private readonly IPerformanceCounterInstancesService _performanceCounterInstancesService;

	public GetPerformanceCategoriesQueryHandler(IPerformanceCounterInstancesService performanceCounterInstancesService)
	{
		_performanceCounterInstancesService = performanceCounterInstancesService;
	}

	public Task<GetPerformanceCategoriesResponse> Handle(GetPerformanceCategoriesQuery request, CancellationToken cancellationToken)
	{
		GetPerformanceCategoriesResponse getPerformanceCategoriesResponse = new GetPerformanceCategoriesResponse();
		getPerformanceCategoriesResponse.Values.AddRange(_performanceCounterInstancesService.GetCategoryNames());
		return Task.FromResult(getPerformanceCategoriesResponse);
	}
}
