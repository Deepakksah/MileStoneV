using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using VideoOS.DataCollector.Service.Entities.DataCollection;
using VideoOS.DataCollector.Service.Grpc;
using VideoOS.DataCollector.Service.PerfCounterConfiguration.Queries.Configuration;
using VideoOS.DataCollector.Service.Rule.RuleConfiguration.Commands;
using VideoOS.DataCollector.Service.Sample.Queries;

namespace VideoOS.DataCollector.Service.Services;

[Authorize]
public class DataCollectorGrpcService : LiveDataCollectorService.LiveDataCollectorServiceBase
{
	private readonly IMediator _mediator;

	private readonly IMapper _mapper;

	public DataCollectorGrpcService(IMediator mediator, IMapper mapper)
	{
		_mediator = mediator;
		_mapper = mapper;
	}

	public override async Task<GetDataCollectorIdResponse> GetDataCollectorId(Empty request, ServerCallContext context)
	{
		return await _mediator.Send((IRequest<GetDataCollectorIdResponse>)new GetDataCollectorIdQuery(), default(CancellationToken));
	}

	public override async Task<GetPerformanceCategoriesResponse> GetPerformanceCategories(Empty request, ServerCallContext context)
	{
		return await _mediator.Send((IRequest<GetPerformanceCategoriesResponse>)new GetPerformanceCategoriesQuery(), default(CancellationToken));
	}

	public override async Task<GetPerformanceCounterInstancesResponse> GetPerformanceCounterInstances(GetPerformanceCounterInstancesRequest request, ServerCallContext context)
	{
		return await _mediator.Send((IRequest<GetPerformanceCounterInstancesResponse>)new GetPerformanceCounterInstancesQuery(request.CategoryName), default(CancellationToken));
	}

	public override async Task<GetPerformanceCounterValuesResponse> GetPerformanceCounterValues(GetPerformanceCounterValuesRequest request, ServerCallContext context)
	{
		return await _mediator.Send((IRequest<GetPerformanceCounterValuesResponse>)new GetPerformanceCounterValuesQuery(request.CategoryName, request.CounterName, request.InstanceName, request.SampleFrequency), default(CancellationToken));
	}

	public override Task<IsAliveResponse> IsAlive(Empty request, ServerCallContext context)
	{
		return Task.FromResult(new IsAliveResponse
		{
			Value = true
		});
	}

	public override async Task<Empty> PostUpdatedCounterStateRuleConfigurations(PostUpdatedCounterStateRuleConfigurationsRequest request, ServerCallContext context)
	{
		await _mediator.Send((IRequest<Unit>)new PostUpdatedCounterStateRuleConfigurationsCommand(_mapper.Map<VideoOS.DataCollector.Service.Entities.DataCollection.CounterStateRulesConfigurationUpdate[]>(request.RuleConfigUpdates)), default(CancellationToken));
		return new Empty();
	}
}
