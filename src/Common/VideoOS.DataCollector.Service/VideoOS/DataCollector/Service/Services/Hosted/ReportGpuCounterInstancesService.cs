using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using VideoOS.DataCollector.Media;
using VideoOS.DataCollector.Service.Common.Hosting;
using VideoOS.DataCollector.Service.Common.ResultResponse;
using VideoOS.DataCollector.Service.PerfCounterConfiguration.Commands.RegisterDataCollector;
using VideoOS.DataCollector.Service.PerfCounterConfiguration.Commands.ReportGpuCounterInstances;

namespace VideoOS.DataCollector.Service.Services.Hosted;

public class ReportGpuCounterInstancesService : WaitForNotifyCompleteBase
{
	private readonly IConfigurationCache _configurationCache;

	private readonly IMediator _mediator;

	private readonly ILogger<ReportGpuCounterInstancesService> _logger;

	public ReportGpuCounterInstancesService(IConfigurationCache configurationCache, IMediator mediator, INotifyComplete applicationStartup, ILogger<ReportGpuCounterInstancesService> logger)
		: base(applicationStartup)
	{
		_configurationCache = configurationCache;
		_mediator = mediator;
		_logger = logger;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		await WaitForNotifyComplete(stoppingToken);
		using INvidiaInformationWrapper temporaryWrapper = GetWrapper();
		if (temporaryWrapper.HasHACapability())
		{
			IMediator mediator = _mediator;
			await mediator.Send((IRequest<Result<Unit>>)new ReportGpuCounterInstancesServerCommand(await _configurationCache.GetOrAddDataCollectorIdAsync(stoppingToken)), stoppingToken);
		}
		else
		{
			_logger.LogInformation("No Nvidia card present, won't create counter instances.");
		}
	}

	public virtual INvidiaInformationWrapper GetWrapper()
	{
		return new NvidiaInformationWrapper();
	}
}
