namespace VideoOS.DataCollector.Service.PerfCounterConfiguration.Commands.ReportMachineConfiguration;

public interface IServiceControlManager
{
	bool IsServiceInstalled(string serviceName);

	bool IsRunning(string serviceName);
}
