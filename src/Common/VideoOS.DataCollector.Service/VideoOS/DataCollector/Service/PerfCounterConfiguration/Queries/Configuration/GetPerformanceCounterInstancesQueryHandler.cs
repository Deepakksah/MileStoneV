using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using VideoOS.DataCollector.Service.Entities.DataCollection;
using VideoOS.DataCollector.Service.Grpc;
using VideoOS.DataCollector.Service.Services;

namespace VideoOS.DataCollector.Service.PerfCounterConfiguration.Queries.Configuration;

public class GetPerformanceCounterInstancesQueryHandler : IRequestHandler<GetPerformanceCounterInstancesQuery, GetPerformanceCounterInstancesResponse>
{
	private readonly IPerformanceCounterInstancesService _performanceCounterInstancesService;

	private readonly IMapper _mapper;

	public GetPerformanceCounterInstancesQueryHandler(IPerformanceCounterInstancesService performanceCounterInstancesService, IMapper mapper)
	{
		_performanceCounterInstancesService = performanceCounterInstancesService;
		_mapper = mapper;
	}

	public Task<GetPerformanceCounterInstancesResponse> Handle(GetPerformanceCounterInstancesQuery request, CancellationToken cancellationToken)
	{
		VideoOS.DataCollector.Service.Entities.DataCollection.PerformanceCounterInstance[] performanceCounterInstances = _performanceCounterInstancesService.GetPerformanceCounterInstances(request.CategoryName);
		VideoOS.DataCollector.Service.Grpc.PerformanceCounterInstance[] values = _mapper.Map<VideoOS.DataCollector.Service.Grpc.PerformanceCounterInstance[]>(performanceCounterInstances);
		GetPerformanceCounterInstancesResponse getPerformanceCounterInstancesResponse = new GetPerformanceCounterInstancesResponse();
		getPerformanceCounterInstancesResponse.Values.Add(values);
		return Task.FromResult(getPerformanceCounterInstancesResponse);
	}
}
