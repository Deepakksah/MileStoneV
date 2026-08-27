using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using VideoOS.DataCollector.Service.Entities;
using VideoOS.DataCollector.Service.Entities.DataCollection;
using VideoOS.DataCollector.Service.Grpc;
using VideoOS.DataCollector.Service.Persistence;
using VideoOS.DataCollector.Service.Sample.Services;
using VideoOS.DataCollector.Service.Services;

namespace VideoOS.DataCollector.Service.Sample.Queries;

public class GetPerformanceCounterValuesQueryHandler : IRequestHandler<GetPerformanceCounterValuesQuery, GetPerformanceCounterValuesResponse>
{
	private readonly IPerformanceCounterInstancesService _performanceCounterInstancesService;

	private readonly ILiveCounterRepository _liveCounterRepository;

	private readonly PerformanceCounterReaderService _reader;

	private readonly IMapper _mapper;

	private readonly ILogger<GetPerformanceCounterValuesQueryHandler> _logger;

	public GetPerformanceCounterValuesQueryHandler(IPerformanceCounterInstancesService performanceCounterInstancesService, ILiveCounterRepository liveCounterRepository, PerformanceCounterReaderService reader, IMapper mapper, ILogger<GetPerformanceCounterValuesQueryHandler> logger)
	{
		_performanceCounterInstancesService = performanceCounterInstancesService;
		_liveCounterRepository = liveCounterRepository;
		_reader = reader;
		_mapper = mapper;
		_logger = logger;
	}

	public Task<GetPerformanceCounterValuesResponse> Handle(GetPerformanceCounterValuesQuery request, CancellationToken cancellationToken)
	{
		PerformanceCounterConfiguration performanceCounterConfiguration = new PerformanceCounterConfiguration(new VideoOS.DataCollector.Service.Entities.DataCollection.PerformanceCounterInstance(request.InstanceName, request.CounterName, request.CategoryName), request.SampleFrequency);
		PerformanceCounterCategory category = new PerformanceCounterCategory(performanceCounterConfiguration.CounterInstance.CategoryName);
		Dictionary<string, VideoOS.DataCollector.Service.Entities.DataCollection.PerformanceCounterInstance> configuredPerformanceCounterInstances = _performanceCounterInstancesService.GetConfiguredPerformanceCounterInstances(category, performanceCounterConfiguration);
		List<VideoOS.DataCollector.Service.Entities.DataCollection.PerformanceCounterValue> list = new List<VideoOS.DataCollector.Service.Entities.DataCollection.PerformanceCounterValue>();
		foreach (VideoOS.DataCollector.Service.Entities.DataCollection.PerformanceCounterInstance value3 in configuredPerformanceCounterInstances.Values)
		{
			if (_liveCounterRepository.Counters.TryGetValue(value3.Path, out PerformanceCounterCollector value))
			{
				if (value.SampleFrequency != performanceCounterConfiguration.SampleFrequency)
				{
					value.SampleFrequency = performanceCounterConfiguration.SampleFrequency;
				}
				if (!value.Active)
				{
					value.Active = true;
					value.SampleData(DateTime.UtcNow, _reader);
					string message = string.Format(CultureInfo.InvariantCulture, "Re-activated live counter scheduled for removal: {0}", value3.Path);
					_logger.LogInformation(message);
				}
			}
			else
			{
				value = new PerformanceCounterCollector(value3);
				_liveCounterRepository.Counters.TryAdd(value.Path, value);
				string message2 = string.Format(CultureInfo.InvariantCulture, "Created new live counter: {0}", value3.Path);
				_logger.LogInformation(message2);
			}
			VideoOS.DataCollector.Service.Entities.DataCollection.PerformanceCounterValue value2 = value.GetValue(_reader);
			if (!(value2 is UnknownPerformanceCounterValue))
			{
				list.Add(value2);
			}
		}
		return Task.FromResult(_mapper.Map<GetPerformanceCounterValuesResponse>(list));
	}
}
