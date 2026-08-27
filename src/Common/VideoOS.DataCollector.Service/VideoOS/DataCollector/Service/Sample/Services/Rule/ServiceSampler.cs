using VideoOS.DataCollector.Service.PerfCounterConfiguration.Commands.ReportMachineConfiguration;
using VideoOS.DataCollector.Service.Rule.Entities.Sample;

namespace VideoOS.DataCollector.Service.Sample.Services.Rule;

public class ServiceSampler : IServiceSampler, IRuleSampler
{
	private readonly IServiceControlManager _serviceControlManager;

	public ServiceSampler(IServiceControlManager serviceControlManager)
	{
		_serviceControlManager = serviceControlManager;
	}

	public bool Sample(string serviceName)
	{
		return _serviceControlManager.IsRunning(serviceName);
	}
}
