using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VideoOS.DataCollector.Service.Common.Hosting;
using VideoOS.DataCollector.Service.Common.ResultResponse;
using VideoOS.DataCollector.Service.PerfCounterConfiguration.Commands.RegisterDataCollector;
using VideoOS.DataCollector.Service.PerfCounterConfiguration.Commands.RegisterServiceApplications;

namespace VideoOS.DataCollector.Service.Services.Hosted;

public class RegisterServiceApplicationsService : NotifyCompleteBase
{
	private readonly IConfigurationCache _configurationCache;

	private readonly IMediator _mediator;

	public RegisterServiceApplicationsService(IConfigurationCache configurationCache, IMediator mediator, INotifyComplete applicationStartupSync)
		: base(applicationStartupSync)
	{
		_configurationCache = configurationCache;
		_mediator = mediator;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		int dataCollectorId = await _configurationCache.GetOrAddDataCollectorIdAsync(stoppingToken);
		await _mediator.Send((IRequest<Result<Unit>>)new RegisterServiceApplicationsServerCommand(dataCollectorId), stoppingToken);
		NotifyComplete();
	}
}
