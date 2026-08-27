using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VideoOS.DataCollector.Service.Common.ResultResponse;
using VideoOS.DataCollector.Service.Entities.DataCollection;
using VideoOS.DataCollector.Service.PerfCounterConfiguration.Commands.RegisterDataCollector;
using VideoOS.DataCollector.Service.PerfCounterConfiguration.Refresh.EventHandlers.RefreshActivePerformanceCounters;
using VideoOS.DataCollector.Service.PerfCounterConfiguration.Refresh.Events;
using VideoOS.DataCollector.Service.Persistence;

namespace VideoOS.DataCollector.Service.PerfCounterConfiguration.Refresh.EventHandlers.RefreshInstalledServices;

public class RefreshConfigurationInstalledServicesEventHandler : INotificationHandler<RefreshConfigurationEvent>
{
	private readonly IInstallerServicesRepository _repository;

	private readonly IInstallerServicesService _service;

	private readonly IMediator _mediator;

	private readonly IConfigurationCache _configurationCache;

	public RefreshConfigurationInstalledServicesEventHandler(IInstallerServicesRepository repository, IInstallerServicesService service, IMediator mediator, IConfigurationCache configurationCache)
	{
		_repository = repository;
		_service = service;
		_mediator = mediator;
		_configurationCache = configurationCache;
	}

	public async Task Handle(RefreshConfigurationEvent notification, CancellationToken cancellationToken)
	{
		ConcurrentDictionary<string, string> services = _repository.Services;
		List<PerformanceCounterValue> counterValues = _service.UpdateInstallerServices(services);
		if (counterValues.Any())
		{
			IMediator mediator = _mediator;
			await mediator.Send((IRequest<Result<Unit>>)new AddPerformanceCounterDataWithConfigRefreshServerCommand(await _configurationCache.GetOrAddDataCollectorIdAsync(cancellationToken), counterValues), cancellationToken);
		}
	}
}
