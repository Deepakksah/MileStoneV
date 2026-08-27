using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VideoOS.DataCollector.Service.Common.ResultResponse;
using VideoOS.DataCollector.Service.Entities.DataCollection;

namespace VideoOS.DataCollector.Service.PerfCounterConfiguration.Commands.ReportGpuCounterInstances;

public class ReportGpuCounterInstancesServerCommandHandler : IRequestHandler<ReportGpuCounterInstancesServerCommand, Result<Unit>>
{
	private readonly IDataCollection _dataCollectorServiceProxy;

	public ReportGpuCounterInstancesServerCommandHandler(IDataCollection dataCollectorServiceProxy)
	{
		_dataCollectorServiceProxy = dataCollectorServiceProxy;
	}

	public Task<Result<Unit>> Handle(ReportGpuCounterInstancesServerCommand request, CancellationToken cancellationToken)
	{
		PerformanceCounterData counterData = new PerformanceCounterData(request.DataCollectorId, CreateGpuCounterInstances().ToArray());
		_dataCollectorServiceProxy.AddPerformanceCounterDataWithConfigRefresh(counterData);
		Result<Unit> result = default(Unit);
		return Task.FromResult(result);
	}

	private static List<PerformanceCounterValue> CreateGpuCounterInstances()
	{
		return new List<PerformanceCounterValue>
		{
			new PerformanceCounterValue("_total", "% Decoder Usage", "VideoOS Recording Server GPU Performance")
			{
				CalculatedValue = 0f,
				CollectionTime = DateTime.UtcNow,
				RawValue = 0L
			},
			new PerformanceCounterValue("_total", "% Rendering Usage", "VideoOS Recording Server GPU Performance")
			{
				CalculatedValue = 0f,
				CollectionTime = DateTime.UtcNow,
				RawValue = 0L
			},
			new PerformanceCounterValue("_total", "% Memory Usage", "VideoOS Recording Server GPU Performance")
			{
				CalculatedValue = 0f,
				CollectionTime = DateTime.UtcNow,
				RawValue = 0L
			}
		};
	}
}
