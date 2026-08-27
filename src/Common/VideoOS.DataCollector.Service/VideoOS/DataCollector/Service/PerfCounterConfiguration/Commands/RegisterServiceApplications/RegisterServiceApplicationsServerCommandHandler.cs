using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VideoOS.DataCollector.Service.Common.ResultResponse;
using VideoOS.DataCollector.Service.Entities.DataCollection;

namespace VideoOS.DataCollector.Service.PerfCounterConfiguration.Commands.RegisterServiceApplications;

public class RegisterServiceApplicationsServerCommandHandler : IRequestHandler<RegisterServiceApplicationsServerCommand, Result<Unit>>
{
	private readonly IDataCollection _dataCollectorServiceProxy;

	private readonly IRegisterInstalledServicesService _registerInstalledServicesService;

	public RegisterServiceApplicationsServerCommandHandler(IDataCollection dataCollectorServiceProxy, IRegisterInstalledServicesService installedServicesService)
	{
		_dataCollectorServiceProxy = dataCollectorServiceProxy;
		_registerInstalledServicesService = installedServicesService;
	}

	public Task<Result<Unit>> Handle(RegisterServiceApplicationsServerCommand request, CancellationToken cancellationToken)
	{
		ServiceApplication[] applicationList = _dataCollectorServiceProxy.GetServiceApplicationList(request.DataCollectorId);
		_registerInstalledServicesService.EnumServiceApplications(ref applicationList);
		_dataCollectorServiceProxy.RegisterServiceApplication(request.DataCollectorId, applicationList);
		Result<Unit> result = default(Unit);
		return Task.FromResult(result);
	}
}
