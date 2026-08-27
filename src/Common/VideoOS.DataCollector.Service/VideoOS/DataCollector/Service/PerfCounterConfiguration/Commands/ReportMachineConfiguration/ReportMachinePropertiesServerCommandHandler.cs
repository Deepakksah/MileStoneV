using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VideoOS.DataCollector.Service.Common.ResultResponse;
using VideoOS.DataCollector.Service.Entities.DataCollection;

namespace VideoOS.DataCollector.Service.PerfCounterConfiguration.Commands.ReportMachineConfiguration;

public class ReportMachinePropertiesServerCommandHandler : IRequestHandler<ReportMachinePropertiesServerCommand, Result<Unit>>
{
	private readonly IDataCollection _dataCollectorServiceProxy;

	private readonly IMachineProperties _machineProperties;

	public ReportMachinePropertiesServerCommandHandler(IDataCollection dataCollectorServiceProxy, IMachineProperties machineProperties)
	{
		_dataCollectorServiceProxy = dataCollectorServiceProxy;
		_machineProperties = machineProperties;
	}

	public Task<Result<Unit>> Handle(ReportMachinePropertiesServerCommand request, CancellationToken cancellationToken)
	{
		_dataCollectorServiceProxy.UpdateMachineProperties(request.DataCollectorId, _machineProperties.Properties);
		Result<Unit> result = default(Unit);
		return Task.FromResult(result);
	}
}
